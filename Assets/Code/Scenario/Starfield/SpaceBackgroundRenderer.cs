using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Code.Scenario.Starfield
{
    /// <summary>
    /// Draws the procedural space background: star layers and nebula
    /// layers composed in one pass, from the farthest to the nearest.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Star and nebula layers come from separate assets and are merged by
    /// parallax factor, so a nebula sits between star layers simply by
    /// being configured at a depth between them.
    /// </para>
    /// <para>
    /// Everything is a function of the camera position, the seeds and
    /// <see cref="NebulaTime"/>. Data reaches the shader through a
    /// MaterialPropertyBlock, which also lets the editor preview the
    /// background without Play mode and without touching the material.
    /// </para>
    /// </remarks>
    [ExecuteAlways]
    [RequireComponent(typeof(MeshRenderer))]
    public class SpaceBackgroundRenderer : MonoBehaviour
    {
        /// <summary>Must match BACKGROUND_MAX_LAYERS in the shader.</summary>
        public const int MaxComposedLayers =
            StarfieldLayerData.MaxLayers + NebulaLayerData.MaxLayers;

        private const float KindStars = 0f;
        private const float KindNebula = 1f;

        private static readonly int StarLayerParamsId =
            Shader.PropertyToID("_StarLayerParams");

        private static readonly int NebulaLayerParamsId =
            Shader.PropertyToID("_NebulaLayerParams");

        private static readonly int NebulaPhaseId =
            Shader.PropertyToID("_NebulaPhase");

        private static readonly int LayerOrderId =
            Shader.PropertyToID("_LayerOrder");

        private static readonly int LayerOrderCountId =
            Shader.PropertyToID("_LayerOrderCount");

        private static readonly int NebulaStarHaloId =
            Shader.PropertyToID("_NebulaStarHalo");

        private static readonly int NebulaStarClusterId =
            Shader.PropertyToID("_NebulaStarCluster");

        private static readonly int GradientAtlasId =
            Shader.PropertyToID("_GradientAtlas");

        private static readonly int PixelsPerUnitId =
            Shader.PropertyToID("_PixelsPerUnit");

        private static readonly int CameraPositionId =
            Shader.PropertyToID("_CameraPosition");

        private static readonly int StarfieldTimeId =
            Shader.PropertyToID("_StarfieldTime");

        [Header("Configuration")]
        [SerializeField]
        private StarfieldConfig config;

        [SerializeField]
        private NebulaConfig nebulaConfig;

        [Tooltip("Defaults to the main camera.")]
        [SerializeField]
        private Camera targetCamera;

        [Header("Rendering")]
        [FormerlySerializedAs("starfieldMaterial")]
        [SerializeField]
        private Material material;

        [SerializeField]
        [Range(1f, 2f)]
        private float quadOverscan = 1.2f;

        [Tooltip(
            "Sprite pixels per world unit. Stars and nebulae snap to this " +
            "grid so they match the pixel art. 0 renders them smooth at " +
            "screen resolution."
        )]
        [SerializeField]
        [Min(0f)]
        private float pixelsPerUnit = 32f;

        [Header("Gradient")]
        [SerializeField]
        [Min(2)]
        private int gradientResolution = 256;

        private readonly Vector4[] _starParams =
            new Vector4[StarfieldLayerData.ArrayLength];

        private readonly Vector4[] _nebulaParams =
            new Vector4[NebulaLayerData.ArrayLength];

        private readonly float[] _nebulaPhases =
            new float[NebulaLayerData.MaxLayers];

        private readonly float[] _nebulaPeriods =
            new float[NebulaLayerData.MaxLayers];

        private readonly Vector4[] _layerOrder =
            new Vector4[MaxComposedLayers];

        private readonly NebulaClock _clock = new();

        private MaterialPropertyBlock _properties;
        private MeshRenderer _meshRenderer;
        private Texture2D _gradientAtlas;

        private int _nebulaCount;
        private bool _dirty = true;

        /// <summary>
        /// Time driving the nebula flow. Save and restore it to bring the
        /// clouds back exactly as they were.
        /// </summary>
        public double NebulaTime
        {
            get => _clock.Seconds;
            set => _clock.Seconds = value;
        }

        private Camera TargetCamera =>
            targetCamera != null ? targetCamera : Camera.main;

        private void OnEnable()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _properties ??= new MaterialPropertyBlock();
            _dirty = true;

            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            SpaceBackgroundAssets.Changed += MarkDirty;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            SpaceBackgroundAssets.Changed -= MarkDirty;

            ReleaseResources();
        }

        private void Update()
        {
            // Only game time moves the clouds: a paused game
            // (timeScale 0) and the editor preview keep them still.
            if (Application.isPlaying)
                _clock.Advance(Time.deltaTime);
        }

        private void OnValidate()
        {
            gradientResolution = Mathf.Max(2, gradientResolution);
            _dirty = true;
        }

        private void MarkDirty()
        {
            _dirty = true;
        }

        private void OnBeginCameraRendering(
            ScriptableRenderContext context,
            Camera camera)
        {
            if (camera != TargetCamera)
                return;

            // Rebuilt lazily here rather than in OnValidate, which runs
            // in the middle of serialization where creating textures and
            // touching renderers is not allowed.
            if (_dirty)
                Rebuild();

            UpdateFrameParameters(camera);
        }

        [ContextMenu("Rebuild Background")]
        public void Rebuild()
        {
            _dirty = false;

            ReleaseResources();

            if (config == null || config.layers == null)
                return;

            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();

            if (material != null && _meshRenderer.sharedMaterial != material)
                _meshRenderer.sharedMaterial = material;

            int starCount = WriteStarLayers();
            List<NebulaPalette> palettes = WriteNebulaLayers(config.layers.Count);
            int orderCount = WriteLayerOrder(starCount);

            try
            {
                _gradientAtlas = StarfieldGradientBaker.Bake(
                    config,
                    palettes,
                    gradientResolution
                );

                if (_gradientAtlas != null)
                    _gradientAtlas.hideFlags = HideFlags.DontSave;
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, this);
                ReleaseResources();
                return;
            }

            _properties.Clear();

            // Arrays are always set at full length: Unity fixes an array's
            // size the first time it is set on a property block.
            _properties.SetVectorArray(StarLayerParamsId, _starParams);
            _properties.SetVectorArray(NebulaLayerParamsId, _nebulaParams);
            _properties.SetVectorArray(LayerOrderId, _layerOrder);
            _properties.SetInteger(LayerOrderCountId, orderCount);
            _properties.SetFloatArray(NebulaPhaseId, _nebulaPhases);

            _properties.SetFloat(
                NebulaStarHaloId,
                nebulaConfig != null ? nebulaConfig.starHalo : 0f
            );

            _properties.SetFloat(
                NebulaStarClusterId,
                nebulaConfig != null ? nebulaConfig.starCluster : 0f
            );

            _properties.SetFloat(PixelsPerUnitId, pixelsPerUnit);

            if (_gradientAtlas != null)
                _properties.SetTexture(GradientAtlasId, _gradientAtlas);

            _meshRenderer.SetPropertyBlock(_properties);
        }

        /// <returns>Number of star layers written.</returns>
        private int WriteStarLayers()
        {
            Array.Clear(_starParams, 0, _starParams.Length);

            int count = Mathf.Min(config.layers.Count, StarfieldLayerData.MaxLayers);

            if (config.layers.Count > count)
            {
                Debug.LogWarning(
                    $"Starfield has {config.layers.Count} layers; only the " +
                    $"first {StarfieldLayerData.MaxLayers} are rendered.",
                    this
                );
            }

            // A null layer stays zeroed, which the shader reads as disabled.
            for (var i = 0; i < count; i++)
            {
                if (config.layers[i] != null)
                    StarfieldLayerData.Write(config.layers[i], config.worldSeed, _starParams, i);
            }

            return count;
        }

        /// <param name="firstPaletteRow">
        /// Atlas row of the first nebula palette, right after the star rows.
        /// </param>
        /// <returns>The palettes to bake, one per nebula layer written.</returns>
        private List<NebulaPalette> WriteNebulaLayers(int firstPaletteRow)
        {
            Array.Clear(_nebulaParams, 0, _nebulaParams.Length);
            Array.Clear(_nebulaPhases, 0, _nebulaPhases.Length);

            var palettes = new List<NebulaPalette>();
            _nebulaCount = 0;

            if (nebulaConfig == null || nebulaConfig.layers == null)
                return palettes;

            _nebulaCount = Mathf.Min(nebulaConfig.layers.Count, NebulaLayerData.MaxLayers);

            if (nebulaConfig.layers.Count > _nebulaCount)
            {
                Debug.LogWarning(
                    $"Nebula config has {nebulaConfig.layers.Count} layers; only " +
                    $"the first {NebulaLayerData.MaxLayers} are rendered.",
                    nebulaConfig
                );
            }

            for (var i = 0; i < _nebulaCount; i++)
            {
                NebulaLayer layer = nebulaConfig.layers[i];

                palettes.Add(layer?.palette);

                if (layer == null)
                    continue;

                NebulaLayerData.Write(
                    layer,
                    nebulaConfig,
                    firstPaletteRow + i,
                    _nebulaParams,
                    i
                );

                _nebulaPeriods[i] = layer.cyclePeriod;
            }

            return palettes;
        }

        /// <summary>
        /// Merges star and nebula layers by parallax, from the farthest
        /// (lowest factor) to the nearest. On a tie the star layer goes
        /// first, so the nebula covers it.
        /// </summary>
        /// <returns>Number of layers in the order.</returns>
        private int WriteLayerOrder(int starCount)
        {
            var entries = new List<(float parallax, float kind, int index)>();

            for (var i = 0; i < starCount; i++)
            {
                if (config.layers[i] != null)
                    entries.Add((config.layers[i].parallaxFactor, KindStars, i));
            }

            for (var i = 0; i < _nebulaCount; i++)
            {
                NebulaLayer layer = nebulaConfig.layers[i];

                if (layer != null)
                    entries.Add((layer.parallaxFactor, KindNebula, i));
            }

            // OrderBy is stable, unlike List.Sort: layers at the same depth
            // keep their order from the configs.
            entries = entries
                .OrderBy(entry => entry.parallax)
                .ThenBy(entry => entry.kind)
                .ToList();

            Array.Clear(_layerOrder, 0, _layerOrder.Length);

            for (var i = 0; i < entries.Count; i++)
                _layerOrder[i] = new Vector4(entries[i].kind, entries[i].index, 0f, 0f);

            return entries.Count;
        }

        private void UpdateFrameParameters(Camera camera)
        {
            if (_gradientAtlas == null || !camera.orthographic)
                return;

            Vector3 position = camera.transform.position;

            float height = camera.orthographicSize * 2f;
            float width = height * camera.aspect;

            transform.position = new Vector3(position.x, position.y, transform.position.z);
            transform.localScale = new Vector3(width * quadOverscan, height * quadOverscan, 1f);

            float frameRate = nebulaConfig != null ? nebulaConfig.animationFrameRate : 0f;

            for (var i = 0; i < _nebulaCount; i++)
                _nebulaPhases[i] = _clock.Phase(_nebulaPeriods[i], frameRate);

            _properties.SetVector(CameraPositionId, new Vector4(position.x, position.y, 0f, 0f));
            _properties.SetFloat(StarfieldTimeId, Application.isPlaying ? Time.time : 0f);
            _properties.SetFloatArray(NebulaPhaseId, _nebulaPhases);

            _meshRenderer.SetPropertyBlock(_properties);
        }

        private void ReleaseResources()
        {
            if (_meshRenderer != null)
                _meshRenderer.SetPropertyBlock(null);

            _nebulaCount = 0;

            if (_gradientAtlas == null)
                return;

            if (Application.isPlaying)
                Destroy(_gradientAtlas);
            else
                DestroyImmediate(_gradientAtlas);

            _gradientAtlas = null;
        }
    }
}

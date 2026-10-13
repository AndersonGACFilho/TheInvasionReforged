using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Scenario.Starfield
{
    [RequireComponent(typeof(MeshRenderer))]
    public class StarfieldRenderer : MonoBehaviour
    {
        private static readonly int LayerCountId =
            Shader.PropertyToID("_LayerCount");

        private static readonly int LayerParamsId =
            Shader.PropertyToID("_LayerParams");

        private static readonly int GradientAtlasId =
            Shader.PropertyToID("_GradientAtlas");

        private static readonly int CameraPositionId =
            Shader.PropertyToID("_CameraPosition");

        private static readonly int CameraWorldSizeId =
            Shader.PropertyToID("_CameraWorldSize");

        private static readonly int StarfieldTimeId =
            Shader.PropertyToID("_StarfieldTime");

        [Header("Configuration")] [SerializeField]
        private StarfieldConfig config;

        [SerializeField] private Camera targetCamera;

        [Header("Rendering")] [SerializeField] private Material starfieldMaterial;

        [SerializeField] [Range(1f, 2f)] private float quadOverscan = 1.2f;

        [Header("Gradient")] [SerializeField] [Min(2)]
        private int gradientResolution = 256;

        /// <summary>
        ///     Layer settings packed as float4 values. See
        ///     <see cref="StarfieldLayerData" /> for the layout.
        /// </summary>
        private readonly Vector4[] _layerParams =
            new Vector4[StarfieldLayerData.ArrayLength];

        private bool _configurationDirty = true;

        private Texture2D _gradientAtlas;

        /// <summary>
        ///     Number of layers uploaded to the shader, at most
        ///     <see cref="StarfieldLayerData.MaxLayers" />.
        /// </summary>
        private int _layerCount;

        private MeshRenderer _meshRenderer;
        private Material _originalMaterial;
        private Material _runtimeMaterial;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();

            if (targetCamera == null)
                targetCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_configurationDirty)
                Rebuild();

            SyncLayerVisibility();
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            Rebuild();
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
            ReleaseResources();
        }

        private void OnDestroy()
        {
            ReleaseResources();
        }

        private void OnValidate()
        {
            gradientResolution = Mathf.Max(
                2,
                gradientResolution
            );

            _configurationDirty = true;
        }

        private void OnBeginCameraRendering(
            ScriptableRenderContext context,
            Camera camera)
        {
            if (camera != targetCamera)
                return;

            UpdateCameraParameters();
        }

        [ContextMenu("Rebuild Starfield")]
        public void Rebuild()
        {
            _configurationDirty = false;

            ReleaseResources();

            if (config == null ||
                starfieldMaterial == null ||
                config.layers == null ||
                config.layers.Count == 0)
            {
                return;
            }

            if (_meshRenderer == null)
                _meshRenderer = GetComponent<MeshRenderer>();

            _originalMaterial = _meshRenderer.sharedMaterial;

            int count = config.layers.Count;

            if (count > StarfieldLayerData.MaxLayers)
            {
                Debug.LogWarning(
                    $"Starfield has {count} layers; only the first " +
                    $"{StarfieldLayerData.MaxLayers} are rendered.",
                    this
                );

                count = StarfieldLayerData.MaxLayers;
            }

            // Unused slots stay zeroed, which the shader reads as disabled.
            Array.Clear(_layerParams, 0, _layerParams.Length);

            for (int i = 0; i < count; i++)
            {
                if (config.layers[i] == null)
                {
                    Debug.LogError(
                        $"Starfield layer {i} is null.",
                        this
                    );
                    return;
                }

                StarfieldLayerData.Write(
                    config.layers[i],
                    config.worldSeed,
                    _layerParams,
                    i
                );
            }

            _layerCount = count;

            try
            {
                _gradientAtlas = StarfieldGradientBaker.Bake(
                    config,
                    gradientResolution
                );

                _runtimeMaterial = new Material(starfieldMaterial)
                {
                    name = starfieldMaterial.name + " (Runtime)"
                };

                _runtimeMaterial.SetVectorArray(
                    LayerParamsId,
                    _layerParams
                );

                _runtimeMaterial.SetTexture(
                    GradientAtlasId,
                    _gradientAtlas
                );

                _runtimeMaterial.SetInteger(LayerCountId, count);

                _meshRenderer.sharedMaterial = _runtimeMaterial;

                UpdateCameraParameters();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception, this);
                ReleaseResources();
            }
        }

        private void SyncLayerVisibility()
        {
            if (_runtimeMaterial == null ||
                config?.layers == null ||
                config.layers.Count < _layerCount)
            {
                return;
            }

            bool changed = false;

            for (var i = 0; i < _layerCount; i++)
            {
                StarLayer layer = config.layers[i];

                if (layer == null)
                    continue;

                changed |= StarfieldLayerData.WriteEnabled(
                    layer.enabled,
                    _layerParams,
                    i
                );
            }

            if (changed)
            {
                _runtimeMaterial.SetVectorArray(
                    LayerParamsId,
                    _layerParams
                );
            }
        }

        private void UpdateCameraParameters()
        {
            if (_runtimeMaterial == null ||
                targetCamera == null)
                return;

            if (!targetCamera.orthographic)
                return;

            Vector3 position = targetCamera.transform.position;

            float height = targetCamera.orthographicSize * 2f;
            float width = height * targetCamera.aspect;

            _runtimeMaterial.SetVector(
                CameraPositionId,
                new Vector4(position.x, position.y, 0f, 0f)
            );

            _runtimeMaterial.SetVector(
                CameraWorldSizeId,
                new Vector4(width, height, 0f, 0f)
            );

            _runtimeMaterial.SetFloat(
                StarfieldTimeId,
                Time.time
            );

            transform.position = new Vector3(
                position.x,
                position.y,
                transform.position.z
            );

            transform.localScale = new Vector3(
                width * quadOverscan,
                height * quadOverscan,
                1f
            );
        }

        private void ReleaseResources()
        {
            if (_meshRenderer != null && _runtimeMaterial != null &&
                _meshRenderer.sharedMaterial == _runtimeMaterial)
            {
                _meshRenderer.sharedMaterial = _originalMaterial;
            }

            _layerCount = 0;

            if (_gradientAtlas != null)
            {
                Destroy(_gradientAtlas);
                _gradientAtlas = null;
            }

            if (_runtimeMaterial != null)
            {
                Destroy(_runtimeMaterial);
                _runtimeMaterial = null;
            }
        }
    }
}

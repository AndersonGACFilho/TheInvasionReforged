using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Scenario.Starfield
{
    [RequireComponent(typeof(MeshRenderer))]
    public class StarfieldRenderer : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField]
        private StarfieldConfig config;

        [SerializeField]
        private Camera targetCamera;

        [Header("Rendering")]
        [SerializeField]
        private Material starfieldMaterial;
        [SerializeField, Range(1f, 2f)]
        private float quadOverscan = 1.2f;

        [Header("Gradient")]
        [SerializeField, Min(2)]
        private int gradientResolution = 256;

        private MeshRenderer _meshRenderer;
        private Material _runtimeMaterial;
        private Material _originalMaterial;

        private GraphicsBuffer _layerBuffer;
        private StarfieldLayerData[] _layerData;
        private Texture2D _gradientAtlas;

        private bool _configurationDirty = true;

        private static readonly int LayerCountId =
            Shader.PropertyToID("_LayerCount");

        private static readonly int StarLayersId =
            Shader.PropertyToID("_StarLayers");

        private static readonly int GradientAtlasId =
            Shader.PropertyToID("_GradientAtlas");

        private static readonly int WorldSeedId =
            Shader.PropertyToID("_WorldSeed");

        private static readonly int CameraPositionId =
            Shader.PropertyToID("_CameraPosition");

        private static readonly int CameraWorldSizeId =
            Shader.PropertyToID("_CameraWorldSize");

        private static readonly int StarfieldTimeId =
            Shader.PropertyToID("_StarfieldTime");

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();

            if (targetCamera == null)
                targetCamera = Camera.main;
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
            Rebuild();
        }
        
        private void OnBeginCameraRendering(
            ScriptableRenderContext context,
            Camera camera)
        {
            if (camera != targetCamera)
                return;

            UpdateCameraParameters();
        }

        private void LateUpdate()
        {
            if (_configurationDirty)
                Rebuild();

            SyncLayerVisibility();
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

            _layerData = new StarfieldLayerData[count];

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

                _layerData[i] =
                    StarfieldLayerData.FromConfig(config.layers[i]);
            }

            try
            {
                int stride =
                    Marshal.SizeOf<StarfieldLayerData>();

                _layerBuffer = new GraphicsBuffer(
                    GraphicsBuffer.Target.Structured,
                    count,
                    stride
                );

                _layerBuffer.SetData(_layerData);

                _gradientAtlas = StarfieldGradientBaker.Bake(
                    config,
                    gradientResolution
                );

                _runtimeMaterial = new Material(starfieldMaterial)
                {
                    name = starfieldMaterial.name + " (Runtime)"
                };

                _runtimeMaterial.SetBuffer(
                    StarLayersId,
                    _layerBuffer
                );

                _runtimeMaterial.SetTexture(
                    GradientAtlasId,
                    _gradientAtlas
                );

                _runtimeMaterial.SetInt(LayerCountId, count);

                _runtimeMaterial.SetInt(
                    WorldSeedId,
                    config.worldSeed
                );

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
            if (_layerBuffer == null ||
                _layerData == null ||
                config?.layers == null ||
                config.layers.Count != _layerData.Length)
            {
                return;
            }

            bool changed = false;

            for (int i = 0; i < _layerData.Length; i++)
            {
                StarLayer layer = config.layers[i];

                if (layer == null)
                    continue;

                uint enabled = layer.enabled ? 1u : 0u;

                if (_layerData[i].Enabled == enabled)
                    continue;

                _layerData[i].Enabled = enabled;
                changed = true;
            }

            if (changed)
                _layerBuffer.SetData(_layerData);
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

        private void OnValidate()
        {
            gradientResolution = Mathf.Max(
                2,
                gradientResolution
            );

            _configurationDirty = true;
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

        private void ReleaseResources()
        {
            if (_meshRenderer != null && _runtimeMaterial != null &&
                _meshRenderer.sharedMaterial == _runtimeMaterial)
            {
                _meshRenderer.sharedMaterial = _originalMaterial;
            }

            _layerBuffer?.Release();
            _layerBuffer = null;

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

using UnityEngine;

namespace Standard_Assets.Effects.ImageEffects.Scripts
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
    public class EdgeDetection : PostEffectsBase
    {
        public enum EdgeDetectMode
        {
            TriangleDepthNormals = 0,
            RobertsCrossDepthNormals = 1,
            SobelDepth = 2,
            SobelDepthThin = 3,
            TriangleLuminance = 4
        }

        private Material edgeDetectMaterial;

        public Shader edgeDetectShader;
        public float edgeExp = 1.0f;
        public float edgesOnly;
        public Color edgesOnlyBgColor = Color.white;
        public float lumThreshold = 0.2f;


        public EdgeDetectMode mode = EdgeDetectMode.SobelDepthThin;
        private EdgeDetectMode oldMode = EdgeDetectMode.SobelDepthThin;
        public float sampleDist = 1.0f;
        public float sensitivityDepth = 1.0f;
        public float sensitivityNormals = 1.0f;


        public override bool CheckResources()
        {
            CheckSupport(true);

            edgeDetectMaterial = CheckShaderAndCreateMaterial(edgeDetectShader, edgeDetectMaterial);
            if (mode != oldMode)
                SetCameraFlag();

            oldMode = mode;

            if (!isSupported)
                ReportAutoDisable();
            return isSupported;
        }


        private new void Start()
        {
            oldMode = mode;
        }

        private void SetCameraFlag()
        {
            if (mode == EdgeDetectMode.SobelDepth || mode == EdgeDetectMode.SobelDepthThin)
                GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
            else if (mode == EdgeDetectMode.TriangleDepthNormals || mode == EdgeDetectMode.RobertsCrossDepthNormals)
                GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
        }

        private void OnEnable()
        {
            SetCameraFlag();
        }

        [ImageEffectOpaque]
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (CheckResources() == false)
            {
                Graphics.Blit(source, destination);
                return;
            }

            var sensitivity = new Vector2(sensitivityDepth, sensitivityNormals);
            edgeDetectMaterial.SetVector("_Sensitivity",
                new Vector4(sensitivity.x, sensitivity.y, 1.0f, sensitivity.y));
            edgeDetectMaterial.SetFloat("_BgFade", edgesOnly);
            edgeDetectMaterial.SetFloat("_SampleDistance", sampleDist);
            edgeDetectMaterial.SetVector("_BgColor", edgesOnlyBgColor);
            edgeDetectMaterial.SetFloat("_Exponent", edgeExp);
            edgeDetectMaterial.SetFloat("_Threshold", lumThreshold);

            Graphics.Blit(source, destination, edgeDetectMaterial, (int) mode);
        }
    }
}
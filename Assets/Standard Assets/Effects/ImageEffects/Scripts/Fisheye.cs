using UnityEngine;

namespace Standard_Assets.Effects.ImageEffects.Scripts
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Displacement/Fisheye")]
    public class Fisheye : PostEffectsBase
    {
        private Material fisheyeMaterial;

        public Shader fishEyeShader;

        [Range(0.0f, 1.5f)] public float strengthX = 0.05f;

        [Range(0.0f, 1.5f)] public float strengthY = 0.05f;


        public override bool CheckResources()
        {
            CheckSupport(false);
            fisheyeMaterial = CheckShaderAndCreateMaterial(fishEyeShader, fisheyeMaterial);

            if (!isSupported)
                ReportAutoDisable();
            return isSupported;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (CheckResources() == false)
            {
                Graphics.Blit(source, destination);
                return;
            }

            var oneOverBaseSize = 80.0f / 512.0f; // to keep values more like in the old version of fisheye

            var ar = source.width * 1.0f / (source.height * 1.0f);

            fisheyeMaterial.SetVector("intensity",
                new Vector4(strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize,
                    strengthX * ar * oneOverBaseSize, strengthY * oneOverBaseSize));
            Graphics.Blit(source, destination, fisheyeMaterial);
        }
    }
}
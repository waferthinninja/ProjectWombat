using UnityEngine;

namespace Standard_Assets.Effects.ImageEffects.Scripts
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Displacement/Vortex")]
    public class Vortex : ImageEffectBase
    {
        public float angle = 50;
        public Vector2 center = new Vector2(0.5F, 0.5F);
        public Vector2 radius = new Vector2(0.4F, 0.4F);

        // Called by camera to apply image effect
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            ImageEffects.RenderDistortion(material, source, destination, angle, center, radius);
        }
    }
}
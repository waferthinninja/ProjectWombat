using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts._Simple2DLine
{
    public class UniformTexturedLine : MonoBehaviour
    {
        public Texture lineTexture;
        public float lineWidth = 8.0f;
        public float textureScale = 1.0f;

        private void Start()
        {
            // Make a Vector2 list with 2 elements...
            var linePoints = new List<Vector2>();
            linePoints.Add(new Vector2(0,
                Random.Range(0, Screen.height / 2))); // ...one on the left side of the screen somewhere
            linePoints.Add(new Vector2(Screen.width - 1, Random.Range(0, Screen.height))); // ...and one on the right

            // Make a VectorLine object using the above points, with the texture as specified in the inspector, and set the texture scale
            var line = new VectorLine("Line", linePoints, lineTexture, lineWidth);
            line.textureScale = textureScale;

            // Draw the line
            line.Draw();
        }
    }
}
// This script animates a partial line segment in a spline

using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts.PartialLine
{
    public class AnimatePartialLine : MonoBehaviour
    {
        private float endIndex;
        private VectorLine line;

        public Texture lineTexture;
        public int segments = 60;
        public float speed = 60.0f;
        private float startIndex;
        public int visibleLineSegments = 20;

        private void Start()
        {
            startIndex = -visibleLineSegments;
            endIndex = 0;

            // Make Vector2 array where the size is the number of segments plus one, since we'll use a continuous line
            var linePoints = new List<Vector2>(segments + 1);
            // Make a VectorLine object using the above points, with a width of 30 pixels
            line = new VectorLine("Spline", linePoints, lineTexture, 30.0f, LineType.Continuous, Joins.Weld);
            var sw = Screen.width / 5;
            var sh = Screen.height / 3;
            line.MakeSpline(new[]
            {
                new Vector2(sw, sh), new Vector2(sw * 2, sh * 2), new Vector2(sw * 3, sh * 2), new Vector2(sw * 4, sh)
            });
        }

        private void Update()
        {
            // Change startIndex and endIndex over time, wrapping around as necessary
            startIndex += Time.deltaTime * speed;
            endIndex += Time.deltaTime * speed;
            if (startIndex >= segments + 1)
            {
                startIndex = -visibleLineSegments;
                endIndex = 0;
            }
            else if (startIndex < -visibleLineSegments)
            {
                startIndex = segments;
                endIndex = segments + visibleLineSegments;
            }

            line.drawStart = (int) startIndex;
            line.drawEnd = (int) endIndex;
            line.Draw();
        }
    }
}
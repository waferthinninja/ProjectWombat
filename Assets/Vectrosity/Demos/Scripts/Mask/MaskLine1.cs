using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts.Mask
{
    public class MaskLine1 : MonoBehaviour
    {
        public Color lineColor = Color.green;
        public GameObject mask;
        public float moveSpeed = 2.0f;

        public int numberOfRects = 30;
        private VectorLine rectLine;
        private Vector3 startPos;
        private float t;

        private void Start()
        {
            rectLine = new VectorLine("Rects", new List<Vector3>(numberOfRects * 8), 2.0f);
            var idx = 0;
            for (var i = 0; i < numberOfRects; i++)
            {
                rectLine.MakeRect(
                    new Rect(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(0.25f, 3.0f),
                        Random.Range(0.25f, 2.0f)), idx);
                idx += 8;
            }

            rectLine.color = lineColor;
            rectLine.capLength = 1.0f;
            rectLine.drawTransform = transform;
            rectLine.SetMask(mask);

            startPos = transform.position;
        }

        private void Update()
        {
            // Move this transform around in a circle, and the line uses the same movement since it's using this transform with .drawTransform
            t = Mathf.Repeat(t + Time.deltaTime * moveSpeed, 360.0f);
            transform.position = new Vector2(startPos.x + Mathf.Sin(t) * 1.5f, startPos.y + Mathf.Cos(t) * 1.5f);
            rectLine.Draw();
        }
    }
}
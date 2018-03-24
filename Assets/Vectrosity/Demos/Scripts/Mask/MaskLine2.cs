using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts.Mask
{
    public class MaskLine2 : MonoBehaviour
    {
        public Color lineColor = Color.yellow;
        public float lineHeight = 17.0f;
        public float lineWidth = 9.0f;
        public GameObject mask;

        public int numberOfPoints = 100;
        private VectorLine spikeLine;
        private Vector3 startPos;
        private float t;

        private void Start()
        {
            spikeLine = new VectorLine("SpikeLine", new List<Vector3>(numberOfPoints), 2.0f, LineType.Continuous);
            var y = lineHeight / 2;
            for (var i = 0; i < numberOfPoints; i++)
            {
                spikeLine.points3[i] = new Vector2(Random.Range(-lineWidth / 2, lineWidth / 2), y);
                y -= lineHeight / numberOfPoints;
            }

            spikeLine.color = lineColor;
            spikeLine.drawTransform = transform;
            spikeLine.SetMask(mask);

            startPos = transform.position;
        }

        private void Update()
        {
            // Move this transform around in a circle, and the line uses the same movement since it's using this transform with .drawTransform
            t = Mathf.Repeat(t + Time.deltaTime, 360.0f);
            transform.position = new Vector2(startPos.x, startPos.y + Mathf.Cos(t) * 4);
            spikeLine.Draw();
        }
    }
}
// Makes an efficient randomly-animated 3/4 circle power bar using MakeArc and SetColor

using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts.PowerBar
{
    public class PowerBar : MonoBehaviour
    {
        private VectorLine bar;
        private float currentPower;
        public int lineWidth = 25;
        private Vector2 position;
        public float radius = 60.0f;
        public int segmentCount = 200;

        public float speed = 0.25f;
        private float targetPower;

        private void Start()
        {
            position = new Vector2(radius + 20, Screen.height - (radius + 20));

            // Set up a white circle for the background of the power bar
            var circle = new VectorLine("BarBackground", new List<Vector2>(50), null, lineWidth, LineType.Continuous,
                Joins.Weld);
            circle.MakeCircle(position, radius);
            circle.Draw();

            // Make the power bar by drawing a 270° arc
            bar = new VectorLine("TotalBar", new List<Vector2>(segmentCount + 1), null, lineWidth - 4,
                LineType.Continuous, Joins.Weld);
            bar.color = Color.black;
            bar.MakeArc(position, radius, radius, 0.0f, 270.0f);
            bar.Draw();

            currentPower = Random.value;
            SetTargetPower();
            // Set the initial bar colors by coloring the segments from the beginning to the current power level
            bar.SetColor(Color.red, 0, (int) Mathf.Lerp(0, segmentCount, currentPower));
        }

        private void SetTargetPower()
        {
            targetPower = Random.value;
        }

        private void Update()
        {
            var oldPower = currentPower;
            // Move current power up or down, and choose a new target power when the current power reaches the target
            if (targetPower < currentPower)
            {
                currentPower -= speed * Time.deltaTime;
                if (currentPower < targetPower) SetTargetPower();
                // When the bar decreases, use SetColor to "erase" the color from the current power to the old power
                bar.SetColor(Color.black, (int) Mathf.Lerp(0, segmentCount, currentPower),
                    (int) Mathf.Lerp(0, segmentCount, oldPower));
            }
            else
            {
                currentPower += speed * Time.deltaTime;
                if (currentPower > targetPower) SetTargetPower();
                // When the bar increases, use SetColor to color the line segments from the old power to the current power
                bar.SetColor(Color.red, (int) Mathf.Lerp(0, segmentCount, oldPower),
                    (int) Mathf.Lerp(0, segmentCount, currentPower));
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 15, 80, 30),
                "Power: " + (currentPower * 100).ToString("f0") + "%");
        }
    }
}
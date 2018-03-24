using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts.Grid
{
    public class DrawGrid : MonoBehaviour
    {
        private VectorLine gridLine;

        public int gridPixels = 50;

        private void Start()
        {
            gridLine = new VectorLine("Grid", new List<Vector2>(), 1.0f);
            // Align 1-pixel lines on the pixel grid, so they don't potentially get messed up by anti-aliasing
            gridLine.alignOddWidthToPixels = true;
            MakeGrid();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 30, 20), gridPixels.ToString());
            gridPixels = (int) GUI.HorizontalSlider(new Rect(40, 15, 590, 20), gridPixels, 5, 200);
            if (GUI.changed) MakeGrid();
        }

        private void MakeGrid()
        {
            var numberOfGridPoints = (Screen.width / gridPixels + 1 + Screen.height / gridPixels + 1) * 2;
            gridLine.Resize(numberOfGridPoints);

            var index = 0;
            for (var x = 0; x < Screen.width; x += gridPixels)
            {
                gridLine.points2[index++] = new Vector2(x, 0);
                gridLine.points2[index++] = new Vector2(x, Screen.height - 1);
            }

            for (var y = 0; y < Screen.height; y += gridPixels)
            {
                gridLine.points2[index++] = new Vector2(0, y);
                gridLine.points2[index++] = new Vector2(Screen.width - 1, y);
            }

            gridLine.Draw();
        }
    }
}
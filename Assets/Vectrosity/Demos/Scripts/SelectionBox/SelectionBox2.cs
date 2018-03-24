using System.Collections.Generic;
using UnityEngine;

namespace Vectrosity.Demos.Scripts.SelectionBox
{
    public class SelectionBox2 : MonoBehaviour
    {
        public Texture lineTexture;
        private Vector2 originalPos;
        private VectorLine selectionLine;
        public float textureScale = 4.0f;

        private void Start()
        {
            selectionLine = new VectorLine("Selection", new List<Vector2>(5), lineTexture, 4.0f, LineType.Continuous);
            selectionLine.textureScale = textureScale;
            // Prevent line from getting blurred by anti-aliasing (the line width is 4 but the texture has transparency that makes it effectively 1)
            selectionLine.alignOddWidthToPixels = true;
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 300, 25), "Click & drag to make a selection box");
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) originalPos = Input.mousePosition;
            if (Input.GetMouseButton(0))
            {
                selectionLine.MakeRect(originalPos, Input.mousePosition);
                selectionLine.Draw();
            }

            selectionLine.textureOffset = -Time.time * 2.0f % 1;
        }
    }
}
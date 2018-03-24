using UnityEngine;

namespace Vectrosity.Demos.Scripts._Simple2DLine
{
    public class ReallyBasicLine : MonoBehaviour
    {
        private void Start()
        {
            // Draw a line from the lower-left corner to the upper-right corner
            VectorLine.SetLine(Color.white, new Vector2(0, 0), new Vector2(Screen.width - 1, Screen.height - 1));
        }
    }
}
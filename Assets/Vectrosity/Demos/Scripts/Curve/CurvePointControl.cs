using UnityEngine;

namespace Vectrosity.Demos.Scripts.Curve
{
    public class CurvePointControl : MonoBehaviour
    {
        public GameObject controlObject;
        public GameObject controlObject2;

        public int objectNumber;

        private void OnMouseDrag()
        {
            transform.position = DrawCurve.cam.ScreenToViewportPoint(Input.mousePosition);
            DrawCurve.use.UpdateLine(objectNumber, Input.mousePosition, gameObject);
        }
    }
}
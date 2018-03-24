using UnityEngine;

namespace Vectrosity.Demos.Scripts.Scribblecube
{
    public class CameraZoom : MonoBehaviour
    {
        public float keyZoomSpeed = 20.0f;

        public float zoomSpeed = 10.0f;

        private void Update()
        {
            transform.Translate(Vector3.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
            transform.Translate(Vector3.forward * keyZoomSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
        }
    }
}
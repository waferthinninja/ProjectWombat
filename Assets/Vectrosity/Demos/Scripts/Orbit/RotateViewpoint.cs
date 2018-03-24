using UnityEngine;

namespace Vectrosity.Demos.Scripts.Orbit
{
    public class RotateViewpoint : MonoBehaviour
    {
        public float rotateSpeed = 5.0f;

        private void Update()
        {
            transform.RotateAround(Vector3.zero, Vector3.right, rotateSpeed * Time.deltaTime);
        }
    }
}
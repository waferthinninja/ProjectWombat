using UnityEngine;

namespace Vectrosity.Demos.Scripts.Highlight
{
    public class RotateAroundY : MonoBehaviour
    {
        public float rotateSpeed = 10.0f;

        private void Update()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
        }
    }
}
using UnityEngine;

namespace Controllers.Cameras
{
    public abstract class CameraController : MonoBehaviour, ICameraController
    {
        public float CameraSpeed = 40f;
        protected float Rotation = 0f;
        public Transform StartPosition;
        public float ZoomSpeed = 40f;

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public abstract void MoveCameraUp(float deltaTime);

        public abstract void MoveCameraDown(float deltaTime);

        public abstract void MoveCameraLeft(float deltaTime);

        public abstract void MoveCameraRight(float deltaTime);

        public abstract void MoveCameraForward(float deltaTime);

        public abstract void MoveCameraBackward(float deltaTime);

        public abstract void RotateCameraLeft(float deltaTime);

        public abstract void RotateCameraRight(float deltaTime);

        public abstract void ZoomCameraIn(float deltaTime);

        public abstract void ZoomCameraOut(float deltaTime);

        public abstract void ResetCamera();

        public abstract void Activate();
    }
}
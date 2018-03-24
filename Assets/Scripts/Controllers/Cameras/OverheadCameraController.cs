using Controllers.ShipComponents;
using Managers;
using UnityEngine;

namespace Controllers.Cameras
{
    public class OverheadCameraController : CameraController
    {
        public float MaxHeight = 100f;

        public float MinHeight = 1f;

        private void Start()
        {
            InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
        }

        private void OnSelectedShipChange(ShipController ship)
        {
            if (gameObject.activeSelf)
                if (InputManager.Instance.SelectedShip != null)
                    transform.position = new Vector3(
                        InputManager.Instance.SelectedShip.transform.position.x,
                        transform.position.y,
                        InputManager.Instance.SelectedShip.transform.position.z);
        }

        public override void Activate()
        {
            gameObject.SetActive(true);
            transform.parent = StartPosition;
            transform.localPosition = new Vector3();
            transform.rotation = new Quaternion();
            Rotation = 0;
        }

        public override void MoveCameraBackward(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.position = new Vector3(
                transform.position.x + moveSpeed * Mathf.Sin(Rotation * Mathf.Deg2Rad),
                transform.position.y,
                transform.position.z - moveSpeed * Mathf.Cos(Rotation * Mathf.Deg2Rad));
        }

        public override void MoveCameraUp(float deltaTime)
        {
            transform.Translate(Vector3.up * ZoomSpeed * deltaTime, Space.World);
            //Debug.Log("up" + transform.position.y);
            if (transform.position.y > MaxHeight)
            {
                Debug.Log("trim to max");
                transform.position = new Vector3(transform.position.x, MaxHeight, transform.position.z);
            }
        }

        public override void MoveCameraDown(float deltaTime)
        {
            transform.Translate(-Vector3.up * ZoomSpeed * deltaTime, Space.World);
            //Debug.Log("dn" + transform.position.y);
            if (transform.position.y < MinHeight)
            {
                Debug.Log("trim to min");
                transform.position = new Vector3(transform.position.x, MinHeight, transform.position.z);
            }
        }

        public override void MoveCameraForward(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.position = new Vector3(
                transform.position.x - moveSpeed * Mathf.Sin(Rotation * Mathf.Deg2Rad),
                transform.position.y,
                transform.position.z + moveSpeed * Mathf.Cos(Rotation * Mathf.Deg2Rad));
        }

        public override void MoveCameraLeft(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.position = new Vector3(
                transform.position.x - moveSpeed * Mathf.Cos(Rotation * Mathf.Deg2Rad),
                transform.position.y,
                transform.position.z - moveSpeed * Mathf.Sin(Rotation * Mathf.Deg2Rad));
        }

        public override void MoveCameraRight(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.position = new Vector3(
                transform.position.x + moveSpeed * Mathf.Cos(Rotation * Mathf.Deg2Rad),
                transform.position.y,
                transform.position.z + moveSpeed * Mathf.Sin(Rotation * Mathf.Deg2Rad));
        }

        public override void RotateCameraLeft(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.Rotate(Vector3.up, -moveSpeed, Space.World);
            Rotation += moveSpeed;
        }

        public override void RotateCameraRight(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.Rotate(Vector3.up, moveSpeed, Space.World);
            Rotation -= moveSpeed;
        }

        public override void ResetCamera()
        {
            Activate();
        }

        public override void ZoomCameraIn(float deltaTime)
        {
            MoveCameraDown(deltaTime);
        }

        public override void ZoomCameraOut(float deltaTime)
        {
            MoveCameraUp(deltaTime);
        }
    }
}
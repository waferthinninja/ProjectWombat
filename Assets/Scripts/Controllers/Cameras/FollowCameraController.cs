using System;
using Controllers.ShipComponents;
using Managers;
using UnityEngine;

namespace Controllers.Cameras
{
    public class FollowCameraController : CameraController
    {
        public float MaxDistance = 100f;
        public float MinDistance = 5f;

        private void Start()
        {
            InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
        }

        private void OnSelectedShipChange(ShipController ship)
        {
            if (gameObject.activeSelf)
                Activate(); // this will move the camera to the new ship
        }

        public override void Activate()
        {
            gameObject.SetActive(true);
            if (InputManager.Instance.SelectedShip == null) InputManager.Instance.SelectNextShip();

            transform.parent = InputManager.Instance.SelectedShip.transform.Find("CameraPoint");
            transform.localPosition = new Vector3();
            transform.rotation = new Quaternion();
        }

        public override void MoveCameraBackward(float deltaTime)
        {
            // zoom instead
            ZoomCameraOut(deltaTime);
        }

        public override void MoveCameraDown(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public override void MoveCameraForward(float deltaTime)
        {
            // zoom instead
            ZoomCameraIn(deltaTime);
        }

        public override void MoveCameraLeft(float deltaTime)
        {
            RotateCameraLeft(deltaTime);
        }

        public override void MoveCameraRight(float deltaTime)
        {
            RotateCameraRight(deltaTime);
        }

        public override void MoveCameraUp(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public override void RotateCameraLeft(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.RotateAround(InputManager.Instance.SelectedShip.transform.position, Vector3.up, moveSpeed);
        }

        public override void RotateCameraRight(float deltaTime)
        {
            var moveSpeed = CameraSpeed * deltaTime;
            transform.RotateAround(InputManager.Instance.SelectedShip.transform.position, Vector3.up, -moveSpeed);
        }

        public override void ResetCamera()
        {
            Activate();
        }

        public override void ZoomCameraIn(float deltaTime)
        {
            // get vector from ship to camera
            var dir = transform.position - InputManager.Instance.SelectedShip.transform.position;
            dir.Normalize();
            transform.position = transform.position - dir * deltaTime * ZoomSpeed;

            // trim distance if we have gone too far
            if (Vector3.Distance(transform.position, InputManager.Instance.SelectedShip.transform.position) <
                MinDistance)
                transform.position = InputManager.Instance.SelectedShip.transform.position + dir * MinDistance;
        }

        public override void ZoomCameraOut(float deltaTime)
        {
            // get vector from ship to camera
            var dir = transform.position - InputManager.Instance.SelectedShip.transform.position;
            dir.Normalize();
            transform.position = transform.position + dir * deltaTime * ZoomSpeed;

            // trim distance if we have gone too far
            if (Vector3.Distance(transform.position, InputManager.Instance.SelectedShip.transform.position) >
                MaxDistance)
                transform.position = InputManager.Instance.SelectedShip.transform.position + dir * MaxDistance;
        }
    }
}
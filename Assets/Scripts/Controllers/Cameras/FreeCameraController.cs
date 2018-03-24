using System;
using UnityEngine;

namespace Controllers.Cameras
{
    public class FreeCameraController : CameraController
    {
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

        public override void MoveCameraDown(float deltaTime)
        {
            throw new NotImplementedException();
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

        public override void MoveCameraUp(float deltaTime)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override void ZoomCameraOut(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
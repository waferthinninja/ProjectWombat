using System;
using UnityEngine;

public abstract class CameraController : MonoBehaviour, ICameraController
{
    public float CameraSpeed = 40f;
    public float ZoomSpeed = 40f;
    protected float _rotation = 0f;
    public Transform StartPosition;

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
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


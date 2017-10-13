using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraController : CameraController, ICameraController
{
    public float MinDistance = 5f;
    public float MaxDistance = 100f;

    void Start()
    {
        InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
    }

    public void OnSelectedShipChange(ShipController ship)
    {
        if (gameObject.activeSelf)
            Activate(); // this will move the camera to the new ship
    }

    public override void Activate()
    {
        this.gameObject.SetActive(true);
        if (InputManager.Instance.SelectedShip == null)
        {
            InputManager.Instance.SelectNextShip();
        }

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
        float moveSpeed = CameraSpeed * deltaTime;
        transform.RotateAround(InputManager.Instance.SelectedShip.transform.position, Vector3.up, moveSpeed);
    }

    public override void RotateCameraRight(float deltaTime)
    {
        float moveSpeed = CameraSpeed * deltaTime;
        transform.RotateAround(InputManager.Instance.SelectedShip.transform.position, Vector3.up, -moveSpeed);
    }

    public override void ResetCamera()
    {
        Activate();
    }

    public override void ZoomCameraIn(float deltaTime)
    {
        // get vector from ship to camera
        Vector3 dir = transform.position - InputManager.Instance.SelectedShip.transform.position;
        dir.Normalize();
        transform.position = transform.position - dir * deltaTime * ZoomSpeed;
        
        // trim distance if we have gone too far
        if (Vector3.Distance(transform.position, InputManager.Instance.SelectedShip.transform.position) < MinDistance)
        {
            transform.position = InputManager.Instance.SelectedShip.transform.position + dir * MinDistance;
        }
        
    }

    public override void ZoomCameraOut(float deltaTime)
    {
        // get vector from ship to camera
        Vector3 dir = transform.position - InputManager.Instance.SelectedShip.transform.position;
        dir.Normalize();
        transform.position = transform.position + dir * deltaTime * ZoomSpeed;

        // trim distance if we have gone too far
        if (Vector3.Distance(transform.position, InputManager.Instance.SelectedShip.transform.position) > MaxDistance)
        {
            transform.position = InputManager.Instance.SelectedShip.transform.position + dir * MaxDistance;
        }
    }
    

}

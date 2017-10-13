using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverheadCameraController : CameraController {

    public float MinHeight = 1f;
    public float MaxHeight = 100f;

    void Start()
    {
        InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);
    }

    public void OnSelectedShipChange(ShipController ship)
    {
        if (gameObject.activeSelf)
        {
            // move to above the ship
            transform.position = new Vector3(
                InputManager.Instance.SelectedShip.transform.position.x,
                transform.position.y,
                InputManager.Instance.SelectedShip.transform.position.z);
        }
    }

    public override void Activate()
    {
        this.gameObject.SetActive(true);
        transform.parent = StartPosition;
        transform.localPosition = new Vector3();
        transform.rotation = new Quaternion();
        _rotation = 0;
    }

    public override void MoveCameraBackward(float deltaTime)
    {
        float moveSpeed = CameraSpeed * deltaTime;
        transform.position = new Vector3(
            transform.position.x + moveSpeed * Mathf.Sin(_rotation * Mathf.Deg2Rad),
            transform.position.y,
            transform.position.z - moveSpeed * Mathf.Cos(_rotation * Mathf.Deg2Rad));
    }

    public override void MoveCameraUp(float deltaTime)
    {        
        transform.Translate(Vector3.up * ZoomSpeed * deltaTime, Space.World);
        Debug.Log("up" + transform.position.y);
        if (transform.position.y > MaxHeight)
        {
            Debug.Log("trim to max");
            transform.position = new Vector3(transform.position.x, MaxHeight, transform.position.z);
        }
    }

    public override void MoveCameraDown(float deltaTime)
    {        
        transform.Translate(-Vector3.up * ZoomSpeed * deltaTime, Space.World);
        Debug.Log("dn" + transform.position.y);
        if (transform.position.y < MinHeight)
        {
            Debug.Log("trim to min");
            transform.position = new Vector3(transform.position.x, MinHeight, transform.position.z);
        }
    }

    public override void MoveCameraForward(float deltaTime)
    {
        float moveSpeed = CameraSpeed * deltaTime;
        transform.position = new Vector3(
            transform.position.x - moveSpeed * Mathf.Sin(_rotation * Mathf.Deg2Rad),
            transform.position.y,
            transform.position.z + moveSpeed * Mathf.Cos(_rotation * Mathf.Deg2Rad));
    }

    public override void MoveCameraLeft(float deltaTime)
    {
        float moveSpeed = CameraSpeed * deltaTime;
        transform.position = new Vector3(
            transform.position.x - moveSpeed * Mathf.Cos(_rotation * Mathf.Deg2Rad),
            transform.position.y,
            transform.position.z - moveSpeed * Mathf.Sin(_rotation * Mathf.Deg2Rad));
    }

    public override void MoveCameraRight(float deltaTime)
    {
        float moveSpeed = CameraSpeed * deltaTime;
        transform.position = new Vector3(
            transform.position.x + moveSpeed * Mathf.Cos(_rotation * Mathf.Deg2Rad),
            transform.position.y,
            transform.position.z + moveSpeed * Mathf.Sin(_rotation * Mathf.Deg2Rad));

    }

    public override void RotateCameraLeft(float deltaTime)
    {
        float moveSpeed = CameraSpeed * deltaTime;
        transform.Rotate(Vector3.up, -moveSpeed, Space.World);
        _rotation += moveSpeed;
    }
    public override void RotateCameraRight(float deltaTime)
    {
        float moveSpeed = CameraSpeed * deltaTime;
        transform.Rotate(Vector3.up, moveSpeed, Space.World);
        _rotation -= moveSpeed;
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

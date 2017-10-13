using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraController : CameraController, ICameraController {
    
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

    public override void MoveCameraDown(float deltaTime)
    {
        throw new NotImplementedException();
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

    public override void MoveCameraUp(float deltaTime)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public override void ZoomCameraOut(float deltaTime)
    {
        throw new NotImplementedException();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
    
    //MAKE INSTANCE
    private static CameraManager _instance;

    public static CameraManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<CameraManager>();
            return _instance;
        }
    }
    //END MAKE INSTANCE
    
    public Transform FreeStartPosition;
    public Transform OverheadStartPosition;

    public CameraMode CameraMode;

    private Dictionary<CameraMode, float> _cameraSpeed = new Dictionary<CameraMode, float>
    {
        { CameraMode.Free, 40f },
        { CameraMode.Follow, 40f },
        { CameraMode.Overhead, 40f }
    };

    // Use this for initialization
    void Start ()
    {
        SwitchToOverheadMode();
        InputManager.Instance.RegisterOnSelectedShipChange(OnSelectedShipChange);

	}
	
	// Update is called once per frame
	void Update ()
    {
    }

    public void OnSelectedShipChange(ShipController ship)
    {
        if (CameraMode == CameraMode.Follow)
        {
            SwitchToFollowMode(); // slight hack, this will move the camera to the new ship
        }
    }



    public void MoveCameraUp(float deltaTime)
    {
        float moveSpeed = _cameraSpeed[CameraMode] * deltaTime;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + moveSpeed);
    }

    public void MoveCameraDown(float deltaTime)
    {
        float moveSpeed = _cameraSpeed[CameraMode] * deltaTime;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - moveSpeed);
    }

    public void MoveCameraLeft(float deltaTime)
    {
        float moveSpeed = _cameraSpeed[CameraMode] * deltaTime;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - moveSpeed, Camera.main.transform.position.y, Camera.main.transform.position.z);


    }

    public void MoveCameraRight(float deltaTime)
    { 
        float moveSpeed = _cameraSpeed[CameraMode] * deltaTime;        
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + moveSpeed, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }

    public void RotateCameraLeft(float deltaTime)
    {
        float moveSpeed = _cameraSpeed[CameraMode] * deltaTime;
        if (CameraMode == CameraMode.Follow)
        {
            Camera.main.transform.RotateAround(InputManager.Instance.SelectedShip.transform.position, Vector3.up, moveSpeed);
        }
        else
        {
            Camera.main.transform.Rotate(Vector3.up, -moveSpeed);
        }

    }

    public void RotateCameraRight(float deltaTime)
    {
        float moveSpeed = _cameraSpeed[CameraMode] * deltaTime;
        if (CameraMode == CameraMode.Follow)
        {
            Camera.main.transform.RotateAround(InputManager.Instance.SelectedShip.transform.position, Vector3.up, -moveSpeed);
        }
        else
        {
            Camera.main.transform.Rotate(Vector3.up, -moveSpeed);
        }

    }

    public void CycleCameraMode()
    {
        if (CameraMode == CameraMode.Free) SwitchToFollowMode();
        else if (CameraMode == CameraMode.Follow) SwitchToOverheadMode();
        else if (CameraMode == CameraMode.Overhead) SwitchToFreeMode();
    }

    public void SwitchToFreeMode()
    {
        CameraMode = CameraMode.Free;
        Camera.main.transform.parent = FreeStartPosition;
        Camera.main.transform.localPosition = new Vector3();
        Camera.main.transform.rotation = new Quaternion();  

    }

    public void SwitchToFollowMode()
    {
        CameraMode = CameraMode.Follow;

        if (InputManager.Instance.SelectedShip == null)
        {
            InputManager.Instance.SelectNextShip();
        }

        Camera.main.transform.parent = InputManager.Instance.SelectedShip.transform.Find("CameraPoint");
        Camera.main.transform.localPosition = new Vector3();
        Camera.main.transform.rotation = new Quaternion();
    }

    public void SwitchToOverheadMode()
    {
        CameraMode = CameraMode.Overhead;

        Camera.main.transform.parent = OverheadStartPosition;
        Camera.main.transform.localPosition = new Vector3();
        Camera.main.transform.rotation = new Quaternion();
    }

}

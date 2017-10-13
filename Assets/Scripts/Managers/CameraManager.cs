using System;
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

    public CameraController[] UserCameras;
    public CutSceneCameraController CutSceneCamera;
    private int _currentCameraIndex;
        
    public Action OnCameraChange;
    public void RegisterOnCameraChange(Action action) { OnCameraChange += action; }

    // Use this for initialization
    void Start ()
    {
        SetUserCamera(0);        
	}
	    
    public void SetUserCamera(int index)
    {
        _currentCameraIndex = index % UserCameras.Length;
        for (int i = 0; i < UserCameras.Length; i++)
        {
            if (i == _currentCameraIndex)
            {
                UserCameras[i].Activate();
            }
            else
            {
                UserCameras[i].Deactivate();
            }
        }
        if (OnCameraChange != null)
        {
            OnCameraChange();
        }
    }

    public void CycleCameraMode()
    {
        SetUserCamera(++_currentCameraIndex);        
    }    

    public CameraController GetCurrentCamera()
    {
        return UserCameras[_currentCameraIndex];
    }

    public void ActivateCutSceneCamera()
    {
        foreach (var camera in UserCameras)
        {
            camera.Deactivate();
        }
        CutSceneCamera.Activate();
    }

    public void DeactivateCutSceneCamera()
    {
        UserCameras[_currentCameraIndex].Activate();
        CutSceneCamera.Deactivate();
    }
}

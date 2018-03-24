using System;
using Controllers.Cameras;
using UnityEngine;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        //MAKE INSTANCE
        private static CameraManager _instance;
        private int _currentCameraIndex;

        private Action _onCameraChange;

        public CutSceneCameraController CutSceneCamera;
        //END MAKE INSTANCE

        public CameraController[] UserCameras;

        public static CameraManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<CameraManager>();
                return _instance;
            }
        }

        public void RegisterOnCameraChange(Action action)
        {
            _onCameraChange += action;
        }

        public void UnregisterOnCameraChange(Action action)
        {
            _onCameraChange -= action;
        }

        // Use this for initialization
        private void Start()
        {
            SetUserCamera(0);
        }

        private void SetUserCamera(int index)
        {
            _currentCameraIndex = index % UserCameras.Length;
            for (var i = 0; i < UserCameras.Length; i++)
                if (i == _currentCameraIndex)
                    UserCameras[i].Activate();
                else
                    UserCameras[i].Deactivate();
            if (_onCameraChange != null) _onCameraChange();
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
            foreach (var cam in UserCameras) cam.Deactivate();
            CutSceneCamera.Activate();
        }

        public void DeactivateCutSceneCamera()
        {
            UserCameras[_currentCameraIndex].Activate();
            CutSceneCamera.Deactivate();
        }
    }
}
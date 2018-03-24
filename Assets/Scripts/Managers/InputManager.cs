using System;
using Controllers.ShipComponents;
using Model.Enums;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        //MAKE INSTANCE
        private static InputManager _instance;

        private Action<ShipController> _onSelectedShipChange;

        private Action<ShipController> _onTargetSelected;

        private int _selectedShipIndex;

        private bool _targetSelectMode;

        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<InputManager>();
                return _instance;
            }
        }
        //END MAKE INSTANCE

        public ShipController SelectedShip { get; private set; }

        public void RegisterOnSelectedShipChange(Action<ShipController> action)
        {
            _onSelectedShipChange += action;
        }

        public void RegisterOnTargetSelected(Action<ShipController> action)
        {
            _targetSelectMode = true;

            _onTargetSelected += action;
        }

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            // disable most controls during intro
            if (GameManager.Instance.GameState == GameState.CutScene)
            {
            }

            // keyboard
            if (Input.GetKeyDown(KeyCode.C)) CameraManager.Instance.CycleCameraMode();
            if (Input.GetKey(KeyCode.UpArrow))
                CameraManager.Instance.GetCurrentCamera().MoveCameraForward(Time.deltaTime);
            if (Input.GetKey(KeyCode.DownArrow))
                CameraManager.Instance.GetCurrentCamera().MoveCameraBackward(Time.deltaTime);
            if (Input.GetKey(KeyCode.LeftArrow))
                CameraManager.Instance.GetCurrentCamera().MoveCameraLeft(Time.deltaTime);
            if (Input.GetKey(KeyCode.RightArrow))
                CameraManager.Instance.GetCurrentCamera().MoveCameraRight(Time.deltaTime);
            if (Input.GetKey(KeyCode.W)) CameraManager.Instance.GetCurrentCamera().MoveCameraForward(Time.deltaTime);
            if (Input.GetKey(KeyCode.S)) CameraManager.Instance.GetCurrentCamera().MoveCameraBackward(Time.deltaTime);
            if (Input.GetKey(KeyCode.A)) CameraManager.Instance.GetCurrentCamera().MoveCameraLeft(Time.deltaTime);
            if (Input.GetKey(KeyCode.D)) CameraManager.Instance.GetCurrentCamera().MoveCameraRight(Time.deltaTime);
            if (Input.GetKey(KeyCode.Q)) CameraManager.Instance.GetCurrentCamera().RotateCameraLeft(Time.deltaTime);
            if (Input.GetKey(KeyCode.E)) CameraManager.Instance.GetCurrentCamera().RotateCameraRight(Time.deltaTime);
            if (Input.GetKey(KeyCode.Backspace)) CameraManager.Instance.GetCurrentCamera().ResetCamera();
            if (Input.GetKeyDown(KeyCode.Tab)) SelectNextShip();
            if (Input.GetKeyDown(KeyCode.Escape))
                if (_targetSelectMode)
                {
                    TargetShip(null);
                }
                else
                {
                    if (SelectedShip != null) ClearSelectedShip();
                }

            // mouse
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                CameraManager.Instance.GetCurrentCamera().ZoomCameraOut(Time.deltaTime);
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                CameraManager.Instance.GetCurrentCamera().ZoomCameraIn(Time.deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                var go = GetClickedGameObject();
                ShipController ship = null;
                if (go != null)
                    while (ship == null && go.transform.parent != null)
                    {
                        go = go.transform.parent.gameObject;
                        ship = go.GetComponent<ShipController>();
                    }

                if (ship != null)
                    if (_targetSelectMode)
                        TargetShip(ship);
                    else
                        SelectShip(ship);
            }
        }


        public void SelectNextShip()
        {
            if (SelectedShip == null)
            {
                _selectedShipIndex = 0;
            }
            else
            {
                _selectedShipIndex++;
                if (_selectedShipIndex >= GameManager.Instance.Ships.Count) _selectedShipIndex = 0;
            }

            if (GameManager.Instance.Ships[_selectedShipIndex].IsDying) SelectNextShip();

            SelectShip(_selectedShipIndex);
        }

        public void ClearSelectedShip()
        {
            SelectedShip = null;
            if (_onSelectedShipChange != null) _onSelectedShipChange(null);
        }

        public void TargetShip(ShipController ship)
        {
            if (_onTargetSelected != null)
            {
                _onTargetSelected(ship);

                // clear this action
                _onTargetSelected = null;
                _targetSelectMode = false;
            }
        }

        public void SelectShip(int index)
        {
            if (index < 0)
            {
                Debug.LogError("Tried to select ship index < 0");
                index = 0;
            }

            if (index >= GameManager.Instance.Ships.Count)
            {
                Debug.LogError("Tried to select ship index out of bounds");
                index = 0;
            }

            _selectedShipIndex = index;
            SelectShip(GameManager.Instance.Ships[index]);
        }

        public void SelectShip(ShipController ship)
        {
            for (var i = 0; i < GameManager.Instance.Ships.Count; i++)
                if (ship == GameManager.Instance.Ships[i])
                    _selectedShipIndex = i;

            SelectedShip = ship;
            //Debug.Log(String.Format("{0}(index {1}) selected", SelectedShip.ShipName, _selectedShipIndex));

            if (_onSelectedShipChange != null) _onSelectedShipChange(ship);
        }

        private GameObject GetClickedGameObject()
        {
            // Builds a ray from camera point of view to the mouse position 
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            // Casts the ray and get the first game object hit 
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) return hit.transform.gameObject;

            return null;
        }
    }
}
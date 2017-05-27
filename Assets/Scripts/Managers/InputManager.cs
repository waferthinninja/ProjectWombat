using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    //MAKE INSTANCE
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<InputManager>();
            return _instance;
        }
    }
    //END MAKE INSTANCE
    
    public ShipController SelectedShip { get; private set; }

    private int _selectedShipIndex;

    private Action<ShipController> OnSelectedShipChange;
    public void RegisterOnSelectedShipChange(Action<ShipController> action) { OnSelectedShipChange += action; }

    private Action<ShipController> OnTargetSelected;
    public void RegisterOnTargetSelected(Action<ShipController> action)
    {
        _targetSelectMode = true;
        
        OnTargetSelected += action;
    }

    private bool _targetSelectMode; 

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.C))
        {
            CameraManager.Instance.CycleCameraMode();
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            CameraManager.Instance.MoveCameraUp(Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            CameraManager.Instance.MoveCameraDown(Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            CameraManager.Instance.MoveCameraLeft(Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            CameraManager.Instance.MoveCameraRight(Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            CameraManager.Instance.RotateCameraLeft(Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            CameraManager.Instance.RotateCameraRight(Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNextShip();
        }

        if (Input.GetMouseButtonDown(0))
        {
            var go = GetClickedGameObject();
            if (go != null && go.transform.parent != null && go.transform.parent.gameObject.GetComponent<ShipController>() != null)
            {
                if (_targetSelectMode)
                {
                    TargetShip(go.transform.parent.gameObject.GetComponent<ShipController>());
                }
                else
                {
                    SelectShip(go.transform.parent.gameObject.GetComponent<ShipController>());
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_targetSelectMode)
            {
                TargetShip(null);
            }
            else
            {
                if (SelectedShip != null)
                {
                    ClearSelectedShip();
                }
                else
                {
                    //TODO - add confirmation
                    //Application.Quit();
                }
            }
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
            if (_selectedShipIndex >= GameManager.Instance.Ships.Count)
            {
                _selectedShipIndex = 0;
            }
        }
        
        if (GameManager.Instance.Ships[_selectedShipIndex].IsDying)
        {
            SelectNextShip();
        }

        SelectShip(_selectedShipIndex);
    }

    public void ClearSelectedShip()
    {
        SelectedShip = null;
        if (OnSelectedShipChange != null)
        {
            OnSelectedShipChange(null);
        }
    }

    public void TargetShip(ShipController ship)
    {
        if (OnTargetSelected != null)
        {
            OnTargetSelected(ship);

            // clear this action
            OnTargetSelected = null;
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
        for (int i = 0; i < GameManager.Instance.Ships.Count; i++)
        {
            if (ship == GameManager.Instance.Ships[i])
            {
                _selectedShipIndex = i; 
            }
        }

        SelectedShip = ship;
        Debug.Log(String.Format("{0}(index {1}) selected", SelectedShip.ShipName, _selectedShipIndex));

        if (OnSelectedShipChange != null)
        {
            OnSelectedShipChange(ship);
        }
        
    }

    GameObject GetClickedGameObject()
    {
        // Builds a ray from camera point of view to the mouse position 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        // Casts the ray and get the first game object hit 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.transform.gameObject;
        }
        else
        {
            return null;
        }
    }
}

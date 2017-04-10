using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {
    

    public int CameraSpeed = 20;

    public Slider TurnSlider;
    public Slider AccelerationSlider;

    private ShipController _selectedShip;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float moveSpeed = CameraSpeed * Time.deltaTime;

		if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y , Camera.main.transform.position.z + moveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - moveSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - moveSpeed, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + moveSpeed, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var go = GetClickedGameObject();
            if (go != null && go.transform.parent.gameObject.GetComponent<ShipController>() != null)
            {
                _selectedShip = go.transform.parent.gameObject.GetComponent<ShipController>();
                Debug.Log(_selectedShip.ShipName + " selected");

                TurnSlider.value = _selectedShip.Turn;
                AccelerationSlider.value = _selectedShip.Acceleration;
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_selectedShip != null)
            {
                _selectedShip = null;
            }
            else
            {
                Application.Quit();
            }
        }
    }

    public void ApplyTurnSliderChange()
    {
        if (_selectedShip == null) return;

        _selectedShip.SetTurn(TurnSlider.value);
    }

    public void ApplyAccelerationSliderChange()
    {
        if (_selectedShip == null) return;

        _selectedShip.SetAcceleration(AccelerationSlider.value);
    }

    GameObject GetClickedGameObject()
    {
        // Builds a ray from camera point of view to the mouse position 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        
        // Casts the ray and get the first game object hit 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            return hit.transform.gameObject;
        else
            return null;
    }
}

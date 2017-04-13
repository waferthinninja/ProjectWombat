using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    //MAKE INSTANCE
    private static InputController _instance;

    public static InputController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<InputController>();
            return _instance;
        }
    }
    //END MAKE INSTANCE


    public ShipControlsController ShipControlsController;
    public ShipDetailsController ShipDetailsController;

    private float _cameraSpeed = 20;

    public ShipController SelectedShip { get; private set; }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float moveSpeed = _cameraSpeed * Time.deltaTime;

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
            if (go != null && go.transform.parent != null && go.transform.parent.gameObject.GetComponent<ShipController>() != null)
            {
                SelectShip(go.transform.parent.gameObject.GetComponent<ShipController>());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SelectedShip != null)
            {
                ClearSelectedShip();
            }
            else
            {
                //TODO - add confirmatio 
                //Application.Quit();
            }
        }
    }

    public void ClearSelectedShip()
    {
        SelectedShip = null;
        ShipControlsController.ClearSelectedShip();
        ShipDetailsController.ClearSelectedShip();
    }

    public void SelectShip(ShipController ship)
    {
        SelectedShip = ship;
        Debug.Log(SelectedShip.ShipName + " selected");

        // TODO - use callback action for these
        ShipControlsController.SelectShip(ship);
        ShipDetailsController.SelectShip(ship);
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

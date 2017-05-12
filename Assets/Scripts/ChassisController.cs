using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChassisController : MonoBehaviour {

    ShipController ship; 

	// Use this for initialization
	void Start () {
        // store ref to the ship this is attached to
        ship = transform.parent.GetComponent<ShipController>();
	}
	

}

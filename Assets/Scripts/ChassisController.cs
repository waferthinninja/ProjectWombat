using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChassisController : MonoBehaviour {

    public ShipController Ship; 

	// Use this for initialization
	void Start () {
        // store ref to the ship this is attached to
        Ship = transform.parent.GetComponent<ShipController>();
	}
	

}

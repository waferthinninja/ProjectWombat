using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour {

    public GameObject ProjectilePrefab;

    public int Capacity { get; private set; }

	// Use this for initialization
	void Start () {
		// register for setup complete, so we can set a sensible pool size based on the number of ships
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

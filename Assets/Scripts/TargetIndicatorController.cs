using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TargetIndicatorController : MonoBehaviour {

    private LineRenderer _lineRenderer;
    

	// Use this for initialization
	void Start () {
        _lineRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

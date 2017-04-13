using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {

    public ShipController Ship;
    
    public MeshRenderer ChassisRenderer;

    public Color Color;

    // Use this for initialization
    void Start () {

        ChassisRenderer.material.SetColor("_Color", Color);

        RecalculatePosition();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecalculatePosition()
    {
        var points = new Vector3[GameManager.NUM_MOVEMENT_STEPS + 1];

        // first point is current position
        points[0] = Ship.transform.position;

        Vector3 pos = Ship.transform.position;
        Quaternion rot = Ship.transform.rotation;
        // now simulate 5 seconds of movement placing a point at the end of each
        for (int t = 0; t < GameManager.NUM_MOVEMENT_STEPS; t++)
        {
            // apply proportion of the turn
            rot *= Quaternion.Euler(Vector3.up * Ship.TurnProportion * Ship.MaxTurn / GameManager.NUM_MOVEMENT_STEPS);
            // move forward 1 seconds worth of movement in the new direction
            pos += rot * Vector3.forward * GameManager.MOVEMENT_STEP_LENGTH * (Ship.CurrentSpeed + Ship.Acceleration * Ship.MaxAcceleration);
            
            points[t + 1] = pos;
            
        }

        // set the line renderer points
        GetComponent<LineRenderer>().numPositions = points.Length;
        GetComponent<LineRenderer>().SetPositions(points);

        // place the ghost at the end of the line
        transform.position = new Vector3(pos.x, pos.y - 0.01f, pos.z); 
        transform.rotation = rot;
        

    }

}

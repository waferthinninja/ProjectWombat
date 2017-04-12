using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {

    public ShipController Ship;

    // turn length settings - should prob move to a central place
    float GHOST_STEP_SIZE = 0.5f; // seconds per interval
    int GHOST_NUM_STEPS = 10; // intervals per turn 

    // Use this for initialization
    void Start () {
        RecalculatePosition();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RecalculatePosition()
    {
        var points = new Vector3[GHOST_NUM_STEPS + 1];

        // first point is current position
        points[0] = Ship.transform.position;

        Vector3 pos = Ship.transform.position;
        Quaternion rot = Ship.transform.rotation;
        // now simulate 5 seconds of movement placing a point at the end of each
        for (int t = 0; t < GHOST_NUM_STEPS; t++)
        {
            // apply proportion of the turn
            rot *= Quaternion.Euler(Vector3.up * Ship.Turn * Ship.MaxTurn / (float)GHOST_NUM_STEPS);
            // move forward 1 seconds worth of movement in the new direction
            pos += rot * Vector3.forward * GHOST_STEP_SIZE * (Ship.CurrentSpeed + Ship.Acceleration * Ship.MaxAcceleration);
            
            points[t + 1] = pos;
            
        }

        // set the line renderer points
        GetComponent<LineRenderer>().numPositions = points.Length;
        GetComponent<LineRenderer>().SetPositions(points);

        // place the ghost at the end of the line
        transform.position = pos; 
        transform.rotation = rot;
        

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    // ship details
    public int ShipId { get; private set; }
    public string ShipName;
    public float MaxAcceleration;
    public float MaxTurn;

    // current settings
    public float CurrentSpeed { get; private set; } // this is the speed as of last turn
    public float Acceleration { get; private set; } // this is the delta speed applied this turn rather than real acceleration
    public float TurnProportion { get; private set; }

    public Vector3[] ProjectedPositions;
    public Quaternion[] ProjectedRotations;

    public LineRenderer ProjectedPath;

    public Faction Faction;

    bool _ghostEnabled;
    
    private int _targetIndex;

	// Use this for initialization
	void Start ()
    {
        ShipId = GameManager.Instance.GetShipId();
        Debug.Log("Ship " + ShipName + " allocated id " + ShipId.ToString());

        ProjectedPositions = new Vector3[GameManager.NUM_MOVEMENT_STEPS + 1];
        ProjectedRotations = new Quaternion[GameManager.NUM_MOVEMENT_STEPS + 1];

        ProjectedPositions[0] = transform.position;
        ProjectedRotations[0] = transform.rotation;
        RecalculateProjections();

        // register callbacks
        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfPlayback(OnStartOfPlayback);
        TimeController.Instance.RegisterOnTimeChange(OnTimeChange);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (GameManager.Instance.GameState == GameState.Playback)
        {
            //float distanceToTravel = Mathf.Abs(Time.deltaTime * CurrentSpeed * 10f);
            //while (distanceToTravel > 0 && _targetPathIndex < _path.Length)
            //{
            //    // are we going to reach the next checkpoint
            //    if (Vector3.Distance(transform.position, _path[_targetPathIndex]) <= distanceToTravel)
            //    {
            //        transform.position = _path[_targetPathIndex];
            //        distanceToTravel -= Vector3.Distance(transform.position, _path[_targetPathIndex]);
            //        _targetPathIndex++;
            //        if (_targetPathIndex < _path.Length)
            //        {
            //            transform.Rotate(Vector3.up, TurnProportion * MaxTurn / GameManager.NUM_MOVEMENT_STEPS);
            //        }
                    
            //    }
            //    else
            //    {
            //        Vector3 direction = _path[_targetPathIndex] - transform.position;
            //        direction.Normalize();
            //        transform.position = transform.position + direction * distanceToTravel;
            //        distanceToTravel = 0;
            //    }
            //}

        }
	}

    // callback called at the start of the playback phase
    public void OnStartOfPlayback()
    {
        transform.position = ProjectedPositions[0];
    }

    // callback called at the start of the planning phase;
    public void OnStartOfPlanning()
    {        
        // move ship to the final position of previous turn
        ProjectedPositions[0] = ProjectedPositions[GameManager.NUM_MOVEMENT_STEPS];
        ProjectedRotations[0] = ProjectedRotations[GameManager.NUM_MOVEMENT_STEPS];

        // reset controls and calculate new projected path
        Acceleration = 0;
        TurnProportion = 0;
        RecalculateProjections();
    }

    public void OnTimeChange(float time)
    {
        int i = (int)time;
        transform.position = ProjectedPositions[i];
        transform.rotation = ProjectedRotations[i];
    }

    public void SetAcceleration(float acceleration)
    {
        Acceleration = acceleration;
        RecalculateProjections();
    }

    public void SetTurn(float turn)
    {
        TurnProportion = turn;
        RecalculateProjections();
    }


    public void RecalculateProjections()
    {   
        // start at the beginning of the turn   
        Vector3 pos = ProjectedPositions[0];
        Quaternion rot = ProjectedRotations[0];
        
        // now simulate the turns movement placing a point at the end of each
        for (int t = 1; t <= GameManager.NUM_MOVEMENT_STEPS; t++)
        {
            // apply proportion of the turn
            rot *= Quaternion.Euler(Vector3.up * TurnProportion * MaxTurn / GameManager.NUM_MOVEMENT_STEPS);

            // move forward 1 seconds worth of movement in the new direction
            pos += rot * Vector3.forward * GameManager.MOVEMENT_STEP_LENGTH * (CurrentSpeed + Acceleration * MaxAcceleration);

            ProjectedPositions[t] = pos;
            ProjectedRotations[t] = rot;

        }

        // set the line renderer points
        ProjectedPath.numPositions = ProjectedPositions.Length;
        ProjectedPath.SetPositions(ProjectedPositions);

    }

}

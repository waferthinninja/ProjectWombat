using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MobileObjectBase {

    // ship details
    public string ShipName;

    public Faction Faction;

    public float MaxHullPoints;
    private float _hullPointsAtStartOfTurn;
    private float _hullPoints;


    public Vector3[] ProjectedPositions;
    public Quaternion[] ProjectedRotations;

    public LineRenderer ProjectedPath;


    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        _hullPoints = MaxHullPoints;
        _hullPointsAtStartOfTurn = _hullPoints;
        
        ProjectedPositions = new Vector3[GameManager.NUM_MOVEMENT_STEPS + 1];
        ProjectedRotations = new Quaternion[GameManager.NUM_MOVEMENT_STEPS + 1];

        ProjectedPositions[0] = transform.position;
        ProjectedRotations[0] = transform.rotation;
        
        RecalculateProjections();
    }
	
	// Update is called once per frame
	new void Update ()
    {
        base.Update();
	}
    
    public float ApplyDamage(float damage)
    {
        _hullPoints -= damage;
        if (_hullPoints <= 0)
        {
            KillSelf();
            return -_hullPoints; // return any damage which overkilled
        }
        return 0;
    }

    protected override void KillSelf()
    {
        // tell child objects to kill themselves (so we unregister any callbacks)

        base.KillSelf();

    }

    public void RecalculateProjections()
    {
        // start at the beginning of the turn   
        Vector3 pos = ProjectedPositions[0];
        Quaternion rot = ProjectedRotations[0];

        int stepsPerStep = (int)(GameManager.TURN_LENGTH * 60f / GameManager.NUM_MOVEMENT_STEPS); // hack so we calculate more accurately but store less data
        float stepLength = 1f / 60f;

        // now simulate the turns movement placing a point at the end of each
        for (int t = 1; t <= GameManager.NUM_MOVEMENT_STEPS; t++)
        {
            for (int x = 0; x < stepsPerStep; x++)
            {
                // apply proportion of the turn
                rot *= Quaternion.Euler(Vector3.up * TurnProportion * MaxTurn * stepLength / GameManager.TURN_LENGTH);

                // move forward 1 step in the new direction
                pos += rot * Vector3.forward * stepLength * (SpeedProportion * MaxSpeed);
            }
            ProjectedPositions[t] = pos;
            ProjectedRotations[t] = rot;
        }

        // set the line renderer points
        if (ProjectedPath != null)
        {
            ProjectedPath.numPositions = ProjectedPositions.Length;
            ProjectedPath.SetPositions(ProjectedPositions);
        }
    }

    public override void SetSpeed(float speedProportion)
    {
        base.SetSpeed(speedProportion);
        RecalculateProjections();
    }

    public override void SetTurn(float turn)
    {
        base.SetTurn(turn);
        RecalculateProjections();
    }

}

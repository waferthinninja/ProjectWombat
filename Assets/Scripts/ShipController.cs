using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MobileObjectBase {

    // ship details
    public string ShipName;

    public Faction Faction;

    public float MaxHullPoints;
    private float _hullPointsAtStart;
    public float HullPoints { get; private set; }
    
    public Vector3[] ProjectedPositions;
    public Quaternion[] ProjectedRotations;

    public LineRenderer ProjectedPath;

    public List<ShieldController> Shields { get; private set; }
    public List<WeaponController> Weapons { get; private set; }

    private bool _dying;

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        HullPoints = MaxHullPoints;
        _hullPointsAtStart = HullPoints;
        
        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);

        InitialiseProjections();
        InitialiseShields();
        InitialiseWeapons();
    }

    private void InitialiseWeapons()
    {
        Weapons = new List<WeaponController>();
        foreach (Transform t in transform)
        {
            var weapon = t.GetComponent<WeaponController>();
            if (weapon != null)
            {
                Weapons.Add(weapon);
            }
        }
    }

    private void InitialiseShields()
    {
        Shields = new List<ShieldController>();
        foreach (Transform t in transform)
        {
            var shield = t.GetComponent<ShieldController>();
            if (shield != null)
            {
                Shields.Add(shield);
            }
        }
    }

    // Update is called once per frame
    new void Update ()
    {
        base.Update();
	}
    
    private void InitialiseProjections()
    {
        ProjectedPositions = new Vector3[GameManager.NUM_MOVEMENT_STEPS + 1];
        ProjectedRotations = new Quaternion[GameManager.NUM_MOVEMENT_STEPS + 1];

        ProjectedPositions[0] = transform.position;
        ProjectedRotations[0] = transform.rotation;

        RecalculateProjections();
    }

    public float ApplyDamage(float damage)
    {
        HullPoints -= damage;
        if (HullPoints <= 0)
        {
            SetDeath();
            return -HullPoints; // return any damage which overkilled
        }
        return 0;
    }

    public void OnStartOfPlanning()
    {
        RecalculateProjections();
    }

    public override void OnResetToStart()
    {
        base.OnResetToStart();

        _dying = false;
        HullPoints = _hullPointsAtStart;
    }

    public override void OnEndOfTurn()
    {
        // actually dispose of ourselves if we died this turn
        if (_dying)
        {
            KillSelf();
        }

        base.OnEndOfTurn();

        _hullPointsAtStart = HullPoints;
        ProjectedPositions[0] = transform.position;
        ProjectedRotations[0] = transform.rotation;
        RecalculateProjections();
    }

    private void SetDeath()
    {
        // since we might want to rewind, can't actually destroy the object, just set it to die at end of turn and TODO make it invisible and trigger explosions etc
        _dying = true;
    }

    protected override void KillSelf()
    {
        // tell child objects to kill themselves (so we unregister any callbacks)


        GameManager.Instance.UnregisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.UnregisterOnEndOfTurn(OnEndOfTurn);

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
                pos += rot * Vector3.forward * stepLength * Mathf.Lerp(MinSpeed, MaxSpeed, SpeedProportion);
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

    public float GetSpeed()
    {
        return Mathf.Lerp(MinSpeed, MaxSpeed, SpeedProportion);
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

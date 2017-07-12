using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MobileObjectController))]
public class ShipController : MonoBehaviour {

    // ship details
    public string Name;

    public Faction Faction;

    public float MaxHullPoints;
    private float _hullPointsAtStart;
    public float HullPoints { get; private set; }
    
    public Vector3[] ProjectedPositions;
    public Quaternion[] ProjectedRotations;

    public LineRenderer ProjectedPath;

    public OffscreenIndicatorController OffscreenIndicator;

    public List<ShieldController> Shields { get; private set; }
    public List<WeaponController> Weapons { get; private set; }
    public List<ShipSectionController> ShipSections { get; private set; }

    public bool IsDying { get; private set; }

    private Action OnDeath;
    public void RegisterOnDeath(Action action) { OnDeath += action;  }
    public void UnregisterOnDeath(Action action) { OnDeath -= action; }
    
    private MobileObjectController _mob;

    private bool _showPath;

    // Use this for initialization
    public void Start ()
    {
        _mob = GetComponent<MobileObjectController>();

        HullPoints = MaxHullPoints;
        _hullPointsAtStart = HullPoints;
        
        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);

        InitialiseProjections();
        InitialiseShields();
        InitialiseWeapons();
        InitialiseShipSections();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.rotation.ToString());
    }

    private void InitialiseShipSections()
    {
        ShipSections = new List<ShipSectionController>();
        foreach (Transform t in transform)
        {
            var shipSection = t.GetComponent<ShipSectionController>();
            if (shipSection != null)
            {
                ShipSections.Add(shipSection);
            }
        }
    }

    internal void InitializeFromStruct(Ship ship, ChassisType type)
    {
        Name = ship.Name;
        transform.position = new Vector3(ship.PosX, ship.PosY, ship.PosZ);
        transform.rotation = Quaternion.Euler(ship.RotX, ship.RotY, ship.RotZ);
        _mob = GetComponent<MobileObjectController>();
        _mob.Acceleration = type.Acceleration;
        _mob.Deceleration = type.Deceleration;
        _mob.MaxSpeed = type.MaxSpeed;
        _mob.SetStartSpeed(ship.CurrentSpeed);
        InitialiseProjections();
        SetTurn(0f);
        SetSpeed(0.5f);
        Faction = ship.Faction == "Friendly" ? Faction.Friendly : Faction.Enemy;
    }

    private void InitialiseWeapons()
    {
        Weapons = new List<WeaponController>();
        foreach (Transform t in transform)
        {
            if (t.GetComponent<HardpointController>() != null)
            {
                foreach (Transform t1 in t)
                {
                    var weapon = t1.GetComponent<WeaponController>();
                    if (weapon != null)
                    {
                        Weapons.Add(weapon);
                    }
                }
            }
        }
    }

    private void InitialiseShields()
    {
        Shields = new List<ShieldController>();
        foreach (Transform t in transform)
        {
            if (t.GetComponent<HardpointController>() != null)
            {
                foreach (Transform t1 in t)
                {
                    var shield = t1.GetComponent<ShieldController>();
                    if (shield != null)
                    {
                        Shields.Add(shield);
                    }
                }
            }
        }
    }

    
    private void InitialiseProjections()
    {

        ProjectedPath.material.SetColor("_TintColor", FactionColors.PathColor[Faction]);

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
            Die();
            return -HullPoints; // return any damage which overkilled
        }
        return 0;
    }

    public void OnStartOfPlanning()
    {        
        RecalculateProjections();
        SetShowPath(true);
    }

    public void OnStartOfOutcome()
    {
        SetShowPath(false);
    }

    public void SetShowPath(bool enabled)
    {
        _showPath = enabled;
        ProjectedPath.enabled = enabled;
    }

    public void OnResetToStart()
    {
        if (IsDying)
        {
            IsDying = false;

            // make visible again
            ProjectedPath.enabled = true;
        }

        HullPoints = _hullPointsAtStart;
    }

    public void OnEndOfTurn()
    {
        // actually dispose of ourselves if we died this turn
        if (IsDying)
        {
            KillSelf();
        }        

        _hullPointsAtStart = HullPoints;
        ProjectedPositions[0] = transform.position;
        ProjectedRotations[0] = transform.rotation;
        RecalculateProjections();
    }

    public void Die()
    {
        // since we might want to rewind, can't actually destroy the object, just set it to die at end of turn and make it invisible and trigger explosions etc
        IsDying = true;

        foreach (ShieldController shield in Shields) { shield.Die(); }
        foreach (WeaponController weapon in Weapons) { weapon.Die(); }
        foreach (ShipSectionController shipSection in ShipSections) { shipSection.Die(); }
        ProjectedPath.enabled = false;
        OffscreenIndicator.enabled = false;

        if (OnDeath != null)
        {
            OnDeath();
        }
    }

    protected void KillSelf()
    {       
        // tell child objects to kill themselves (so we unregister any callbacks)
        foreach (ShieldController shield in Shields)
        {
            shield.KillSelf();
        }

        foreach (WeaponController weapon in Weapons)
        {
            weapon.KillSelf();
        }

        foreach (ShipSectionController shipSection in ShipSections)
        {
            shipSection.KillSelf();
        }

        GameObject.Destroy(OffscreenIndicator.transform.gameObject);

        GameManager.Instance.UnregisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.UnregisterOnEndOfTurn(OnEndOfTurn);

        GameManager.Instance.RemoveFromShipList(this);

        _mob.KillSelf();
    }

    public void RecalculateProjections()
    {
        // start at the beginning of the turn   
        Vector3 pos = ProjectedPositions[0];
        Quaternion rot = ProjectedRotations[0];

        int stepsPerStep = (int)(GameManager.TURN_LENGTH * 60f / GameManager.NUM_MOVEMENT_STEPS); // hack so we calculate more accurately but store less data
        float stepLength = 1f / 60f;

        float turn = GetTurn();
        float speed = GetSpeed();
        // now simulate the turns movement placing a point at the end of each
        for (int t = 1; t <= GameManager.NUM_MOVEMENT_STEPS; t++)
        {
            for (int x = 0; x < stepsPerStep; x++)
            {
                // apply proportion of the turn
                rot *= Quaternion.Euler(Vector3.up * turn * stepLength / GameManager.TURN_LENGTH);

                // move forward 1 step in the new direction
                pos += rot * Vector3.forward * stepLength * speed;
            }
            ProjectedPositions[t] = pos;
            ProjectedRotations[t] = rot;
        }

        // set the line renderer points
        if (ProjectedPath != null)
        {
            ProjectedPath.positionCount = ProjectedPositions.Length;
            ProjectedPath.SetPositions(ProjectedPositions);
        }
    }

    public float GetSpeed()
    {
        return _mob.GetSpeed();
    }

    public float GetTurn()
    {
        return _mob.GetTurn();
    }

    public void SetSpeed(float speedProportion)
    {
        _mob.SetSpeed(speedProportion);
        RecalculateProjections();
    }

    public void SetTurn(float turn)
    {
        _mob.SetTurn(turn);
        RecalculateProjections();
    }

}

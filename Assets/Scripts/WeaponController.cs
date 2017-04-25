using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public float MaxAngle; // in degrees
    public float Range;

    private float lastMaxAngle;
    private float lastRange;

    public LineRenderer FireArcIndicator;

    public Transform RotationPoint;
    public Transform FirePoint;

    public Transform Projectile;
    public float TimeBetweenShots;
    public float TimeSinceLastShot;

	// Use this for initialization
	void Start () {
        //TimeController.Instance.RegisterOnTimeChange(OnTimeChange);
	}

    // Update is called once per frame
    void Update()
    {
        RedrawFireArcIfChanged();

        if (GameManager.Instance.GameState == GameState.Processing)
        {
            if (TimeSinceLastShot >= TimeBetweenShots)
            {
                if (TargetInRange())
                {
                    Fire();
                }
            }
            TimeSinceLastShot += Time.deltaTime; // not sure here - am I trusting that time flowing same rate as time slider?
        }
    }

    private bool TargetInRange()
    {
        for (int i = 0; i < GameManager.Instance.Ships.Length; i++)
        {
            ShipController ship = GameManager.Instance.Ships[i];

            // don't target friends
            if (ship.Faction == Faction.Friendly) continue;

            // check range
            if (Vector3.Distance(transform.position, ship.transform.position) > Range) continue;

            // check firing arc
            float angle = Vector3.Angle(transform.forward, (ship.transform.position - transform.position)); 

            if (angle < MaxAngle / 2f)
            {
                RotationPoint.LookAt(ship.transform);
                return true;
            }
            
        }

        // if we got here, no target found
        return false; 
    }

    private void Fire()
    {
        // instantiate projectile 
        Transform t = Instantiate(Projectile);
        t.position = FirePoint.position;
        t.rotation = RotationPoint.rotation;
        ProjectileController projectile = t.GetComponent<ProjectileController>();
        //projectile.MaxAcceleration = 5f;

        projectile.TimeOffset = TimeController.Instance.GetTurnTime();
        projectile.DeathTime = projectile.TimeOffset + Range / (projectile.MaxAcceleration * GameManager.MOVEMENT_STEP_LENGTH); 

        TimeSinceLastShot = 0f;
    }

    private void RedrawFireArcIfChanged()
    {
        // redraw fire arc indicator if maxangle or range has changed
        if (Mathf.Abs(lastMaxAngle - MaxAngle) > 0.001f
            || Mathf.Abs(lastRange - Range) > 0.001f)
        {
            RedrawFireArc();
        }

        lastMaxAngle = MaxAngle;
        lastRange = Range;
    }

    private void RedrawFireArc()
    {
        float anglePerPoint = 5f;
        int pointsInArc = (int)(MaxAngle / anglePerPoint);
        if (pointsInArc < 2) pointsInArc = 2;
        Vector3[] points = new Vector3[pointsInArc + 2];

        // first and last points are at origin
        points[0] = new Vector3();
        points[points.Length - 1] = new Vector3();

        float angle = -MaxAngle / 2f;
        for (int i = 1; i <= points.Length - 2; i++)
        {
            float angleInRads = Mathf.Deg2Rad * angle;
            points[i] = new Vector3(Range * Mathf.Sin(angleInRads), 0, Range * Mathf.Cos(angleInRads));
            angle += (MaxAngle / pointsInArc);
        }

        // set the points
        FireArcIndicator.numPositions = points.Length;
        FireArcIndicator.SetPositions(points);
    }
}

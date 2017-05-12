using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public float MaxAngle; // in degrees
    public float Range;
    public float Damage;

    public Color LaserColor;
    
    public LineRenderer FireArcIndicator;

    public Transform RotationPoint;
    public Transform FirePoint;

    public Transform Projectile;
    public float TimeBetweenShots;
        
    private float TimeSinceLastShot;
    private float _timeSinceLastShotAtStart;

    private float lastMaxAngle;
    private float lastRange;
    private Faction _faction;
    
    // Use this for initialization
    void Start ()
    {
        // get faction of ship 
        _faction = transform.parent.GetComponent<ShipController>().Faction;
        LaserColor = FactionColors.LaserColor[_faction];

        GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
    }

    // Update is called once per frame
    void Update()
    {
        RedrawFireArcIfChanged();

        if (TimeController.Instance.Paused == false)
        {
            if (TimeSinceLastShot >= TimeBetweenShots)
            {
                if (TargetInRange())
                {
                    Fire();
                }
            }
            TimeSinceLastShot += Time.deltaTime; 
        }
    }

    public void OnResetToStart()
    {
        TimeSinceLastShot = _timeSinceLastShotAtStart;
    }

    private bool TargetInRange()
    {
        for (int i = 0; i < GameManager.Instance.Ships.Length; i++)
        {
            ShipController ship = GameManager.Instance.Ships[i];

            // don't target friends
            if (ship.Faction == _faction) continue;

            // check range
            float distance = Vector3.Distance(ship.transform.position, transform.position);
            float timeToTarget = distance / Projectile.GetComponent<ProjectileController>().MaxSpeed;

            // temporarily move object to provide lead   
            Debug.DrawRay(ship.transform.position + new Vector3(0,10,0), ship.transform.forward * timeToTarget * ship.SpeedProportion * ship.MaxSpeed, Color.red);         
            ship.transform.Translate(ship.transform.forward * timeToTarget * ship.SpeedProportion * ship.MaxSpeed);
            Vector3 target = ship.transform.position;
            ship.transform.Translate(ship.transform.forward * -timeToTarget * ship.SpeedProportion * ship.MaxSpeed);
 
            distance = Vector3.Distance(transform.position, target);
            if (distance <= Range)
            {
                // check firing arc
                float angle = Vector3.Angle(transform.forward, (target - transform.position));

                if (angle < MaxAngle / 2f)
                {
                    RotationPoint.LookAt(target);
                    return true;
                }
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

        // at the moment no friendly fire, bullets will only hit enemies
        projectile.LayerMask = (transform.gameObject.layer == GameManager.PLAYER_LAYER ? 1 << GameManager.ENEMY_LAYER : 1 << GameManager.PLAYER_LAYER);  
        projectile.Damage = Damage;

        LineRenderer renderer = projectile.GetComponent<LineRenderer>();

        renderer.material.SetColor("_TintColor", LaserColor);

        projectile.CreatedThisTurn = true; 

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

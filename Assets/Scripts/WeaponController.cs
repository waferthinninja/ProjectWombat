using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public string Name;
    public float MaxAngle; // in degrees
    public float Range;
    public float Damage;
    public float ProjectileSpeed;
    
    public LineRenderer FireArcIndicator;

    public Transform RotationPoint;
    public Transform FirePoint;

    public Transform Projectile;

    public float TimeBetweenShots;        
    private float _timeSinceLastShot;
    private float _timeSinceLastShotAtStart;

    private float _lastMaxAngle;
    private float _lastRange;

    private Faction _faction;
    private Color _laserColor;

    // Use this for initialization
    void Start ()
    {
        // get faction of ship 
        _faction = transform.parent.GetComponent<ShipController>().Faction;
        _laserColor = FactionColors.LaserColor[_faction];

        GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
    }

    // Update is called once per frame
    void Update()
    {
        RedrawFireArcIfChanged();

        if (TimeManager.Instance.Paused == false)
        {
            if (_timeSinceLastShot >= TimeBetweenShots)
            {
                if (TargetInRange())
                {
                    Fire();
                }
            }
            _timeSinceLastShot += Time.deltaTime; 
        }
    }

    public void OnResetToStart()
    {
        _timeSinceLastShot = _timeSinceLastShotAtStart;
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
            //Debug.DrawRay(ship.transform.position + new Vector3(0,10,0), ship.transform.forward * timeToTarget * ship.GetSpeed(), Color.red);
            Vector3 lead = ship.transform.forward * timeToTarget * ship.GetSpeed();
            ship.transform.Translate(lead);
            Vector3 target = ship.transform.position;
            ship.transform.Translate(-lead);
 
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
        projectile.Range = Range;
        projectile.MinSpeed = ProjectileSpeed;
        projectile.MaxSpeed = ProjectileSpeed;

        LineRenderer renderer = projectile.GetComponent<LineRenderer>();

        renderer.material.SetColor("_TintColor", _laserColor);

        projectile.CreatedThisTurn = true; 

        _timeSinceLastShot = 0f;
    }

    private void RedrawFireArcIfChanged()
    {
        // redraw fire arc indicator if maxangle or range has changed
        if (Mathf.Abs(_lastMaxAngle - MaxAngle) > 0.001f
            || Mathf.Abs(_lastRange - Range) > 0.001f)
        {
            RedrawFireArc();
        }

        _lastMaxAngle = MaxAngle;
        _lastRange = Range;
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

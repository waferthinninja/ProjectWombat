using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MobileObjectController))]
public class ProjectileController : MonoBehaviour {

    public float Damage;
    public float Range;

    private RaycastHit _hitInfo;
    private Ray _ray;
    public int LayerMask;
    
    private float _distanceTravelled;

    private MobileObjectController _mob;

    public void Awake()
    {
        _mob = GetComponent<MobileObjectController>();
        _mob.CreatedThisTurn = true;
        _distanceTravelled = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.Instance.Paused == false)
        {
            CheckForCollision();   

            _distanceTravelled += Time.deltaTime * _mob.MaxSpeed;
            if (_distanceTravelled >= Range)
            {
                _mob.KillSelf();
            }
        }
    }
    
    public float GetSpeed()
    {
        return _mob.MaxSpeed;
    }

    public void SetSpeed(float speed)
    {
        _mob.MaxSpeed = speed;
        _mob.Acceleration = speed;
        _mob.Deceleration = -speed;
    }

    private void CheckForCollision()
    {
        // Cast a ray 
        _ray = new Ray(transform.position, transform.forward);

        float lengthOfRay = _mob.MaxSpeed * Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward, Color.white);

        if (Physics.Raycast(_ray, out _hitInfo, lengthOfRay, LayerMask))
        {
            //print("Collided With " + _hitInfo.collider.gameObject.name);
            GameObject col = _hitInfo.collider.gameObject;
            if (col.GetComponent<ShieldController>() != null)
            {
                ShieldController shield = col.GetComponent<ShieldController>();

                float angle = Vector3.Angle(shield.RotationPoint.forward, -transform.forward);

                if (angle <= shield.Width * Mathf.Rad2Deg)
                {
                    Damage = shield.ApplyDamage(Damage);
                }
            }
            else if (col.GetComponent<ShipSectionController>() != null)
            {
                ShipSectionController c = col.GetComponent<ShipSectionController>();
                Damage = c.Ship.ApplyDamage(Damage); 
            }
        }

        if (Damage <= 0)
        {
            KillSelf();
        }

    }

    private void KillSelf()
    {
        _mob.KillSelf();
    }

}

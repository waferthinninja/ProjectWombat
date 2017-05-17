using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MobileObjectBase {

    RaycastHit _hitInfo;
    Ray _ray;
    //float _lengthOfRay = 2f;

    public int LayerMask;

    public float Damage;

    public float Range;

    private float _distanceTravelled;

    public override void Start()
    {
        base.Start();
        _distanceTravelled = 0;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();        
        
        if (TimeManager.Instance.Paused == false)
        {
            CheckForCollision();   

            _distanceTravelled += Time.deltaTime * MaxSpeed;
            if (_distanceTravelled >= Range)
            {
                KillSelf();
            }
        }
    }
    
    private void CheckForCollision()
    {
        // Cast a ray 
        _ray = new Ray(transform.position, transform.forward);

        float lengthOfRay = MaxSpeed * Time.deltaTime;
        Debug.DrawRay(transform.position, transform.forward, Color.white);

        if (Physics.Raycast(_ray, out _hitInfo, lengthOfRay, LayerMask))
        {
            print("Collided With " + _hitInfo.collider.gameObject.name);
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
            else if (col.GetComponent<ChassisController>() != null)
            {
                ChassisController c = col.GetComponent<ChassisController>();
                Damage = c.Ship.ApplyDamage(Damage); 
            }
        }

        if (Damage <= 0)
        {
            KillSelf();
        }

    }


}

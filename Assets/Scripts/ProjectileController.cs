using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MobileObjectBase {

    RaycastHit _hitInfo;
    Ray _ray;
    float _lengthOfRay = 5f;

    public int LayerMask;

    public float Damage;

    public override void Start()
    {
        base.Start();

        SetAcceleration(1f);
    }

    // Update is called once per frame
    void Update()
    {
        // only apply collisions in processing mode
        if (GameManager.Instance.GameState == GameState.Processing)
        {
            // Cast a ray 
            _ray = new Ray (transform.position, transform.forward );
            
            Debug.DrawRay(transform.position, transform.forward, Color.white);

            if (Physics.Raycast(_ray, out _hitInfo, _lengthOfRay, LayerMask))
            {
                print("Collided With " + _hitInfo.collider.gameObject.name);
                GameObject col = _hitInfo.collider.gameObject;
                if (col.GetComponent<ShieldController>() != null)
                {
                    ShieldController shield = col.GetComponent<ShieldController>();
                    
                    float angle = Vector3.Angle(shield.RotationPoint.forward, -transform.forward);
                    Debug.Log(angle);
                    if (angle <= shield.Width * Mathf.Rad2Deg)
                    {
                        Damage = shield.ApplyDamage(Damage);
                    }
                }            
            }

            if (Damage <= 0)
            {
                DeathTime = TimeController.Instance.GetTime();
            }
        }
    }
    

}

using System.Collections.Generic;
using Controllers.ShipComponents;
using Managers;
using Model.Enums;
using UnityEngine;
using Collision = Model.Structs.Collision;

namespace Controllers
{
    [RequireComponent(typeof(MobileObjectController))]
    public class ProjectileController : MonoBehaviour
    {
        private List<Collision> _collisions;

        private float _distanceTravelled;
        private RaycastHit _hitInfo;

        private MobileObjectController _mob;
        private Ray _ray;

        public float Damage;

        public int LayerMask;
        public float Range;

        public void Awake()
        {
            _mob = GetComponent<MobileObjectController>();
        }

        public void Initialize()
        {
            _mob.SetActive(true);
            _mob.SetActiveFrame(TimeManager.Instance.GetFrameNumber());
            _distanceTravelled = 0;
        }

        private void FixedUpdate()
        {
            if (GameManager.Instance.GameState == GameState.Replay)
            {
                // in replay mode we don't follow the normal process
                // instead we know where we start and end and what happens
            }

            if (TimeManager.Instance.Paused == false)
            {
                CheckForCollision();

                _distanceTravelled += Time.deltaTime * _mob.MaxSpeed;
                if (_distanceTravelled >= Range) _mob.KillSelf();
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

            var lengthOfRay = _mob.MaxSpeed * Time.deltaTime;
            Debug.DrawRay(transform.position, transform.forward, Color.white);

            if (Physics.Raycast(_ray, out _hitInfo, lengthOfRay, LayerMask))
            {
                //print("Collided With " + _hitInfo.collider.gameObject.name);
                var col = _hitInfo.collider.gameObject;
                if (col.GetComponent<ShieldController>() != null)
                {
                    var shield = col.GetComponent<ShieldController>();

                    var angle = Vector3.Angle(shield.RotationPoint.forward, -transform.forward);

                    if (angle <= shield.Width) Damage = shield.ApplyDamage(Damage);
                }
                else if (col.GetComponent<ShipSectionController>() != null)
                {
                    var c = col.GetComponent<ShipSectionController>();
                    Damage = c.Ship.ApplyDamage(Damage);
                }
            }

            if (Damage <= 0) KillSelf();
        }

        private void KillSelf()
        {
            _mob.KillSelf();
        }
    }
}
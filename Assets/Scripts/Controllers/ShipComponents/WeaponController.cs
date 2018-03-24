using Managers;
using Model;
using Model.Enums;
using Model.Structs;
using UnityEngine;

namespace Controllers.ShipComponents
{
    [RequireComponent(typeof(TargetableComponentController))]
    public class WeaponController : MonoBehaviour, IComponentController
    {
        private LineRenderer _arcIndicator;

        private bool _dying;

        private Faction _faction;
        private Color _laserColor;

        private float _lastMaxAngle;
        private float _lastRange;
        private MeshRenderer _meshRenderer;
        private bool _shotCharged;
        private bool _shotChargedAtStart;
        private bool _showArc;

        private TargetableComponentController _targeter;

        private float _timeSinceLastShot;
        private float _timeSinceLastShotAtStart;
        public float BaseDamage;
        public float BaseRange;

        public float DamageBoostCost = 1f; // hard coded for now, could make this variable?
        public Transform FirePoint;
        public float MaxAngle; // in degrees
        public ParticleSystem MuzzleFlareParticleSystem;

        public string Name;
        public PowerController PowerPlant;
        public ProjectileController Projectile;
        public float ProjectileSpeed;
        public float RangeBoostCost = 1f; // hard coded for now, could make this variable?

        public Transform RotationPoint;
        public float TimeBetweenShots;
        public bool FreeFire { get; private set; }
        public bool DamageBoosted { get; private set; }
        public bool RangeBoosted { get; private set; }

        internal void InitialiseFromStruct(Weapon weapon, WeaponType type)
        {
            Name = weapon.Name;
            BaseRange = type.Range;
            BaseDamage = type.Damage;
            ProjectileSpeed = type.ProjectileSpeed;
            TimeBetweenShots = type.TimeBetweenShots;
            MaxAngle = weapon.MaxAngle;
        }

        // Use this for initialization
        private void Start()
        {
            FreeFire = true;
            _showArc = true;
            _targeter = GetComponent<TargetableComponentController>();
            DamageBoosted = false;
            RangeBoosted = false;

            _arcIndicator = transform.Find("ArcIndicator").GetComponent<LineRenderer>();
            _meshRenderer = GetComponentInChildren<MeshRenderer>();

            // get faction of ship 
            _faction = transform.parent.parent.parent.GetComponent<ShipController>().Faction;
            _laserColor = FactionColors.LaserColor[_faction];

            transform.gameObject.layer =
                _faction == Faction.Friendly ? GameManager.PLAYER_LAYER : GameManager.ENEMY_LAYER;

            PowerPlant =
                GetComponentInParent<ShipController>()
                    .GetComponentInChildren<PowerController>(); // for now assumes one power plant per ship

            GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
            GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
            GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
            GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
        }

        // Update is called once per frame
        private void Update()
        {
            RedrawFireArcIfChanged();

            if (TimeManager.Instance.Paused == false && !_dying)
            {
                if (_timeSinceLastShot >= TimeBetweenShots || _shotCharged)
                    if (TargetInRange())
                    {
                        Fire();
                    }
                    else
                    {
                        _shotCharged = true;
                        _timeSinceLastShot = TimeBetweenShots;
                    }

                if (!_shotCharged) _timeSinceLastShot += Time.deltaTime;
            }
        }

        public void RegisterForTargetCallback()
        {
            _targeter.RegisterForTargetCallback();
        }

        public void ToggleArc()
        {
            _showArc = !_showArc;
            _arcIndicator.enabled = _showArc;
        }

        public void SetArc(bool enabled)
        {
            _showArc = enabled;
            _arcIndicator.enabled = enabled;
        }

        public void SetFreeFire(bool state)
        {
            FreeFire = state;
        }

        public void SetRangeBoost(bool state)
        {
            RangeBoosted = state;
            PowerPlant.ChangePower(state ? -RangeBoostCost : RangeBoostCost);
        }

        public void SetDamageBoost(bool state)
        {
            DamageBoosted = state;
            PowerPlant.ChangePower(state ? -DamageBoostCost : DamageBoostCost);
        }

        public void Die()
        {
            // since we might want to rewind, can't actually destroy the object, just set it to die at end of turn and make it invisible and trigger explosions etc
            _dying = true;
            _showArc = false;
            _meshRenderer.enabled = false;
        }

        public void KillSelf()
        {
            _targeter.KillSelf();

            GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);

            Destroy(transform.gameObject);
        }

        public void OnEndOfTurn()
        {
            _shotChargedAtStart = _shotCharged;
            _timeSinceLastShotAtStart = _timeSinceLastShot;
        }

        public void OnStartOfOutcome()
        {
            SetArc(false);
        }

        public void OnStartOfPlanning()
        {
            SetArc(true);
        }

        public void OnResetToStart()
        {
            if (_dying)
            {
                _dying = false;
                _meshRenderer.enabled = true;
            }

            _timeSinceLastShot = _timeSinceLastShotAtStart;
            _shotCharged = _shotChargedAtStart;
        }

        private float GetRange()
        {
            return BaseRange * (RangeBoosted ? 1.5f : 1f);
        }

        private bool TargetInRange()
        {
            for (var i = 0; i < GameManager.Instance.Ships.Count; i++)
            {
                var ship = GameManager.Instance.Ships[i];

                // don't target friends
                if (ship.Faction == _faction) continue;

                // don't target dead ships
                if (ship.IsDying) continue;

                // check range
                var distance = Vector3.Distance(ship.transform.position, transform.position);
                var timeToTarget = distance / ProjectileSpeed;


                // temporarily move object to provide lead   
                Debug.DrawRay(ship.transform.position + new Vector3(0, 10, 0),
                    ship.transform.forward * timeToTarget * ship.GetSpeed(), Color.red);
                var lead = ship.transform.forward * timeToTarget * ship.GetSpeed();
                ship.transform.position += lead;
                var target = ship.transform.position;
                ship.transform.position -= lead;

                distance = Vector3.Distance(transform.position, target);

                //Debug.Log(string.Format("{1} distance: {0} ", distance, Name));
                if (distance <= GetRange())
                {
                    // check firing arc
                    var angle = Vector3.Angle(transform.forward, target - transform.position);

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
            //Debug.Log("Firing " + Name  );
            // instantiate projectile 
            var t = LeanPool.Scripts.LeanPool.Spawn(Projectile);
            t.transform.position = FirePoint.position;
            t.transform.rotation = RotationPoint.rotation;
            var projectile = t.GetComponent<ProjectileController>();

            // at the moment no friendly fire, bullets will only hit enemies
            projectile.LayerMask = transform.gameObject.layer == GameManager.PLAYER_LAYER
                ? 1 << GameManager.ENEMY_LAYER
                : 1 << GameManager.PLAYER_LAYER;
            projectile.Damage = BaseDamage;
            projectile.Range = GetRange();
            projectile.SetSpeed(ProjectileSpeed);

            var renderers = projectile.GetComponentsInChildren<MeshRenderer>();

            foreach (var renderer in renderers) renderer.material.SetColor("_TintColor", _laserColor);

            _timeSinceLastShot -= TimeBetweenShots;
            _shotCharged = false;

            MuzzleFlareParticleSystem.Play();
        }

        private void RedrawFireArcIfChanged()
        {
            var range = GetRange();
            // redraw fire arc indicator if maxangle or range has changed
            if (Mathf.Abs(_lastMaxAngle - MaxAngle) > 0.001f
                || Mathf.Abs(_lastRange - range) > 0.001f)
                RedrawFireArc();

            _lastMaxAngle = MaxAngle;
            _lastRange = range;
        }

        private void RedrawFireArc()
        {
            var range = GetRange();
            var anglePerPoint = 5f;
            var pointsInArc = (int) (MaxAngle / anglePerPoint) + 1;
            if (pointsInArc < 2) pointsInArc = 2;
            var points = new Vector3[pointsInArc + 2];

            // first and last points are at origin
            points[0] = new Vector3();
            points[points.Length - 1] = new Vector3();

            var angle = -MaxAngle / 2f;
            for (var i = 1; i <= points.Length - 2; i++)
            {
                var angleInRads = Mathf.Deg2Rad * angle;
                points[i] = new Vector3(range * Mathf.Sin(angleInRads), 0, range * Mathf.Cos(angleInRads));
                angle += anglePerPoint;
            }

            // set the points
            _arcIndicator.positionCount = points.Length;
            _arcIndicator.SetPositions(points);
        }
    }
}
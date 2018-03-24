using Helper;
using Managers;
using Model;
using Model.Structs;
using UnityEngine;

namespace Controllers.ShipComponents
{
    [RequireComponent(typeof(TargetableComponentController))]
    public class ShieldController : MonoBehaviour, IComponentController
    {
        private const float MAX_INTENSITY = 0.3f;
        private Collider _collider;
        private float _currentHeight;

        private float _currentWidth;

        private bool _dying;
        private float _lastMaxAngle;
        private bool _showArc;
        private float _strength;
        private float _strengthAtStartOfTurn;

        private TargetableComponentController _targeter;
        public float ActivationSpeed = 50f; // how fast does it expand toward full size
        public LineRenderer ArcIndicator;
        public Color Color;
        public MeshRenderer ComponentRenderer;

        [Range(0f, 180f)] public float Height; // in degrees

        public float MaxAngle; // how far it can rotate, in degrees      
        public float MaxStrength = 100f;
        public string Name;
        public LineRenderer NEBeam;
        public LineRenderer NWBeam;
        public PowerController PowerPlant;
        public float Radius;
        public float RechargeAmount = 50f; // hard coded for now, make variable by shield type
        public float RechargeCost = 1f; // hard coded for now, make variable by shield type

        public Transform RotationPoint;
        public LineRenderer SEBeam;
        public MeshRenderer ShieldRenderer;
        public LineRenderer SWBeam;

        [Range(0f, 180f)] public float Width; // in degrees

        public bool Recharging { get; private set; }

        // Use this for initialization
        private void Start()
        {
            // TODO - get renderers automatically 

            _targeter = GetComponent<TargetableComponentController>();

            // get faction of ship 
            var ship = transform.parent.parent.parent.GetComponent<ShipController>();

            Color = FactionColors.ShieldColor[ship.Faction];

            _strength = MaxStrength;
            _strengthAtStartOfTurn = _strength;
            _collider = GetComponent<Collider>();

            SetShaderParams();
            SetBeamPositions();

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
            RedrawArcIfChanged();

            var target = _targeter.Target;

            if (target != null)
            {
                // turn to face target

                // check angle
                var angle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
                if (angle > 180) angle -= 360;

                if (angle < MaxAngle / 2f)
                {
                    RotationPoint.LookAt(target.transform);
                }
                else
                {
                    // if out of angle, turn as far as possible
                    RotationPoint.rotation = new Quaternion();
                    // cross product will tell us which way
                    var cross = Vector3.Cross(transform.forward, target.transform.position - transform.position);
                    RotationPoint.Rotate(0, MaxAngle / 2f * (cross.y < 0 ? -1 : 1), 0);
                }
            }

            if (_currentHeight != Height || _currentWidth != Width)
            {
                AdjustSize();
                SetShaderParams();
                SetBeamPositions();
            }
        }

        private void AdjustSize()
        {
            if (_currentHeight < 0.1f)
                _currentHeight = 0.1f;
            if (_currentWidth < Width)
            {
                _currentWidth += ActivationSpeed * Time.deltaTime;
            }
            else if (_currentWidth > Width)
            {
                _currentWidth -= ActivationSpeed * Time.deltaTime;
            }
            else
            {
                if (_currentHeight < Height)
                    _currentHeight += ActivationSpeed * Time.deltaTime;
                else if (_currentHeight > Height) _currentHeight -= ActivationSpeed * Time.deltaTime;
            }

            // clamp in case of overshoot
            _currentHeight = _currentHeight.Clamp(0, Height);
            _currentWidth = _currentWidth.Clamp(0, Width);
        }

        internal void InitialiseFromStruct(Shield shield, ShieldType type)
        {
            Name = shield.Name;
            Width = type.Width;
            Height = type.Height;
            Radius = shield.Radius;
            _currentWidth = 0;
            _currentHeight = 0;
            MaxStrength = type.Strength;
            _strength = shield.CurrentStrength;
            MaxAngle = shield.MaxAngle;
        }

        public ShipController GetTarget()
        {
            return _targeter.Target;
        }

        public float GetRotationProportion()
        {
            var angle = RotationPoint.localEulerAngles.y;
            if (angle > MaxAngle / 2f) angle -= 360;
            return angle / (MaxAngle / 2f);
        }

        public void SetRotationProportion(float prop)
        {
            RotationPoint.localRotation = Quaternion.Euler(0, prop * (MaxAngle / 2f), 0);
        }

        public void RegisterForTargetCallback()
        {
            _targeter.RegisterForTargetCallback();
        }

        public void ToggleArc()
        {
            _showArc = !_showArc;
            ArcIndicator.enabled = _showArc;
        }

        public void SetArc(bool enabled)
        {
            _showArc = enabled;
            ArcIndicator.enabled = enabled;
        }

        public void SetRecharging(bool state)
        {
            Recharging = state;
            PowerPlant.ChangePower(state ? -RechargeCost : RechargeCost);
        }

        private void RedrawArcIfChanged()
        {
            // redraw fire arc indicator if maxangle or range has changed
            if (Mathf.Abs(_lastMaxAngle - MaxAngle) > 0.001f) RedrawArc();

            _lastMaxAngle = MaxAngle;
        }

        private void RedrawArc()
        {
            var points = new Vector3[3];

            // middle point is at origin
            points[1] = new Vector3();

            var angle = MaxAngle / 2f;
            var angleInRads = Mathf.Deg2Rad * angle;
            angleInRads += Width * Mathf.Deg2Rad;
            points[0] = new Vector3(10f * Mathf.Sin(angleInRads), 0, 10f * Mathf.Cos(angleInRads));
            points[2] = new Vector3(10f * Mathf.Sin(-angleInRads), 0, 10f * Mathf.Cos(-angleInRads));

            // set the points
            ArcIndicator.positionCount = points.Length;
            ArcIndicator.SetPositions(points);
        }

        public void Die()
        {
            // since we might want to rewind, can't actually destroy the object, just set it to die at end of turn and make it invisible and trigger explosions etc
            _dying = true;

            ComponentRenderer.enabled = false;
            ArcIndicator.enabled = _showArc;
            EnableDisable(false);
        }

        public void OnResetToStart()
        {
            if (_dying)
            {
                ComponentRenderer.enabled = true;
                _dying = false;
            }

            _strength = _strengthAtStartOfTurn;
            EnableDisable(_strength > 0);
            ArcIndicator.enabled = _showArc;
            SetShaderParams();
            SetBeamPositions();
        }

        public void OnEndOfTurn()
        {
            _strengthAtStartOfTurn = _strength;
        }

        public void OnStartOfOutcome()
        {
            SetArc(false);
        }

        public void OnStartOfPlanning()
        {
            SetArc(true);
        }

        public float ApplyDamage(float damage)
        {
            _strength -= damage;

            SetShaderParams();

            if (_strength <= 0)
            {
                // shields down - deactivate collider             
                EnableDisable(false);

                // return any damage which continues through 
                return -_strength;
            }

            return 0;
        }

        private void SetShaderParams()
        {
            var alpha = _strength / MaxStrength * MAX_INTENSITY;
            var c = new Color(Color.r, Color.g, Color.b, alpha);
            ShieldRenderer.material.SetColor("_Color", c);
            //if (currentWidth != Width)
            //{
            //    currentWidth = Width;// * Time.deltaTime * speed;
            //}
            //if (currentHeight != Height)
            //{
            //    currentHeight = Height;// * Time.deltaTime * speed;
            //}

            ShieldRenderer.material.SetFloat("_WidthAngle", _currentWidth * Mathf.Deg2Rad);
            ShieldRenderer.material.SetFloat("_HeightAngle", _currentHeight * Mathf.Deg2Rad);
        }

        private void EnableDisable(bool enabled)
        {
            _collider.enabled = enabled;
            ShieldRenderer.enabled = enabled;
            NWBeam.enabled = enabled;
            NEBeam.enabled = enabled;
            SWBeam.enabled = enabled;
            SEBeam.enabled = enabled;
        }

        private void SetBeamPositions()
        {
            // beams
            SetBeamPosition(-_currentWidth, _currentHeight, NWBeam);
            SetBeamPosition(_currentWidth, _currentHeight, NEBeam);
            SetBeamPosition(-_currentWidth, -_currentHeight, SWBeam);
            SetBeamPosition(_currentWidth, -_currentHeight, SEBeam);
        }

        private void SetBeamPosition(float angle1, float angle2, LineRenderer lr)
        {
            angle1 = angle1 * Mathf.Deg2Rad;

            angle2 = angle2 * Mathf.Deg2Rad;
            var p1 = new Vector3(Mathf.Cos(angle1), 0, -Mathf.Sin(angle1));
            var p2 = new Vector3(0, Mathf.Cos(angle2), Mathf.Sin(angle2));


            // cross product
            var cp = Vector3.Cross(p1, p2);
            cp.Normalize();

            var numPoints = 2;
            var points = new Vector3[numPoints];
            points[0] = new Vector3();
            for (var i = 1; i < numPoints; i++) points[i] = Radius * cp / i;

            ShieldRenderer.transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);

            lr.positionCount = numPoints;
            lr.SetPositions(points);
        }

        public void KillSelf()
        {
            GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);
            GameManager.Instance.UnregisterOnStartOfOutcome(OnStartOfOutcome);
            GameManager.Instance.UnregisterOnStartOfPlanning(OnStartOfPlanning);
            GameManager.Instance.UnregisterOnEndOfTurn(OnEndOfTurn);

            Destroy(transform.gameObject);
        }
    }
}
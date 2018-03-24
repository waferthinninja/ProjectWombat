using Managers;
using UnityEngine;

namespace Controllers.ShipComponents
{
    [RequireComponent(typeof(LineRenderer))]
    public class TargetableComponentController : MonoBehaviour
    {
        private ShipController _targetAtStart;
        private LineRenderer _targetIndicator;

        public ShipController Target { get; private set; }

        // Use this for initialization
        private void Start()
        {
            _targetIndicator = GetComponent<LineRenderer>();
            GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
            GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Target != null)
            {
                var points = new Vector3[2];
                points[0] = Target.transform.position;
                points[1] = transform.position;
                _targetIndicator.SetPositions(points);
                _targetIndicator.enabled = true;
            }
            else
            {
                _targetIndicator.enabled = false;
            }
        }

        private void OnStartOfSimulation()
        {
//        Debug.Log(Target);
            _targetAtStart = Target;
        }

        public void RegisterForTargetCallback()
        {
            InputManager.Instance.RegisterOnTargetSelected(SetTarget);
        }

        public void OnResetToStart()
        {
            SetTarget(_targetAtStart);
        }

        public void SetTarget(ShipController target)
        {
            // if we already have a target, unregister that
            if (Target != null) Target.UnregisterOnDeath(OnDeathOfTarget);

            // register to hear about death
            Target = target;
            if (Target != null) Target.RegisterOnDeath(OnDeathOfTarget);
        }

        public void OnDeathOfTarget()
        {
            Target = null;
        }

        public void KillSelf()
        {
            GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);
            GameManager.Instance.UnregisterOnStartOfSimulation(OnStartOfSimulation);

            Destroy(transform.gameObject);
        }
    }
}
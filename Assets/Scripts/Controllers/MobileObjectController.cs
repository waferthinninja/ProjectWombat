using Controllers.ShipComponents;
using Managers;
using Model.Enums;
using UnityEngine;

namespace Controllers
{
    public sealed class MobileObjectController : MonoBehaviour
    {
        private int
            _activeFrame = -1; // this is set if the object came into existence during the turn (projectiles mainly) 

        private Collider[] _colliders;

        // prior to this frame, the object will be inactive   
        // -1 = existed at start of turn
        private int _deathFrame = -1; // this is set if the object dies during the turn

        private Vector3[] _positions;
        private Renderer[] _renderers;

        private Quaternion[] _rotations;
        // we don't dispose of the object as we might want to rewind to when it was alive
        // but should be inactive after this    
        // -1 = does not die this turn                        

        private PropulsionController _propulsion;

        // Use this for initialization
        public void Awake()
        {
            _positions = new Vector3[GameManager.Instance.FramesPerTurn + 1];
            _rotations = new Quaternion[GameManager.Instance.FramesPerTurn + 1];

            _propulsion = GetComponent<PropulsionController>()
            
            // register callbacks
            GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
            GameManager.Instance.RegisterOnResetToStart(OnResetToStart);

            _activeFrame = TimeManager.Instance.GetFrameNumber();

            // store the colliders and renderers so we can activate/deactivate them on the frames this object is "dead"
            _colliders = GetComponentsInChildren<Collider>();
            _renderers = GetComponentsInChildren<Renderer>();

            //SetActive(false); // will be reactivated on first FixedUpdate if this is start of the game
        }

        public void SetStartPosition(Vector3 pos, Quaternion rot)
        {
            _positions[0] = pos;
            _rotations[0] = rot;
        }

        public void SetActive(bool active)
        {
            // we can't actually set active to false else this will never run the FixedUpdate that reactivates it
            // so we fake deactivation by telling its object to turn off its renderers and colliders etc 
            foreach (var coll in _colliders) coll.enabled = active;

            foreach (var rend in _renderers) rend.enabled = active;
        }

        public void SetActiveFrame(int frame)
        {
            _activeFrame = frame;
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            if (TimeManager.Instance.GetFrameNumber() == _activeFrame) SetActive(true);

            if (GameManager.Instance.GameState == GameState.Replay)
            {
                // retrieve and apply positions from stored values
                transform.position = _positions[TimeManager.Instance.GetFrameNumber()];
                transform.rotation = _rotations[TimeManager.Instance.GetFrameNumber()];
            }
            else if (TimeManager.Instance.Paused == false)
            {
                // apply proportion of the turn
                transform.Rotate(0, TurnProportion * MaxTurn * Time.fixedDeltaTime / GameManager.TURN_LENGTH, 0);

                // move forward in the new direction
                transform.Translate(Vector3.forward * GetSpeed() * Time.fixedDeltaTime);

                // if we are in "outcome" mode, we must store positions
                if (GameManager.Instance.GameState == GameState.Outcome)
                {
                    _positions[TimeManager.Instance.GetFrameNumber()] = transform.position;
                    _rotations[TimeManager.Instance.GetFrameNumber()] = transform.rotation;
                }
            }
        }

        public float GetSpeed()
        {
            return Mathf.Lerp(GetMinSpeed(), GetMaxSpeed(), SpeedProportion);
        }

        public float GetMinSpeed()
        {
            return Mathf.Max(_speedAtStart - Deceleration, 0);
        }

        public float GetMaxSpeed()
        {
            return Mathf.Min(_speedAtStart + Acceleration, MaxSpeed);
        }

        public void SetStartSpeed(float speed)
        {
            _speedAtStart = speed;
        }

        private void OnEndOfTurn()
        {
            if (_deathFrame > -1)
            {
                // dispose
                ActuallyDie();
                return;
            }

            _activeFrame = -1;

            // reset start position
            _positions[0] = transform.position;
            _rotations[0] = transform.rotation;
            _speedAtStart = GetSpeed();

            // reset controls
            TurnProportion = 0f;
            SpeedProportion = 0.5f;
        }

        private void ActuallyDie() // hmm might have some issues here
        {
            LeanPool.Scripts.LeanPool.Despawn(transform.gameObject);

            // must unsubscribe from events
            GameManager.Instance.UnregisterOnEndOfTurn(OnEndOfTurn);
            GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);
        }

        public Vector3 GetStartPosition()
        {
            return _positions[0];
        }

        public Quaternion GetStartRotation()
        {
            return _rotations[0];
        }
        
        private void OnResetToStart()
        {
            ResetToStartPosition();
        }

        private void ResetToStartPosition()
        {
            transform.position = _positions[0];
            transform.rotation = _rotations[0];
        }

        public float GetTurn()
        {
            return TurnProportion * MaxTurn;
        }

        public void KillSelf()
        {
            _deathFrame = TimeManager.Instance.GetFrameNumber();
        }

        public void SetSpeed(float speedProportion)
        {
            SpeedProportion = speedProportion;
        }

        public void SetTurn(float turn)
        {
            TurnProportion = turn;
        }
    }
}
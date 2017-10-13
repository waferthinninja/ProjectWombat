using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileObjectController : MonoBehaviour
{    
    public int MobId { get; private set; }
    
    public float Acceleration;
    public float Deceleration;
    public float MaxSpeed;
    public float MaxTurn;

    // current settings
    public float SpeedProportion { get; private set; } 
    public float TurnProportion { get; private set; }    

    public bool CreatedThisTurn; // this is set if the object came into existence during the turn (projectiles mainly)    

    protected bool _active;
    
    private float _speedAtStart;

    private Vector3[] _positions;
    private Quaternion[] _rotations;

    // Use this for initialization
    public void Start()
    {
        MobId = GameManager.Instance.GetMobId();

        _positions = new Vector3[GameManager.Instance.FramesPerTurn + 1];
        _rotations = new Quaternion[GameManager.Instance.FramesPerTurn + 1];
        
        _positions[0] = transform.position;
        _rotations[0] = transform.rotation;

        SpeedProportion = 0.5f;

        // register callbacks
        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
        GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
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

            // move forward 1 seconds worth of movement in the new direction
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

    public virtual void OnEndOfTurn()
    {
        CreatedThisTurn = false;        

        // reset start position
        _positions[0] = transform.position;
        _rotations[0] = transform.rotation;
        _speedAtStart = GetSpeed();

        // reset controls
        TurnProportion = 0f;
        SpeedProportion = 0.5f;
    }

    public virtual void OnResetToStart()
    {
        ResetToStartPosition();
    }
    
    private void ResetToStartPosition()
    {
        if (CreatedThisTurn)
        {
            KillSelf();
            return;
        }

        transform.position = _positions[0];
        transform.rotation = _rotations[0];

    }

    public float GetTurn()
    {
        return TurnProportion * MaxTurn;
    }
        
    public void KillSelf()
    {
        // must unsubscribe from events
        GameManager.Instance.UnregisterOnEndOfTurn(OnEndOfTurn);
        GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);

        Destroy(this.transform.gameObject);
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

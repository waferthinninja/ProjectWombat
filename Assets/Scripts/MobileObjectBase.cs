using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileObjectBase : MonoBehaviour
{    
    public int MobId { get; private set; }
    
    public float MinSpeed;
    public float MaxSpeed;
    public float MaxTurn;

    // current settings
    public float SpeedProportion { get; private set; } 
    public float TurnProportion { get; private set; }    

    public bool CreatedThisTurn; // this is set if the object came into existence during the turn (projectiles mainly)    

    protected bool _active;

    private Vector3 _positionAtStart;
    private Quaternion _rotationAtStart;

    // Use this for initialization
    public virtual void Start()
    {
        MobId = GameManager.Instance.GetMobId();

        _positionAtStart = transform.position;
        _rotationAtStart = transform.rotation;

        // register callbacks
        GameManager.Instance.RegisterOnStartOfEndOfTurn(OnStartOfEndOfTurn);
        GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
    }

    // Update is called once per frame
    public void Update()
    {
        if (TimeController.Instance.Paused == false)
        {
            // apply proportion of the turn
            transform.Rotate(0, TurnProportion * MaxTurn * Time.deltaTime / GameManager.TURN_LENGTH, 0);

            // move forward 1 seconds worth of movement in the new direction
            float speed = MinSpeed + SpeedProportion * (MaxSpeed - MinSpeed);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void OnStartOfEndOfTurn()
    {
        CreatedThisTurn = false;        

        // reset start position
        _positionAtStart = transform.position;
        _rotationAtStart = transform.rotation;

        // reset controls
        TurnProportion = 0;
        SpeedProportion = 0;
    }

    public void OnResetToStart()
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

        transform.position = _positionAtStart;
        transform.rotation = _rotationAtStart;
    }

    
    protected virtual void KillSelf()
    {
        // must unsubscribe from events
        GameManager.Instance.UnregisterOnStartOfEndOfTurn(OnStartOfEndOfTurn);
        GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);

        Destroy(this.transform.gameObject);
    }

    public virtual void SetSpeed(float speedProportion)
    {
        SpeedProportion = speedProportion;        
    }

    public virtual void SetTurn(float turn)
    {
        TurnProportion = turn;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour {

    // ship details
    public int ShipId { get; private set; }
    public string ShipName;
    public float MaxAcceleration;
    public float MaxTurn;

    // current settings
    public float CurrentSpeed { get; private set; } // this is the speed as of last turn
    public float Acceleration { get; private set; } // this is the delta speed applied this turn rather than real acceleration
    public float TurnProportion { get; private set; }

    public GhostController Ghost;

    public Faction Faction;

    bool _ghostEnabled;

    private Vector3[] _path;
    private int _targetPathIndex;

	// Use this for initialization
	void Start ()
    {
        ShipId = GameManager.Instance.GetShipId();
        Debug.Log("Ship " + ShipName + " allocated id " + ShipId.ToString());

        // register callbacks
        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfPlayback(OnStartOfPlayback);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (GameManager.Instance.GameState == GameState.Playback)
        {
            float distanceToTravel = Mathf.Abs(Time.deltaTime * CurrentSpeed * 10f);
            while (distanceToTravel > 0 && _targetPathIndex < _path.Length)
            {
                // are we going to reach the next checkpoint
                if (Vector3.Distance(transform.position, _path[_targetPathIndex]) <= distanceToTravel)
                {
                    transform.position = _path[_targetPathIndex];
                    distanceToTravel -= Vector3.Distance(transform.position, _path[_targetPathIndex]);
                    _targetPathIndex++;
                    if (_targetPathIndex < _path.Length)
                    {
                        transform.Rotate(Vector3.up, TurnProportion * MaxTurn / GameManager.NUM_MOVEMENT_STEPS);
                    }
                    
                }
                else
                {
                    Vector3 direction = _path[_targetPathIndex] - transform.position;
                    direction.Normalize();
                    transform.position = transform.position + direction * distanceToTravel;
                    distanceToTravel = 0;
                }
            }

        }
	}

    // callback called at the start of the playback phase
    public void OnStartOfPlayback()
    {
        var lr = Ghost.GetComponent<LineRenderer>();
        _path = new Vector3[lr.numPositions];
        lr.GetPositions(_path);
        CurrentSpeed = CurrentSpeed + Acceleration;
        _targetPathIndex = 0;

        SetGhost(false);
    }

    // callback called at the start of the planning phase;
    public void OnStartOfPlanning()
    {
        Acceleration = 0;
        TurnProportion = 0;

        SetGhost(true);
    }

    public void SetAcceleration(float acceleration)
    {
        Acceleration = acceleration;
        Ghost.RecalculatePosition();
    }

    public void SetTurn(float turn)
    {
        TurnProportion = turn;
        Ghost.RecalculatePosition();
    }

    public void ToggleGhost()
    {
        SetGhost(!_ghostEnabled);
    }

    void SetGhost(bool enabled)
    {
        _ghostEnabled = enabled;
        Ghost.transform.gameObject.SetActive(_ghostEnabled);
    }   
}

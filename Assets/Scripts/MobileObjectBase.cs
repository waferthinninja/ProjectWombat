﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileObjectBase : MonoBehaviour {

    // ship details
    public int MobId { get; private set; }
    
    public float MaxAcceleration;
    public float MaxTurn;

    // current settings
    public float Acceleration { get; private set; } 
    public float TurnProportion { get; private set; }

    public Vector3[] ProjectedPositions;
    public Quaternion[] ProjectedRotations;

    public LineRenderer ProjectedPath;

    private int _targetIndex;

    public float TimeOffset; // this is set if the object came into existence during the turn (projectiles mainly)
    public float DeathTime; // this is set if the object dies during this turn

    protected bool _active;

    // Use this for initialization
    public virtual void Start()
    {
        MobId = GameManager.Instance.GetMobId();

        ProjectedPositions = new Vector3[GameManager.NUM_MOVEMENT_STEPS + 1];
        ProjectedRotations = new Quaternion[GameManager.NUM_MOVEMENT_STEPS + 1];

        ProjectedPositions[0] = transform.position;
        ProjectedRotations[0] = transform.rotation;
        RecalculateProjections();

        // register callbacks
        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnStartOfEndOfTurn(OnStartOfEndOfTurn);        
    }

    // Update is called once per frame
    public void Update()
    {
        if (GameManager.Instance.GameState == GameState.Outcome
         || GameManager.Instance.GameState == GameState.Simulation)
        {
            // apply proportion of the turn
            transform.Rotate(0, TurnProportion * MaxTurn * Time.deltaTime / GameManager.TURN_LENGTH, 0);

            // move forward 1 seconds worth of movement in the new direction
            transform.Translate(Vector3.forward * Acceleration * MaxAcceleration * Time.deltaTime);

        }
    }

    public void OnStartOfEndOfTurn()
    {
        DeathTime -= GameManager.NUM_MOVEMENT_STEPS;
        if (DeathTime <= 0)
        {
            KillSelf();
            return;
        }

        // move to the final position of previous turn
        if (TimeOffset > 0)
        {
            float endPos = GameManager.NUM_MOVEMENT_STEPS - TimeOffset;
            int i = (int)endPos;
            float rem = endPos - i;
            ProjectedPositions[0] = Vector3.Lerp(ProjectedPositions[i], ProjectedPositions[i + 1], rem);
            ProjectedRotations[0] = Quaternion.Lerp(ProjectedRotations[i], ProjectedRotations[i + 1], rem);

            // clear for next turn
            TimeOffset = 0;
        }
        else
        {
            ProjectedPositions[0] = ProjectedPositions[GameManager.NUM_MOVEMENT_STEPS];
            ProjectedRotations[0] = ProjectedRotations[GameManager.NUM_MOVEMENT_STEPS];
        }
        // reset controls and calculate new projected path
        //Acceleration = 0;
        TurnProportion = 0;
        RecalculateProjections();
    }
    
    public void OnStartOfOutcome()
    {
        ResetToStartPosition();
    }

    public void OnStartOfSimulation()
    {
        ResetToStartPosition();
    }
    public void OnStartOfWaitingForOpponent()
    {
        ResetToStartPosition();
    }

    public void OnStartOfPlanning()
    {
        ResetToStartPosition();
    }

    private void ResetToStartPosition()
    {
        transform.position = ProjectedPositions[0];
        transform.rotation = ProjectedRotations[0];
    }

    private void KillSelf()
    {
        // must unsubscribe from events
        GameManager.Instance.UnregisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.UnregisterOnStartOfPlanning(OnStartOfOutcome);
        GameManager.Instance.UnregisterOnStartOfEndOfTurn(OnStartOfEndOfTurn);
        TimeController.Instance.UnregisterOnTimeChange(OnTimeChange);

        Destroy(this.transform.gameObject);
    }

    protected void SetActive(bool active)
    {
        _active = active;
        var renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)
        {
            r.enabled = active;
        }
    }

    public void OnTimeChange(float time)
    {
        if (time > DeathTime)
        {
            SetActive(false);
            return;
        }
        time -= TimeOffset;
        if (time < 0)
        {
            SetActive(false);            
            return;
        }
        if (!_active)
        {
            SetActive(true);            
        }

        int i = (int)time;
        float rem = time - i;
        if (i > ProjectedPositions.Length - 1 || (i == ProjectedPositions.Length - 1 && rem < 0.0001f))
        {
            transform.position = ProjectedPositions[i];
            transform.rotation = ProjectedRotations[i];
        }
        else
        {
            transform.position = Vector3.Lerp(ProjectedPositions[i], ProjectedPositions[i + 1], rem);
            transform.rotation = Quaternion.Lerp(ProjectedRotations[i], ProjectedRotations[i + 1], rem);
        }
    }

    public void SetAcceleration(float acceleration)
    {
        Acceleration = acceleration;
        RecalculateProjections();
    }

    public void SetTurn(float turn)
    {
        TurnProportion = turn;
        RecalculateProjections();
    }


    public void RecalculateProjections()
    {
        // start at the beginning of the turn   
        Vector3 pos = ProjectedPositions[0];
        Quaternion rot = ProjectedRotations[0];

        int stepsPerStep = (int)(GameManager.TURN_LENGTH * 60f / GameManager.NUM_MOVEMENT_STEPS); // hack so we calculate more accurately but store less data
        float stepLength = 1f / 60f;

        // now simulate the turns movement placing a point at the end of each
        for (int t = 1; t <= GameManager.NUM_MOVEMENT_STEPS; t++)
        {
            for (int x = 0; x < stepsPerStep; x++)
            {
                // apply proportion of the turn
                rot *= Quaternion.Euler(Vector3.up * TurnProportion * MaxTurn * stepLength / GameManager.TURN_LENGTH);

                // move forward 1 step in the new direction
                pos += rot * Vector3.forward * stepLength * (Acceleration * MaxAcceleration);
            }
            ProjectedPositions[t] = pos;
            ProjectedRotations[t] = rot;
        }

        // set the line renderer points
        if (ProjectedPath != null)
        {
            ProjectedPath.numPositions = ProjectedPositions.Length;
            ProjectedPath.SetPositions(ProjectedPositions);
        }
    }

}

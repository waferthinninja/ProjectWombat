using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    //MAKE INSTANCE
    private static TimeManager _instance;

    public static TimeManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<TimeManager>();
            return _instance;
        }
    }
    //END MAKE INSTANCE

    public Slider TimeSlider; 

    public bool Paused;

    private Action OnEndOfPlayback;

    public void RegisterOnEndOfPlayback(Action action) { OnEndOfPlayback += action; }

    public void UnregisterOnEndOfPlayback(Action action) { OnEndOfPlayback += action; }

    void Start()
    {
        // set values on time slider
        TimeSlider.maxValue = GameManager.NUM_MOVEMENT_STEPS;

        GameManager.Instance.RegisterOnStartOfPlanning_Late(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
    }

    void Update()
    {
        if (Paused)
        {
            return;
        }
        else
        {
            float baseTimeMultiplier = 1f / GameManager.MOVEMENT_STEP_LENGTH;
            TimeSlider.value += Time.deltaTime * baseTimeMultiplier;

            // if we have reached the end of the turn, advance
            if (TimeSlider.value >= GameManager.NUM_MOVEMENT_STEPS)
            {
                Paused = true;
                EndOfPlayback();
            }
        }
    }
    
    public float GetTime()
    {
        return TimeSlider.value;
    }


    public void OnStartOfPlanning()
    {
        TimeSlider.value = 0;
        Paused = true;
    }

    public void OnStartOfOutcome()
    {
        TimeSlider.value = 0;
        Paused = false;
    }

    public void OnStartOfWaitingForOpponent()
    {
        TimeSlider.value = 0;
        Paused = true;
    }

    public void OnStartOfSimulation()
    {
        TimeSlider.value = 0;
        Paused = false;
    }

    public void OnEndOfTurn()
    {
        TimeSlider.value = 0;
        Paused = true;
    }

    public void EndOfPlayback()
    {
        if (OnEndOfPlayback != null)
        {
            OnEndOfPlayback();
        }
    }
       
}

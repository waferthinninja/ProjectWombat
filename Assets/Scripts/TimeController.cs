using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

    //MAKE INSTANCE
    private static TimeController _instance;

    public static TimeController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<TimeController>();
            return _instance;
        }
    }
    //END MAKE INSTANCE

    public Slider TimeSlider; 

    public bool Paused;
    
    void Start()
    {
        // set values on time slider
        TimeSlider.maxValue = GameManager.NUM_MOVEMENT_STEPS;

        GameManager.Instance.RegisterOnStartOfPlanning_Late(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnStartOfEndOfTurn(OnStartOfEndOfTurn);
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

    public void OnStartOfEndOfTurn()
    {
        TimeSlider.value = 0;
        Paused = true;
    }
       
}

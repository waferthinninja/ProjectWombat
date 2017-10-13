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
        TimeSlider.maxValue = GameManager.Instance.FramesPerTurn;

        GameManager.Instance.RegisterOnStartOfPlanning_Late(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfWaitingForOpponent(OnStartOfWaitingForOpponent);
        GameManager.Instance.RegisterOnStartOfSimulation(OnStartOfSimulation);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnStartOfReplay(OnStartOfReplay);
        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);

        Paused = true;
    }

    void FixedUpdate()
    {
        if (Paused)
        {
            return;
        }
        else
        {
            // if we have reached the end of the turn, advance
            if (TimeSlider.value == GameManager.Instance.FramesPerTurn)
            {
                Paused = true;
                EndOfPlayback();
            }

            TimeSlider.value += 1;            
        }
    }
    
    public int GetFrameNumber()
    {
        return (int)TimeSlider.value;
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

    public void OnStartOfReplay()
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

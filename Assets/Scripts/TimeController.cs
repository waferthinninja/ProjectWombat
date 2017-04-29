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
    private Action<float> OnTimeChange;
    
    public void RegisterOnTimeChange(Action<float> action) { OnTimeChange += action; }

    public void UnregisterOnTimeChange(Action<float> action) { OnTimeChange -= action; }

    void Start()
    {
        GameManager.Instance.RegisterOnStartOfPlanning_Late(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfProcessing(OnStartOfProcessing);
        GameManager.Instance.RegisterOnStartOfPlayback(OnStartOfPlayback);
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Processing)
        {
            float baseTimeMultiplier = 1f / GameManager.MOVEMENT_STEP_LENGTH;
            TimeSlider.value += Time.deltaTime * baseTimeMultiplier;

            // if we have reached the end of the turn, advance
            if (TimeSlider.value >= GameManager.NUM_MOVEMENT_STEPS)
            {
                GameManager.Instance.StartPlaybackPhase();
            }
            ApplyTimeSlider();
        }
    }

    public float GetTime()
    {
        return TimeSlider.value;
    }

    public void OnStartOfPlanning()
    {
        TimeSlider.value = 0;
        ApplyTimeSlider();
    }

    public void OnStartOfProcessing()
    {
        TimeSlider.value = 0;
        ApplyTimeSlider();
    }

    public void OnStartOfPlayback()
    {
        //TimeSlider.value = 0;
        //ApplyTimeSlider();
    }


    public void ApplyTimeSlider()
    {
        if (OnTimeChange != null)
        {
            OnTimeChange(TimeSlider.value);
        }
    }
}

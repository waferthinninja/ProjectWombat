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

    void Start()
    {

        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfPlayback(OnStartOfPlayback);
    }

    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playback)
        {
            float baseTimeMultiplier = 1f / GameManager.MOVEMENT_STEP_LENGTH;
            TimeSlider.value += Time.deltaTime * baseTimeMultiplier;
            ApplyTimeSlider();
        }
    }

    public void OnStartOfPlanning()
    {

    }

    public void OnStartOfPlayback()
    {
        TimeSlider.value = 0;
        ApplyTimeSlider();
    }

    public void RegisterOnTimeChange(Action<float> action)
    {
        OnTimeChange += action;
    }

    public void ApplyTimeSlider()
    {
        if (OnTimeChange != null)
        {
            OnTimeChange(TimeSlider.value);
        }
    }
}

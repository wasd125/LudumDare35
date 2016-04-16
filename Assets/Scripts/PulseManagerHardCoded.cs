﻿using UnityEngine;
using System.Collections;
using System;

public class PulseManagerHardCoded : MonoBehaviour {

    public static PulseManagerHardCoded Instance;
    public AudioClip clip;

    float BASE_PULSE_TIMER = 0.481f;
    float max_Pulse_Timer;
    float currentPulseTimer = 0f;

    float pitchMultiplier = 1;

    Color[] colors = new Color[] { Color.black, Color.blue, Color.green, Color.white,Color.red,Color.yellow,Color.gray };

    public Action Pulse;

    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (Instance == null)
        {
            // If that is the case, we destroy other instances
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SoundManager.Instance.PlayMusik(clip);
    }

    public void SetPitch(float pitch)
    {
        pitchMultiplier = 2 - pitch;

        currentPulseTimer = currentPulseTimer * pitchMultiplier;
    }

    public float GetPitch()
    {
        return pitchMultiplier;
    }

	// Update is called once per frame
	void Update () {

        currentPulseTimer -= Time.deltaTime;

        if (currentPulseTimer <= 0 && Pulse != null)
        {
            Pulse();
            currentPulseTimer = BASE_PULSE_TIMER * pitchMultiplier;
        }
	}

    public void RegisterPulse(Action action)
    {
        Pulse += action;
    }

    public void UnregisterPulse(Action action)
    {
        Pulse -= action;
    }
}
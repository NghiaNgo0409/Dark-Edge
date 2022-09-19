using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystemUI : MonoBehaviour
{
    public static PauseSystemUI Instance {get; private set;}
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject settingCanvas;
    public event EventHandler OnPauseSystemTurnOn;
    public event EventHandler OnPauseSystemTurnOff;
    void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetIsPause()
    {
        return isPaused;
    }

    public void TurnOnPauseSystem()
    {
        pauseCanvas.SetActive(true);
        isPaused = true;
        OnPauseSystemTurnOn?.Invoke(this, EventArgs.Empty);
    }

    public void TurnOffPauseSystem()
    {
        pauseCanvas.SetActive(false);
        isPaused = false;
        OnPauseSystemTurnOff?.Invoke(this, EventArgs.Empty);
    }

    public void TurnOnSettingSystem()
    {
        settingCanvas.SetActive(true);
    }

    public void TurnOffSettingSystem()
    {
        settingCanvas.SetActive(false);
    }
}

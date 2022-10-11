using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleValueChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }

    private void HandleValueChanged(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * multiplier);
    }
}

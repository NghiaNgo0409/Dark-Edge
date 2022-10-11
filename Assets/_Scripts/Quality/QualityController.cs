using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class QualityController : MonoBehaviour
{
    [SerializeField] List<RenderPipelineAsset> qualities;
    [SerializeField] List<Toggle> toggleList;
    [SerializeField] string modeQuality;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Toggle>().onValueChanged.AddListener(ChangeQuality);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeQuality(bool boolean)
    {
        foreach (Toggle toggle in toggleList)
        {
            toggle.isOn = false;
            toggle.interactable = true;
        }

        this.GetComponent<Toggle>().isOn = boolean;
        this.GetComponent<Toggle>().interactable = false;

        if(modeQuality == "Low")
        {
            QualitySettings.SetQualityLevel(0);
            QualitySettings.renderPipeline = qualities[0];
        }
        else if(modeQuality == "Medium")
        {
            QualitySettings.SetQualityLevel(1);
            QualitySettings.renderPipeline = qualities[0];
        }
        else
        {
            QualitySettings.SetQualityLevel(2);
            QualitySettings.renderPipeline = qualities[0];
        }
    }
}

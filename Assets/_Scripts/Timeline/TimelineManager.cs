using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }
    float timeChange = 10.25f;
    [SerializeField] GameObject timelineCanvas;
    [SerializeField] PlayableDirector endGameTimeLine;
    [SerializeField] GameObject inGameCanvas;
    [SerializeField] GameObject endGameCanvas;
    [SerializeField] GameObject actor;
    bool isTimeline = true;
    private void Awake()
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
    // Start is called before the first frame update
    void Start()
    {
        InteractHelicopter.OnAnyHelicopterStart += InteractWeapon_OnAnyHelicopterStart;
        endGameTimeLine.played += EndGameDirector_Played;
        endGameTimeLine.stopped += EndGameDirector_Stopped;
    }

    // Update is called once per frame
    void Update()
    {
        timeChange -= Time.deltaTime;
        if(timeChange <= 0)
        {
            isTimeline = false;
            timelineCanvas.SetActive(false);
            inGameCanvas.SetActive(true);
        }
    }

    void InteractWeapon_OnAnyHelicopterStart(object sender, EventArgs e)
    {
        actor.SetActive(true);
        UnitActionSystem.Instance.GetSelectingUnit().gameObject.SetActive(false);
        endGameTimeLine.Play();
    }

    void EndGameDirector_Played(PlayableDirector obj)
    {
        isTimeline = true;
        inGameCanvas.SetActive(false);
    }
    void EndGameDirector_Stopped(PlayableDirector obj)
    {
        endGameCanvas.SetActive(true);
    }

    public bool IsTimeline()
    {
        return isTimeline;
    }
}

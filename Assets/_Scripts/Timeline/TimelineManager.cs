using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }
    float timeChange = 10.25f;
    [SerializeField] GameObject timelineCanvas;
    [SerializeField] PlayableDirector endGameTimeLine;
    [SerializeField] GameObject inGameCanvas;
    [SerializeField] GameObject endGameCanvas;
    [SerializeField] GameObject endingCanvas;
    [SerializeField] GameObject actor;
    [SerializeField] CinemachineVirtualCamera cm1;
    [SerializeField] CinemachineVirtualCamera cm2;
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
        actor = GameObject.Find("-------Units-------").transform.GetChild(0).gameObject;
        endGameTimeLine.played += EndGameDirector_Played;
        endGameTimeLine.stopped += EndGameDirector_Stopped;
    }

    // Update is called once per frame
    void Update()
    {
        timeChange -= Time.deltaTime;
        if(timeChange <= 0 && !GameManager.Instance.IsEndGame())
        {
            isTimeline = false;
            timelineCanvas.SetActive(false);
            inGameCanvas.SetActive(true);
        }
    }

    private void OnEnable()
    {
        InteractHelicopter.OnAnyHelicopterStart += InteractWeapon_OnAnyHelicopterStart;
    }

    private void OnDisable()
    {
        InteractHelicopter.OnAnyHelicopterStart -= InteractWeapon_OnAnyHelicopterStart;
    }

    void InteractWeapon_OnAnyHelicopterStart(object sender, EventArgs e)
    {
        UnitActionSystem.Instance.GetSelectingUnit().gameObject.SetActive(false);
        actor.SetActive(true);
        endingCanvas.SetActive(true);
        cm2.Priority = 100;
        endGameTimeLine.Play();
    }

    void EndGameDirector_Played(PlayableDirector obj)
    {
        isTimeline = true;
        inGameCanvas.SetActive(false);
    }
    void EndGameDirector_Stopped(PlayableDirector obj)
    {
        inGameCanvas.SetActive(true);
        endGameCanvas.SetActive(true);
    }

    public bool IsTimeline()
    {
        return isTimeline;
    }
}

    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnNumberText;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTurnNumber();

        TurnSystem.Instance.OnTurnChanged += OnTurnChanged_UpdateTurnSystemUI;
    }

    void OnTurnChanged_UpdateTurnSystemUI(object sender, EventArgs e)
    {
        UpdateTurnNumber();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTurnNumber()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }
}

    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI turnNumberText;
    [SerializeField] GameObject endTurnBtn; 
    [SerializeField] GameObject enemyTurnUI;
    // Start is called before the first frame update
    void Start()
    {
        UpdateTurnNumber();
        UpdateEnemyTurnUI();
        UpdateEndTurnButton();

        TurnSystem.Instance.OnTurnChanged += OnTurnChanged_UpdateTurnSystemUI;
    }

    void OnTurnChanged_UpdateTurnSystemUI(object sender, EventArgs e)
    {
        UpdateTurnNumber();
        UpdateEndTurnButton();
        UpdateEnemyTurnUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTurnNumber()
    {
        turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurnNumber();
    }

    void UpdateEnemyTurnUI()
    {
        enemyTurnUI.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    void UpdateEndTurnButton()
    {
        endTurnBtn.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}

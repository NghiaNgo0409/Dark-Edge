using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionPointsText;
    [SerializeField] Unit unit;
    [SerializeField] Image healthBar;
    [SerializeField] HealthSystem healthSystem;
    // Start is called before the first frame update
    void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged; 
        healthSystem.OnDamage += HealthSystem_OnDamage;

        UpdateActionPointsText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = healthSystem.GetHealthNormalized();
    }

    void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    void HealthSystem_OnDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }
}

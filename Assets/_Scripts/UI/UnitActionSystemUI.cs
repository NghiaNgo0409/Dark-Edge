using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionSystemUIPrefab;
    [SerializeField] Transform actionSystemUIContainer;
    [SerializeField] TextMeshProUGUI actionPointsText; 
    List<ActionButtonUI> actionButtonUIList;
    void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += OnEventUnitChanged_UpdateActionUI;
        UnitActionSystem.Instance.OnSelectedActionChange += OnEventActionChanged_UpdateActionUI;
        UnitActionSystem.Instance.OnActionStarted += OnActionStarted_UpdateActionUI;
        TurnSystem.Instance.OnTurnChanged += OnTurnChanged_UpdateSystem;
        Unit.OnAnyActionPointsChanged += OnAnyActionPoinsChanged_UpdateSystem;
        Unit.OnAnyUnitPickUpItems += Unit_OnAnyUnitPickUpItems; 
        CreateActionUIButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateActionUIButtons()
    {
        foreach(Transform actionSingle in actionSystemUIContainer)
        {
            Destroy(actionSingle.gameObject);
        }
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectingUnit();

        actionButtonUIList.Clear();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            if((baseAction.GetBaseActionName() == "Shoot" && !selectedUnit.DoesHasGun())) continue;

            if ((baseAction.GetBaseActionName() == "Sword" && !selectedUnit.DoesHasMelee())) continue; 
            Transform baseActionTransform = Instantiate(actionSystemUIPrefab, actionSystemUIContainer);
            ActionButtonUI actionButtonUI = baseActionTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
            actionButtonUIList.Add(actionButtonUI);
        }
    }

    void OnEventUnitChanged_UpdateActionUI(object sender, EventArgs e) 
    {
        CreateActionUIButtons();
        UpdateSelectedVisual();
    }

    void OnEventActionChanged_UpdateActionUI(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    void OnActionStarted_UpdateActionUI(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    void OnTurnChanged_UpdateSystem(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    void OnAnyActionPoinsChanged_UpdateSystem(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    void Unit_OnAnyUnitPickUpItems(object sender, EventArgs e)
    {
        CreateActionUIButtons();
        UpdateSelectedVisual();
    }

    void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    void UpdateActionPoints()
    {
        Unit selectingUnit = UnitActionSystem.Instance.GetSelectingUnit();
        actionPointsText.text = "Action Points: " + selectingUnit.GetActionPoints();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionSystemUIPrefab;
    [SerializeField] Transform actionSystemUIContainer;
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
        CreateActionUIButtons();
        UpdateSelectedVisual();
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

    void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }
}

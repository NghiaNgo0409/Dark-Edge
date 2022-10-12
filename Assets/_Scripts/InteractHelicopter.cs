using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHelicopter : MonoBehaviour, IInteractable
{
    public static event EventHandler OnAnyHelicopterStart;

    GridPosition gridPosition;
    Action onInteractCompleted;
    float timer;
    bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 helicopterPos = new Vector3(26, 0, 100);
        gridPosition = LevelGrid.Instance.GetGridPosition(helicopterPos);
        LevelGrid.Instance.SetInteractableObjectAtGridPosition(gridPosition, this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isActive = false;
            onInteractCompleted();
        }
    }

    public void Interact(Action onInteractActionComplete)
    {
        this.onInteractCompleted = onInteractActionComplete;
        isActive = true;
        timer = 1f;
        OnAnyHelicopterStart?.Invoke(this, EventArgs.Empty);
    }
}

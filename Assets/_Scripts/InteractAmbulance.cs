using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAmbulance : MonoBehaviour, IInteractable
{
    GridPosition gridPosition;
    Action onInteractCompleted;
    float timer;
    bool isActive;

    [SerializeField] List<GameObject> itemContainer;

    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
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
        foreach (GameObject item in itemContainer)
        {
            Instantiate(item, transform.position + Vector3.left * 3f, Quaternion.identity);
        }
    }
}

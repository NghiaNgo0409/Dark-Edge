using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    GridPosition gridPosition;
    Action onInteractCompleted;
    float timer;
    bool isActive;

    [SerializeField] bool isGreen;
    [SerializeField] Material redMaterial;
    [SerializeField] Material greenMaterial;
    [SerializeField] MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableObjectAtGridPosition(gridPosition, this);

        if(isGreen)
        {
            SetGreenColor();
        }
        else
        {
            SetRedColor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive) return;
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            isActive = false;
            onInteractCompleted();
        }
    }

    
    public void Interact(Action onInteractActionComplete)
    {
        this.onInteractCompleted = onInteractActionComplete;
        isActive = true;
        timer = .2f;
        if(isGreen)
        {
            SetRedColor();
        }
        else
        {
            SetGreenColor();
        }
    }

    void SetRedColor()
    {
        isGreen = false;
        meshRenderer.material = redMaterial;
    }

    void SetGreenColor()
    {
        isGreen = true;
        meshRenderer.material = greenMaterial;
    }
}

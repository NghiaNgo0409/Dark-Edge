using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractWeapon : MonoBehaviour, IInteractable
{
    public static event EventHandler OnAnySpawnItem;
    public static event EventHandler OnAnyItemCollected;    
    public static event EventHandler OnAnyEndItemCollected;
    enum WeaponType
    {
        Gun,
        Melee,
        Pill,
        EndItem,
    }

    GridPosition gridPosition;
    Action onInteractCompleted;
    float timer;
    bool isActive;

    [SerializeField] WeaponType weaponType;
    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableObjectAtGridPosition(gridPosition, this);
        OnAnySpawnItem?.Invoke(this, EventArgs.Empty);
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
            Destroy(gameObject);
            LevelGrid.Instance.RemoveInteractableObjectAtGridPosition(gridPosition);
            OnAnyItemCollected?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Interact(Action onInteractActionComplete)
    {
        this.onInteractCompleted = onInteractActionComplete;
        isActive = true;
        timer = .2f;
        Unit selectingUnit = UnitActionSystem.Instance.GetSelectingUnit();
        switch (weaponType)
        {
            case WeaponType.Gun:
                selectingUnit.SetGun(true);
                selectingUnit.GetComponent<ShootAction>().enabled = true;
                break;
            case WeaponType.Melee:
                selectingUnit.SetMelee(true);
                selectingUnit.GetComponent<SwordAction>().enabled = true;
                break;
            case WeaponType.Pill:
                GameManager.Instance.ShowWinCanvas();
                GameManager.Instance.SetMap(SceneManager.GetActiveScene().buildIndex);
                break;
            case WeaponType.EndItem:
                OnAnyEndItemCollected?.Invoke(this, EventArgs.Empty);
                break;
            default:
                break;
        }
    }
}

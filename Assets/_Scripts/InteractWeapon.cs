using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractWeapon : MonoBehaviour, IInteractable
{
    public static event EventHandler OnAnySpawnItem;
    enum WeaponType
    {
        Gun,
        Melee,
        Pill
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
        }
    }

    public void Interact(Action onInteractActionComplete)
    {
        this.onInteractCompleted = onInteractActionComplete;
        isActive = true;
        timer = .2f;
        Unit selectingUnit = UnitActionSystem.Instance.GetSelectingUnit();
        switch(weaponType)
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
            default:
                break;
        }
    }
}

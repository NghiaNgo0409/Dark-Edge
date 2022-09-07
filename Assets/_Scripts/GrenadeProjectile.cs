using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    Action onGrenadeComplete;
    Vector3 targetPosition;
    [SerializeField] float moveSpeed;
    [SerializeField] float damageRadius = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float reachedDistance = .2f;
        if(Vector3.Distance(targetPosition, transform.position) < reachedDistance)
        {
            Collider[] colliders = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent<Unit>(out Unit unit))
                {
                    unit.Damage();
                }
                
            }
            Destroy(gameObject);
            onGrenadeComplete();
        }
    }

    public void Setup(GridPosition targetPosition, Action onGrenadeComplete)
    {
        this.onGrenadeComplete = onGrenadeComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    }
}

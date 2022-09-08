using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded; 
    Action onGrenadeComplete;
    Vector3 targetPosition;
    [SerializeField] float moveSpeed;
    [SerializeField] float damageRadius = 4f;
    [SerializeField] Transform grenadeExplodeVfxPrefab;
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] AnimationCurve animationCurve;
    float totalDistance;
    Vector3 positionXZ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ, targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4f;
        float positionY = animationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedDistance = .2f;
        if(Vector3.Distance(targetPosition, positionXZ) < reachedDistance)
        {
            Collider[] colliders = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach(Collider collider in colliders)
            {
                if(collider.TryGetComponent<Unit>(out Unit unit))
                {
                    unit.Damage();
                }

                if(collider.TryGetComponent<DestructibleCrate>(out DestructibleCrate crate))
                {
                    crate.Damage();
                }
                
            }
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            onGrenadeComplete();
        }
    }

    public void Setup(GridPosition targetPosition, Action onGrenadeComplete)
    {
        this.onGrenadeComplete = onGrenadeComplete;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, this.targetPosition);
    }
}

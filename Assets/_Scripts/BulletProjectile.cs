using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] TrailRenderer trailRenderer;
    [SerializeField] ParticleSystem bulletHitVfx;
    Vector3 targetPosition;
    [SerializeField] float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if(distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;

            trailRenderer.transform.parent = null;

            Destroy(gameObject);

            Instantiate(bulletHitVfx, targetPosition, Quaternion.identity);
        }

    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyCrateDestroyed;
    GridPosition gridPosition;
    [SerializeField] Transform destructibleCratePrefab;
    // Start is called before the first frame update
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage()
    {
        Transform crateTransform = Instantiate(destructibleCratePrefab, transform.position, Quaternion.identity);
        ApplyExplosionToCrate(crateTransform, 300f, transform.position, 20f);
        OnAnyCrateDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    void ApplyExplosionToCrate(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);

            }

            ApplyExplosionToCrate(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}

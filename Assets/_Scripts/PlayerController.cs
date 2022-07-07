using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 targetPosition;
    float stoppingDistance = .1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 desiredDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += desiredDirection * moveSpeed * Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector3(4, 0, 4));
        }
    }

    void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}

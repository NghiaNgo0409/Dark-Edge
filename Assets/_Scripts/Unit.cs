using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Vector3 targetPosition;
    float stoppingDistance = .1f;
    float rotateSpeed = 10f;
    Animator unitAnim;
    // Start is called before the first frame update
    void Start()
    {
        unitAnim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            unitAnim.SetBool("isRunning", true);
            Vector3 desiredDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 10f;
            transform.position += desiredDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, desiredDirection, rotateSpeed * Time.deltaTime);
        }
        else
        {
            unitAnim.SetBool("isRunning", false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            Move(MousePosition.GetPosition());
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            unitAnim.SetTrigger("Shoot");
        }
    }

    void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}

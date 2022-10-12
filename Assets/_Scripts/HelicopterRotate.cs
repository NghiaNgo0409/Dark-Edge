using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterRotate : MonoBehaviour
{
    [SerializeField] GameObject helicopterFan;
    [SerializeField] GameObject helicopterTail;
    [SerializeField] float rotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        helicopterFan.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        helicopterTail.transform.Rotate(rotateSpeed * Time.deltaTime, 0, 0);
    }
}

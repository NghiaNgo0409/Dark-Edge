using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform;
    [SerializeField] bool invert;
    void Awake()
    {
        cameraTransform = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if(invert)
        {
            Vector3 dirToCam = (cameraTransform.position - transform.position).normalized;
            transform.LookAt(transform.position + dirToCam * -1);
        }
        else
        {
            transform.LookAt(cameraTransform);
        }
    }
}

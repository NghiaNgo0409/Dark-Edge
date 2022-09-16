using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] Transform objTransform; 
    float delay = 0; 
    float when = 1.0f; 
    Vector3 offset;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        offset = new Vector3(Random.Range(-4, 4), offset.y, offset.z);
        offset = new Vector3(offset.x, Random.Range(0, 3), offset.z); 
        offset = new Vector3(offset.x, offset.y, Random.Range(2, 6)); 
    }

    // Update is called once per frame
    void Update()
    {
        if(when >= delay) 
        { 
            objTransform.position += offset * Time.deltaTime; 
            delay += Time.deltaTime; 
        }
        else 
        { 
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }
}

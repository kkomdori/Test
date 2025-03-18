using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        // Vector3.forward 는 z축, transform.forward 는 오브젝트 기준 정면
        rb.velocity = transform.forward * speed;
    }
}

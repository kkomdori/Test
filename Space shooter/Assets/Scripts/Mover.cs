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
        
        // Vector3.forward �� z��, transform.forward �� ������Ʈ ���� ����
        rb.velocity = transform.forward * speed;
    }
}

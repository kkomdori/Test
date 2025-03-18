using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour
{
    public float tumble;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere * tumble; 
        
        // 원점과 구체 내부의 임의의 한 점을 잇는 벡터 생성. 
        //Debug.Log(Random.insideUnitSphere); 
    }
}

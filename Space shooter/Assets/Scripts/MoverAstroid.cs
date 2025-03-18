using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverAstroid : MonoBehaviour
{
    public static float speed;
    public int speedSeed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * -speed * speedSeed;
    }
}

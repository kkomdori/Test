using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float lifeTime;

    //private float timeCount;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // ���� �ð� �� ����. Time.deltaTime ������� ȿ����.
    }

    private void Update()
    {
        //timeCount += Time.deltaTime;
        //if (timeCount >= lifeTime)
        //    Destroy(gameObject);
    }
}

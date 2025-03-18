using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float lifeTime;

    //private float timeCount;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // 일정 시간 후 제거. Time.deltaTime 방법보다 효율적.
    }

    private void Update()
    {
        //timeCount += Time.deltaTime;
        //if (timeCount >= lifeTime)
        //    Destroy(gameObject);
    }
}

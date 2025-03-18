using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float xMin, xMax, zMin, zMax;
    public float tilt;

    public Rigidbody rb;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;
    private AudioSource audioSource;

    private bool isAutoFire = true;
    public TMP_Text autoFire;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        autoFire.text = "Auto-firing mode (Space bar) : " + isAutoFire;
        StartCoroutine(AutoFire());
    }

    private void Update()
    {
        bool push = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAutoFire = isAutoFire == true ? false : true;
            push = isAutoFire == true ? true : false;
            autoFire.text = "Auto-firing mode (Space bar) : " + isAutoFire;
        }

        if (!isAutoFire)
            Fire();
        else if (push) // isAutoFire 가 false 에서 true로 바뀔 때 한번만 실행
            StartCoroutine(AutoFire()); // 자동발사는 코루틴에서
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //  화살표로 제어하기
        //float moveHorizontal = 0;
        //float moveVertical = 0;

        //if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        //    moveHorizontal = Input.GetAxis("Horizontal");
        //else
        //    moveHorizontal = 0;

        //if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        //    moveVertical = Input.GetAxis("Vertical");
        //else
        //    moveVertical = 0;


        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.velocity = movement.normalized * speed; // 속도 직접 제어
        //rb.AddForce(movement.normalized * speed);

        // rb에 운동을 가하는 중 직접 위치 수정할 경우 transform.position 보다 rb.position 이 더 정확하다.
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, xMin, xMax), 0, Mathf.Clamp(rb.position.z, zMin, zMax));

        // 짐벌락 문제 해결. 오일러 값을 받아서 quaternion 값으로 변환해주는 함수
        rb.rotation = Quaternion.Euler(0, 0, rb.velocity.x * -tilt);
    }

    private void Fire()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire) // Time.time 게임 시작 후 흐른 시간. 초단위
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
    }

    IEnumerator AutoFire()
    {
        while (isAutoFire)
        {
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();

            yield return new WaitForSeconds(fireRate);
        } // isAutoFire == false 이면 코루틴 종료
    }
}

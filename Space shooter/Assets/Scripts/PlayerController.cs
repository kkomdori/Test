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
        else if (push) // isAutoFire �� false ���� true�� �ٲ� �� �ѹ��� ����
            StartCoroutine(AutoFire()); // �ڵ��߻�� �ڷ�ƾ����
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //  ȭ��ǥ�� �����ϱ�
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
        rb.velocity = movement.normalized * speed; // �ӵ� ���� ����
        //rb.AddForce(movement.normalized * speed);

        // rb�� ��� ���ϴ� �� ���� ��ġ ������ ��� transform.position ���� rb.position �� �� ��Ȯ�ϴ�.
        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, xMin, xMax), 0, Mathf.Clamp(rb.position.z, zMin, zMax));

        // ������ ���� �ذ�. ���Ϸ� ���� �޾Ƽ� quaternion ������ ��ȯ���ִ� �Լ�
        rb.rotation = Quaternion.Euler(0, 0, rb.velocity.x * -tilt);
    }

    private void Fire()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire) // Time.time ���� ���� �� �帥 �ð�. �ʴ���
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
        } // isAutoFire == false �̸� �ڷ�ƾ ����
    }
}

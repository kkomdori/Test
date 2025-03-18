using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    private GameController gameController; // Score �����ϴ� �Լ� �ҷ����� ����. �׷��� �̻��� prefab�� ����Ƽ���� �����ų �� ����

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        // üũ���ָ� ����
        //GameObject gameCtrlobj = GameObject.FindWithTag("GameController");
        //if (gameCtrlobj != null)
        //{
        //    gameController = gameCtrlobj.GetComponent<GameController>();

        //    if (gameController == null)
        //        Debug.Log("Cannot find 'GameController' script"); // gameObject �� ���ų� script�� ���� �Ǿ����� ���� ��
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Astroid") //other.CompareTag() �� ��밡��. �Լ� ���� ���� �� �� ��õ
            return;

        Instantiate(explosion, transform.position, transform.rotation);
        
        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
        
        if (other.tag == "Bolt")
            gameController.AddScore(scoreValue); // ���ھ� ���ϱ�
        
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}

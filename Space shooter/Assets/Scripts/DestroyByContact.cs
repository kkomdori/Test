using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    private GameController gameController; // Score 갱신하는 함수 불러오기 위해. 그러나 미생성 prefab은 유니티에서 연결시킬 수 없음

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();

        // 체크해주면 좋음
        //GameObject gameCtrlobj = GameObject.FindWithTag("GameController");
        //if (gameCtrlobj != null)
        //{
        //    gameController = gameCtrlobj.GetComponent<GameController>();

        //    if (gameController == null)
        //        Debug.Log("Cannot find 'GameController' script"); // gameObject 가 없거나 script가 연결 되어있지 않을 때
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Astroid") //other.CompareTag() 도 사용가능. 함수 쓰는 것을 좀 더 추천
            return;

        Instantiate(explosion, transform.position, transform.rotation);
        
        if (other.tag == "Player")
        {
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.GameOver();
        }
        
        if (other.tag == "Bolt")
            gameController.AddScore(scoreValue); // 스코어 더하기
        
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}

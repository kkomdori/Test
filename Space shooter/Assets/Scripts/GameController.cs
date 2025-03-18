using TMPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
//using System;

public class GameController : MonoBehaviour
{
    public float timeLimit;
    public TMP_Text timeLimitText;
    public int score;
    public TMP_Text scoreText;
    private int[] highScores = new int[6]; // Score 5개 출력하는 경우 마지막 하나를 신규 입력값 버퍼로 사용
    const string saveName = "Score ";
    public TMP_Text highScoreText;

    public TMP_Text restartText;
    public TMP_Text gameOverText;
    private bool gameOver;
    private bool restart;

    public GameObject[] hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public int wave;
    public TMP_Text waveText;
    public int difficultyManitude;

    public GameObject restartBtn; // 버튼 활성화/비활성화
    public GameObject joistick;

    private void Start()
    {
        Application.targetFrameRate = 60;

        gameOver = false; // gameover 후 일정시간 후 restart
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        waveText.text = "Wave : " + wave;
        highScoreText.text = "";

        score = 0;
        UpdateScore();
        LoadHighScore();
        StartCoroutine(SpawnWaves()); // 코루틴 함수의 호출
    }

    private void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();
        }

        //Timer(); // timeLimit == 0 일 때, 일시정지
        SetLevel();
        UpdateWaveText();
        //Debug.Log("wave : " + wave);

    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    void SaveHighScore()
    {
        int last = highScores.Length - 1;
        highScores[last] = score;
        System.Array.Sort(highScores);
        System.Array.Reverse(highScores);

        for (int i = 0; i < highScores.Length - 1; i++)
        {
            string str = MakeKey(i);
            PlayerPrefs.SetInt(str, highScores[i]); // PlayerPrefs 클래스를 활용해 Int (value) 값을 str (key) 변수에 저장한다.
        }

        PlayerPrefs.Save(); // 디스크에 저장하기
    }

    void LoadHighScore()
    {
        for (int i = 0; i < highScores.Length - 1; i++)
        {
            string str = MakeKey(i);
            highScores[i] = PlayerPrefs.GetInt(str); // 디스크에서 직접 읽어옴
        }
    }

    void ShowHighScore()
    {
        string str = "High Score\n";
        for (int i = 0; i < highScores.Length - 1; i++)
        {
            str += string.Format("{0}. {1}\n", i + 1, highScores[i]);
        }
        highScoreText.text = str; 
    }

    private string MakeKey(int num)
    {
        string key = string.Format("{0}{1}", saveName, num);
        return key;
    }


    private void UpdateWaveText()
    {
        waveText.text = "Wave : " + wave;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
        joistick.SetActive(false);
        restartBtn.SetActive(true); // 버튼 다시 보이게
    }

    private void UpdateScore()
    {
        scoreText.text = "Score : " + score;
    }

    public void Timer()
    {
        // 텍스트 설정하는 함수
        timeLimit -= Time.deltaTime;
        if (timeLimit <= 0)
        {
            Time.timeScale = 0; // 일시정지
            timeLimit = 0;
        }
        timeLimitText.text = $"Time Limit : {timeLimit:0.00} (s)"; // 숫자 포멧
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void SetLevel()
    {
        MoverAstroid.speed = difficultyManitude * wave; // 없는 오브젝트로 받을 수 없어서 클래스 변수로 접근
    }

    IEnumerator SpawnWaves() // 코루틴 함수. yield return 필수
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                // 소행성 생성 영역 
                Vector3 spawnPosition = new Vector3(
                    Random.Range(-spawnValues.x, spawnValues.x), 
                    spawnValues.y, 
                    spawnValues.z);

                //Quaternion spawnRotation = Quaternion.identity;
                Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(-15, 15), 0); // 회전축 랜덤 > Mover.cs 에서 축 방향으로 발사 처리 
                Instantiate(hazard[Random.Range(0, 3)], spawnPosition, spawnRotation);

                // 비동기 작업에서 쉬는 부분 있어야 다른 작업도 수행 가능하므로 yield return 필수 
                yield return new WaitForSeconds(spawnWait);

                if (gameOver)
                    break;
            }
            wave++;
            AddScore(100);
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "Press 'R' for Restart";
                restart = true;
                break; // 반복문마다
            }
        }

        SaveHighScore();
        ShowHighScore();
    }
}

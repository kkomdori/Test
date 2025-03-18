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
    private int[] highScores = new int[6]; // Score 5�� ����ϴ� ��� ������ �ϳ��� �ű� �Է°� ���۷� ���
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

    public GameObject restartBtn; // ��ư Ȱ��ȭ/��Ȱ��ȭ
    public GameObject joistick;

    private void Start()
    {
        Application.targetFrameRate = 60;

        gameOver = false; // gameover �� �����ð� �� restart
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        waveText.text = "Wave : " + wave;
        highScoreText.text = "";

        score = 0;
        UpdateScore();
        LoadHighScore();
        StartCoroutine(SpawnWaves()); // �ڷ�ƾ �Լ��� ȣ��
    }

    private void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();
        }

        //Timer(); // timeLimit == 0 �� ��, �Ͻ�����
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
            PlayerPrefs.SetInt(str, highScores[i]); // PlayerPrefs Ŭ������ Ȱ���� Int (value) ���� str (key) ������ �����Ѵ�.
        }

        PlayerPrefs.Save(); // ��ũ�� �����ϱ�
    }

    void LoadHighScore()
    {
        for (int i = 0; i < highScores.Length - 1; i++)
        {
            string str = MakeKey(i);
            highScores[i] = PlayerPrefs.GetInt(str); // ��ũ���� ���� �о��
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
        restartBtn.SetActive(true); // ��ư �ٽ� ���̰�
    }

    private void UpdateScore()
    {
        scoreText.text = "Score : " + score;
    }

    public void Timer()
    {
        // �ؽ�Ʈ �����ϴ� �Լ�
        timeLimit -= Time.deltaTime;
        if (timeLimit <= 0)
        {
            Time.timeScale = 0; // �Ͻ�����
            timeLimit = 0;
        }
        timeLimitText.text = $"Time Limit : {timeLimit:0.00} (s)"; // ���� ����
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void SetLevel()
    {
        MoverAstroid.speed = difficultyManitude * wave; // ���� ������Ʈ�� ���� �� ��� Ŭ���� ������ ����
    }

    IEnumerator SpawnWaves() // �ڷ�ƾ �Լ�. yield return �ʼ�
    {
        yield return new WaitForSeconds(startWait);

        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                // ���༺ ���� ���� 
                Vector3 spawnPosition = new Vector3(
                    Random.Range(-spawnValues.x, spawnValues.x), 
                    spawnValues.y, 
                    spawnValues.z);

                //Quaternion spawnRotation = Quaternion.identity;
                Quaternion spawnRotation = Quaternion.Euler(0, Random.Range(-15, 15), 0); // ȸ���� ���� > Mover.cs ���� �� �������� �߻� ó�� 
                Instantiate(hazard[Random.Range(0, 3)], spawnPosition, spawnRotation);

                // �񵿱� �۾����� ���� �κ� �־�� �ٸ� �۾��� ���� �����ϹǷ� yield return �ʼ� 
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
                break; // �ݺ�������
            }
        }

        SaveHighScore();
        ShowHighScore();
    }
}

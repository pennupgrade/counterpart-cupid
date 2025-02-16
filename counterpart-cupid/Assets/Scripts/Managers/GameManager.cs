using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public static string lastSceneName = null;
    private bool isDead = false;
    private int score = 0;
    [SerializeField] private float timer;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text TimerText;


    void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        lastSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            return;
        }
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 0;
            Lose();
        }
        UpdateTimer();

    }

    void UpdateTimer() {
        TimeSpan t = TimeSpan.FromSeconds(timer);
        TimerText.text = t.ToString(@"mm\:ss\:ff");
        // TODO: UPDATE TIMER UI
    }

    void UpdateScore() {
        ScoreText.text = "Score: " + score;
        // TODO: UPDATE SCORE UI
    }

    void Lose() {
        isDead = true;
        SceneManager.LoadSceneAsync("GameOverScreen");
    }

    public static void AddScore(int scoreAddition) {
        Instance.score += scoreAddition;
        Instance.UpdateScore();
    }

    public static int GetScore() {
        return Instance.score;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public static string lastSceneName = null;
    private bool isDead = false;
    private int score = 0;
    [SerializeField] private float timer;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text TimerText;
    public GameObject NPCPrefab;
    public Vector3 fieldMinPos;
    public Vector3 fieldMaxPos;

    // -72.5, 72.5


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
        // assuming only increment 1 score at a time
        Instance.score += scoreAddition;
        Instance.UpdateScore();
        Instance.SpawnNPCs();
    }

    public static int GetScore() {
        return Instance.score;
    }

    void SpawnNPCs() {
        // spawn 1 kiddo
        Instantiate(NPCPrefab, RandomSpawnPos(), UnityEngine.Quaternion.identity);
        // spawn 1 more with 50% chance
        if (Random.Range(0,1) > 0.5f) {
            Instantiate(NPCPrefab, RandomSpawnPos(), UnityEngine.Quaternion.identity);
        }
    }

    Vector3 RandomSpawnPos() {
        Vector3 randomPosition = new(
            Random.Range(fieldMinPos.x, fieldMaxPos.x),
            Random.Range(fieldMinPos.y, fieldMaxPos.y),
            Random.Range(fieldMinPos.z, fieldMaxPos.z)
        );
        return randomPosition;
    }
}

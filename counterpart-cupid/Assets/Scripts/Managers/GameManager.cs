using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null; 

    private int score = 0;
    [SerializeField] private float timer;

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            timer = 0;
            Lose();
        }
        UpdateTimer();

    }

    void UpdateTimer() {
        // TODO: UPDATE TIMER UI
    }

    void UpdateScore() {
        // TODO: UPDATE SCORE UI
    }

    void Lose() {
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

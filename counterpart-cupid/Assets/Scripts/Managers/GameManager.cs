using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null; 

    private int score = 0;
    [SerializeField] private float timer;

    void Awake() {
        Instance = this;
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
        print(timer);
        // TODO: UPDATE TIMER UI
    }

    void UpdateScore() {
        print(score);
        // TODO: UPDATE SCORE UI
    }

    void Lose() {
        // GAME OVER
    }

    public static void AddScore(int scoreAddition) {
        Instance.score += scoreAddition;
        Instance.UpdateScore();
    }
}

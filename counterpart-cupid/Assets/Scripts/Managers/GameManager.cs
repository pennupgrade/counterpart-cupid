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

    // scoring
    private int score = 0;
    [SerializeField] private float timer;
    [SerializeField] private TMP_Text ScoreText;
    [SerializeField] private TMP_Text TimerText;

    [Header("Spawning & Room Info")]
    public NPC_Character NPCPrefab;
    public Vector3 fieldMinPos;
    public Vector3 fieldMaxPos;

    // tracking seen attributes
    private List<NPC_Character> activeCharacters = new List<NPC_Character>(); // Track NPCs
    private Dictionary<int, List<NPC_Character>> attributeGroups = new Dictionary<int, List<NPC_Character>>();
    private Dictionary<int, List<Sprite>> attributeSymbols = new Dictionary<int, List<Sprite>>();
    [SerializeField] private Sprite[] rawSymbols;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // populate attributeSymbols
        for (int i = 0; i < rawSymbols.Length - 1; i += 2)
        {
            attributeSymbols.Add(i / 2, new List<Sprite> { rawSymbols[i], rawSymbols[i + 1] });
        }

        // populate attriButeGroups
        for (int i = 0; i < attributeSymbols.Count; i++)
        {
            attributeGroups.Add(i, new List<NPC_Character>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastSceneName = SceneManager.GetActiveScene().name;
        SpawnNewCharacters();
        SpawnNewCharacters();
        SpawnNewCharacters();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            Lose();
        }
        UpdateTimer();

    }

    void UpdateTimer()
    {
        TimeSpan t = TimeSpan.FromSeconds(timer);
        TimerText.text = t.ToString(@"mm\:ss\:ff");
        // TODO: UPDATE TIMER UI
    }

    void UpdateScore()
    {
        ScoreText.text = "Score: " + score;
        // TODO: UPDATE SCORE UI
    }

    void Lose()
    {
        isDead = true;
        SceneManager.LoadSceneAsync("GameOverScreen");
    }

    public static void AddScore(int scoreAddition)
    {
        // assuming only increment 1 score at a time
        Instance.score += scoreAddition;
        Instance.UpdateScore();
        Instance.SpawnNewCharacters();
    }

    public static int GetScore()
    {
        return Instance.score;
    }

    void SpawnNewCharacters()
    {
        // spawn 1 kiddo
        (int, int) set = GetExistingOrNewShapeSet();
        SpawnNPC(RandomSpawnPos(), set.Item1, set.Item2, attributeSymbols[set.Item1][set.Item2]);
        // spawn 1 more with 50% chance
        if (Random.value > 0.5f)
        {
            set = GetExistingOrNewShapeSet();
            SpawnNPC(RandomSpawnPos(), set.Item1, set.Item2, attributeSymbols[set.Item1][set.Item2]);
        }
    }

    private (int, int) GetExistingOrNewShapeSet()
    {
        // Check if any shapeSet exists with only one character (incomplete pair)
        Dictionary<int, int> shapeCounts = new Dictionary<int, int>();

        foreach (NPC_Character npc in activeCharacters)
        {
            if (!shapeCounts.ContainsKey(npc.shapeSet))
                shapeCounts[npc.shapeSet] = 0;

            shapeCounts[npc.shapeSet]++;
        }
        print(string.Join(Environment.NewLine, shapeCounts));

        foreach (var pair in shapeCounts)
        {
            if (pair.Value == 1) // Found an incomplete pair
                return (pair.Key, 1);
        }

        return (Random.Range(0, attributeSymbols.Count), 0);
    }

    void SpawnNPC(Vector3 position, int shapeSet, int setElement, Sprite shapeSprite)
    {
        NPC_Character npcObject = Instantiate(NPCPrefab, position, UnityEngine.Quaternion.identity);
        NPC_Character npc = npcObject.GetComponent<NPC_Character>();

        if (npc != null)
        {
            print("spawned npc: pair #" + shapeSet + ", #" + setElement);
            npc.Initialize(shapeSet, setElement, shapeSprite);
            activeCharacters.Add(npc);
        }
    }

    Vector3 RandomSpawnPos()
    {
        Vector3 randomPosition = new(
            Random.Range(fieldMinPos.x, fieldMaxPos.x),
            Random.Range(fieldMinPos.y, fieldMaxPos.y),
            Random.Range(fieldMinPos.z, fieldMaxPos.z)
        );
        return randomPosition;
    }
}

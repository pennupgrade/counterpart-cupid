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
    [SerializeField] private GameObject[] NPCPrefabs;
    [SerializeField] private int maxCapacity = 16;
    public Vector3 fieldMinPos;
    public Vector3 fieldMaxPos;

    // tracking seen attributes
    private List<NPC_Character> activeCharacters = new List<NPC_Character>(); // Track NPCs
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
        Instance.timer += (100f / Instance.timer);
        Instance.SpawnNewCharacters();
    }

    public static int GetScore()
    {
        return Instance.score;
    }

    void SpawnNewCharacters()
    {
        // remove inactive ones
        for (int i = activeCharacters.Count - 1; i >= 0; i--)
        {
            if (!activeCharacters[i])
                activeCharacters.RemoveAt(i); // Remove the GameObject from the list
        }

        if (activeCharacters.Count >= maxCapacity) {
            return;
        }

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
            // add (shapeset, curr existing element) into dict
            if (!shapeCounts.ContainsKey(npc.shapeSet))
                shapeCounts[npc.shapeSet] = npc.setElement;
            else
            {
                // exists a complement for that shapeset
                if (npc.setElement != shapeCounts[npc.shapeSet])
                    shapeCounts.Remove(npc.shapeSet);
            }
        }
        // print(string.Join(Environment.NewLine, shapeCounts));

        foreach (var single in shapeCounts)
        {
            return (single.Key, shapeCounts[single.Key] == 1 ? 0 : 1);
        }

        return (Random.value > 0.5f) ? (Random.Range(0, attributeSymbols.Count), 0) : (Random.Range(0, attributeSymbols.Count), 1);
    }

    void SpawnNPC(Vector3 position, int shapeSet, int setElement, Sprite shapeSprite)
    {
        GameObject npcObject = Instantiate(NPCPrefabs[Random.Range(0, NPCPrefabs.Length)], position, UnityEngine.Quaternion.identity);
        NPC_Character npc = npcObject.GetComponent<NPC_Character>();

        if (npc != null)
        {
            // print("spawned npc: pair #" + shapeSet + ", #" + setElement);
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

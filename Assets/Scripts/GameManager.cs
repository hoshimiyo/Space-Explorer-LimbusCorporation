using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton pattern
    public int score = 0; // Player’s score
    public GameObject explosionPrefab;

    public ProgressBar progressBar;
    [SerializeField] private float survivalTime = 45f;
    private float elapsedTime = 0f;
    void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene changes
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        if (progressBar == null) // If not assigned in Inspector, find it
        {
            progressBar = FindObjectOfType<ProgressBar>();
            if (progressBar == null)
            {
                Debug.LogError("ProgressBar not found in the scene! Ensure it's added and assigned.");
            }
        }

        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            progressBar.SetMaxValue(survivalTime);

            // Initialize the score display
            score = 0;
            UpdateScoreUI();
        }

        AudioManager.instance.ToggleMusic(true); // Play music
    }

    public void ResetPlayerStat()
    {
        score = 0;
    }
    public void AddScore(int value)
    {
        score += value; // Increase the score
        UpdateScoreUI(); // Update UI

        //// Check for score thresholds and load the appropriate scene
        //CheckScoreThresholds();
    }

    public int GetScore()
    {
        return score;
    }

    private static void UpdateScoreUI()
    {
        ScoreUI scoreUI = FindFirstObjectByType<ScoreUI>();
        if (scoreUI != null)
        {
            scoreUI.UpdateScoreUI();
        }
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateScoreUI(); // Make sure the UI updates when switching scenes
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            elapsedTime += Time.deltaTime;
            progressBar.SetValue(elapsedTime);

            if (elapsedTime >= survivalTime)
            {
                // Proceed to the next scene
                SceneManager.LoadScene("Scene2");
            }
        }
    }
    public void GameOver()
    {
        // Stop spaceship movement
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<ShipControl>().enabled = false;
        }

        // Delay before transitioning to End Game Scene
        Invoke("LoadEndGameScene", 2f);
        AudioManager.instance.ToggleMusic(false); // Stop music
    }

    public void LoadEndGameScene()
    {
        SceneManager.LoadScene("EndGame"); // Load End Game Scene
    }
}

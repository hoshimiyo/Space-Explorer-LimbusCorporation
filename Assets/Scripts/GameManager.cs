using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton pattern
    public int score = 0; // Player’s score
    public GameObject explosionPrefab;

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
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
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

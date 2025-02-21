using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // UI Text to display score
    public static GameManager instance; // Singleton pattern
    private int score = 0; // Player’s score
    // public GameObject pausePanel; // UI panel to show when the game pauses
    void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the score display
        UpdateScore(0);
        AudioManager.instance.ToggleMusic(true); // Play music
        // pausePanel.SetActive(false); // Hide game over panel at the start
    }

    public void AddScore(int value)
    {
        score += value; // Increase the score
        UpdateScore(score); // Update UI
    }

    void UpdateScore(int newScore)
    {
        scoreText.text = "Score: " + newScore;
    }

    public int GetScore()
    {
        return score;
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

    void LoadEndGameScene()
    {
        SceneManager.LoadScene("EndGame"); // Load End Game Scene
    }
}

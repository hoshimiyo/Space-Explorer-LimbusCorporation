using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{
    [SerializeField] HighScoreManager highScoreManager;
    [SerializeField] string playerName;
    public TextMeshProUGUI finalScoreText;
    void Start()
    {
        // Retrieve the final score from GameManager
        int finalScore = GameManager.instance.GetScore();
        finalScoreText.text = "Final Score: " + finalScore; // Display the final score
        highScoreManager.AddHighScore(new HighScoreElement(playerName, finalScore)); // Add the score to the high score list
        Destroy(GameManager.instance.gameObject); // Destroy the GameManager object 
    }

    public void RestartGame()
    {
        //PlayerStats.playerStat.ResetStats();
        // Reset stats before destroying the GameManager
        GameManager.instance.ResetPlayerStat();
        ShipStat.ResetStat();

        // Reload the gameplay scene
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Load main menu
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

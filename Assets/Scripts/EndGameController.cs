using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    [SerializeField] HighScoreManager highScoreManager;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private TextMeshProUGUI submitConfirm;
    private bool hasSubmitted = false; // Prevent multiple submissions
    private int finalScore;
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        // Retrieve the final score from GameManager
        finalScore = GameManager.instance.GetScore();
        finalScoreText.text = "Final Score: " + finalScore; // Display the final score
        Destroy(GameManager.instance.gameObject); // Destroy the GameManager object 
    }

    public void SubmitScore()
    {
        if (hasSubmitted) return; // Prevent multiple submissions

        hasSubmitted = true;

        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Player name is empty! Assigning default name.");
            playerName = "Player";
        }

        highScoreManager.AddHighScore(new HighScoreElement(playerName, finalScore));

        submitButton.interactable = false; // Disable the submit button
        submitConfirm.gameObject.SetActive(true); // Show the submission confirmation
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

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject highScorePanel;
    void Start()
    {
        if (instructionsPanel == null)
            return;
        instructionsPanel.SetActive(false);
    }

    public void GoToScene(string sceneName)
    {
        ShipStat.ResetStat();
        Debug.Log("play trigger");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void ShowInstructions()
    {
        if (instructionsPanel == null)
            return;
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        if (instructionsPanel == null)
            return;
        instructionsPanel.SetActive(false);
    }

    public void ShowHighScore()
    {
        if (highScorePanel == null)
            return;
        highScorePanel.SetActive(true);
    }

    public void HideHighScore()
    {
        if (highScorePanel == null)
            return;
        highScorePanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

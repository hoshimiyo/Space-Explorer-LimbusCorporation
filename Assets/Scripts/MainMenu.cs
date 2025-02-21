using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsPanel;
    void Start()
    {
        instructionsPanel.SetActive(false);
    }
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

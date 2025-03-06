using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private float delayBeforeTransition; // Time before switching scene
    [SerializeField] GameObject transitionPanel; // Reference to the transition panel

    void Start()
    {
        if (GameManager.instance != null)
        {
            delayBeforeTransition = GameManager.instance.getSurvivalTime(); //  Get the survival time from GameManager
            transitionPanel.SetActive(false); //  Hide the transition panel
            Invoke(nameof(PrepareTransition), delayBeforeTransition);
            return;
        }

        Debug.Log("GameManager not found!");
    }

    void PrepareTransition()
    {
        AsteroidSpawner.stopSpawner = false; //  Disable spawner before transitioning
        StarScript.stopSpawner = false; //  Disable spawner before transitioning
        PowerUpSpawner.stopSpawner = false; //  Disable spawner before transitioning
        LoadNextScene();
    }

    void LoadNextScene()
    {
        transitionPanel.SetActive(true); //  Show the transition panel

        Time.timeScale = 0; //  Pause the game
    }
}

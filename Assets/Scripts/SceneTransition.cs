using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private int scoreThreshold = 1000;  // The score threshold required to transition to the next scene
    [SerializeField] private string nextSceneName;       // The name of the next scene to load
    private bool transitionStarted = false;  // To ensure the transition only happens once

    void Update()
    {
        // Check if the score reaches the threshold
        if (GameManager.instance.GetScore() >= scoreThreshold && !transitionStarted)
        {
            transitionStarted = true;  // Set the flag to true to prevent the transition from happening multiple times
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);  // Load the next scene
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] float delayBeforeTransition = 45f; // Time before switching scene
    [SerializeField] GameObject transitionPanel; // Reference to the transition panel

    void Start()
    {
        transitionPanel.SetActive(false); //  Hide the transition panel
        Invoke(nameof(PrepareTransition), delayBeforeTransition);
    }

    void PrepareTransition()
    {
        AsteroidSpawner.stopSpawner = false; //  Disable spawner before transitioning
        StarScript.stopSpawner = false; //  Disable spawner before transitioning
        PowerUpSpawner.stopSpawner = false; //  Disable spawner before transitioning
        Invoke(nameof(LoadNextScene), 5); 
    }

    void LoadNextScene()
    {    
        transitionPanel.SetActive(true); //  Show the transition panel
        
        Time.timeScale = 0; //  Pause the game
    }
}

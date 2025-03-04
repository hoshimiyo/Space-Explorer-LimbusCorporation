using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] float delayBeforeTransition = 45f; // Time before switching scene
    [SerializeField] private string nextSceneName;

    void Start()
    {
        Invoke("LoadNextScene", delayBeforeTransition);
    }

    void LoadNextScene()
    {    
        SceneManager.LoadScene(nextSceneName);
    }
}

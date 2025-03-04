using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] float delayBeforeTransition = 5f; // Time before switching scene
    public string nextSceneName; // Set this in the Inspector

    void Start()
    {
        Invoke("LoadNextScene", delayBeforeTransition);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

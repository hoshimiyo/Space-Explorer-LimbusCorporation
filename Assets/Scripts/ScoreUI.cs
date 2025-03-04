using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>(); // Find the UI Text in the scene
        UpdateScoreUI();
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.instance.GetScore();
        }
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.instance.GetScore();
        }
    }
}

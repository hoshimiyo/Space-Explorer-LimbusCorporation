using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lives;

    void Start()
    {
        UpdateScoreUI();

    }

    void Update()
    {
        LifeUI();
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.instance.GetScore();
        }
    }

    public void LifeUI()
    {
        if (lives != null)
        {
            if (ShipStat.health <= 0)
            {
                lives.text = "Lives: 0";
            }

            lives.text = "Lives: " + ShipStat.health;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] private GameObject highScorePanel; 
    [SerializeField] private GameObject highScoreElementPrefab; 
    [SerializeField] private Transform elementWrapper;

    private List<GameObject> highScoreElements = new List<GameObject>(); 

    void OnEnable()
    {
        HighScoreManager.onHighscoreListChanged += UpdateUI;
    }

    void OnDisable()
    {
        HighScoreManager.onHighscoreListChanged -= UpdateUI;
    }

    public void ShowPanel()
    {
        highScorePanel.SetActive(true);
    }

    public void HidePanel()
    {
        highScorePanel.SetActive(false);
    }   

    private void UpdateUI(List<HighScoreElement> highScores)
    {
        for (int i = 0; i < highScores.Count; i++)
        {
            HighScoreElement highScore = highScores[i];

            if (highScore.Score > 0)
            {
                if(i >= highScoreElements.Count)
                {
                    var inst = Instantiate(highScoreElementPrefab, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper, false);

                    highScoreElements.Add(inst);
                }
                
                var textComponents = highScoreElements[i].GetComponentsInChildren<TextMeshProUGUI>();
                textComponents[0].text = highScore.PlayerName;
                textComponents[1].text = highScore.Score.ToString(); 
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    List<HighScoreElement> highScores = new List<HighScoreElement>();
    [SerializeField] int maxHighScores = 5;
    [SerializeField] string filename;

    public delegate void OnHighscoreListChanged(List<HighScoreElement> list);
    public static event OnHighscoreListChanged onHighscoreListChanged;
    void Start()
    {
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        highScores = FileManager.ReadListFromJSON<HighScoreElement>(filename);

        while (highScores.Count > maxHighScores)
        {
            highScores.RemoveAt(maxHighScores);
        }

        if (onHighscoreListChanged != null)
        {
            onHighscoreListChanged.Invoke(highScores);
        }
    }

    public void SaveHighScores()
    {
        FileManager.SaveToJSON(highScores, filename);
    }

    public void AddHighScore(HighScoreElement highScore)
    {
        for (int i = 0; i < maxHighScores; i++)
        {
            if (i >= highScores.Count || highScore.Score > highScores[i].Score)
            {
                highScores.Insert(i, highScore);

                while (highScores.Count > maxHighScores)
                {
                    highScores.RemoveAt(maxHighScores);
                }

                SaveHighScores();

                if (onHighscoreListChanged != null)
                {
                    onHighscoreListChanged.Invoke(highScores);
                }
                
                break;
            }
        }
    }
}

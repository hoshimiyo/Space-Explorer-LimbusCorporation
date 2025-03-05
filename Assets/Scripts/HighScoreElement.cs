
using System;

[Serializable]
public class HighScoreElement
{
    public string PlayerName;
    public int Score;

    public HighScoreElement(string playerName, int score)
    {
        PlayerName = playerName;
        Score = score;
    }
}

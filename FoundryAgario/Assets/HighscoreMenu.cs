using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreMenu : MonoBehaviour
{
    [SerializeField] private Text HighscoreList;
    
    /* Populate highscores on awake */
    void Awake()
    {
        HighscoreList.text = "";
        Debug.Log(PlayerPrefs.GetString("highscores"));
        if (!PlayerPrefs.HasKey("highscores")) return;
        List<string> allScores = new List<string>(PlayerPrefs.GetString("highscores").Substring(1).Split(','));
        List<int> allScoresInt = new List<int>();
        foreach (string scoreString in allScores)
        {
            allScoresInt.Add(Convert.ToInt32(scoreString));
        }
        allScoresInt.Sort();
        for (int i = 0; i < (allScoresInt.Count > 6 ? 6 : allScoresInt.Count); i++)
        {
            HighscoreList.text += "\n" + (i+1) + " - " + allScoresInt[i];
        }
    }
}

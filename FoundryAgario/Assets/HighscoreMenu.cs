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
        if (!PlayerPrefs.GetString("highscores").Contains(",")) return;
        List<int> allScores = new List<int>(Convert.ToInt32(PlayerPrefs.GetString("highscores").Split(',')));
        allScores.Sort();
        for (int i = 0; i < (allScores.Count > 6 ? 6 : allScores.Count); i++)
        {
            HighscoreList.text += "\n" + (i+1) + " - " + allScores[i];
        }
    }
}

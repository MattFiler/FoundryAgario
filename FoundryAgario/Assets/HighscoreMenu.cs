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
        List<int> allScores = new List<int>(Convert.ToInt32(PlayerPrefs.GetString("highscores").Split(',')));
        allScores.Sort();
        for (int i = 0; i < allScores.Count; i++)
        {
            HighscoreList.text += "\n" + i + " - " + allScores[i];
        }
    }
}

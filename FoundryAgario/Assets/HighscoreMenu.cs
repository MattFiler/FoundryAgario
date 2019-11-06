using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighscoreMenu : MonoBehaviour
{
    [SerializeField] private Text HighscoreList;
    [SerializeField] private GameObject YouGotHighscore;

    /* Populate highscores on awake */
    void Awake()
    {
        //Show highscore list if they're available
        HighscoreList.text = "";
        if (!PlayerPrefs.HasKey("highscores")) return;
        List<string> allScores = new List<string>(PlayerPrefs.GetString("highscores").Substring(1).Split(','));
        List<int> allScoresInt = new List<int>();
        foreach (string scoreString in allScores)
        {
            allScoresInt.Add(Convert.ToInt32(scoreString));
        }
        allScoresInt.Sort();
        allScoresInt.Reverse();
        for (int i = 0; i < (allScoresInt.Count > 6 ? 6 : allScoresInt.Count); i++)
        {
            HighscoreList.text += "\n" + (i+1) + " - " + allScoresInt[i];
        }

        //If the latest score is at the top of the list, we got a new highscore
        if (!PlayerPrefs.HasKey("latest_score")) return;
        if (Convert.ToInt32(PlayerPrefs.GetString("latest_score")) != allScoresInt[0]) return;
        YouGotHighscore.SetActive(true);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

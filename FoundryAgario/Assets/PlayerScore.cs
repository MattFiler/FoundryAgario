using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoSingleton<PlayerScore>
{
    [SerializeField] private Text ScoreUI;
    public int Score = 0; 

    private void Update()
    {
        string ScoreAsMoney = Score.ToString("C");
        ScoreUI.text = ScoreAsMoney.Substring(0, ScoreAsMoney.Length - 3);
    }
}

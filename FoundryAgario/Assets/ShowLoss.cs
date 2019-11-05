using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowLoss : MonoSingleton<ShowLoss>
{
    public bool ShouldShowLoss = false;
    private float TimeSinceShown = 0.0f;
    private bool DidTriggerSceneChange = false;
    
    void Update()
    {
        if (ShouldShowLoss)
        {
            GetComponent<Animator>().SetBool("showloss", true);
            TimeSinceShown += Time.deltaTime;

            if (TimeSinceShown >= 5.0f && !DidTriggerSceneChange)
            {
                //GameOver
                PlayerPrefs.SetString("highscores", PlayerPrefs.GetString("highscores") + "," + PlayerScore.Instance.Score.ToString());
                PlayerPrefs.Save();
                //TODO: SHOW POPUP HERE
                SceneManager.LoadScene("Highscores");

                DidTriggerSceneChange = true;
            }
        }
    }
}

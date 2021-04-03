using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class End : MonoBehaviour
{
    public TextMeshProUGUI scoreBox;
    public TextMeshProUGUI hiScore;
    public VideoPlayer youKnowTheRules;

    // Start is called before the first frame update
    void Start()
    {
        float score = (PlayerPrefs.GetFloat("Score") * 10) / PlayerPrefs.GetInt("Levels");
        scoreBox.text = "Final Score: " + score.ToString();
        if (score > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", score);
            PlayerPrefs.Save();
        }
        hiScore.text = "High Score: " + PlayerPrefs.GetFloat("HighScore").ToString();

        youKnowTheRules.loopPointReached += Quit;
    }
    void Quit(VideoPlayer vp)
    {
        Application.Quit();
        Debug.LogError("Application Quit");
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void SayGoodbye()
    {
        youKnowTheRules.Play();
        this.gameObject.SetActive(false);
    }
}

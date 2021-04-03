using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float speed = 1;
    public float volume = .75f;
    public GameObject panel;
    public GameObject endButton;

    private void Start()
    {
        panel.SetActive(false);
        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            endButton.SetActive(false);
        }
    }
    public void Play()
    {
        PlayerPrefs.SetFloat("Speed", speed);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Score", 0);
        PlayerPrefs.SetInt("Levels", 1);
        PlayerPrefs.Save();

        if(PlayerPrefs.GetInt("FirstTime") == 0)
        {
            panel.SetActive(true);
            PlayerPrefs.SetInt("FirstTime", 1);
            PlayerPrefs.Save();
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void GotoEnd()
    {
        SceneManager.LoadScene(2);
    }
}

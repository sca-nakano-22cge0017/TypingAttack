using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    Fade fade;

    bool isToMain = false;
    bool isToStart = false;
    bool isToResult = false;
    bool isExit = false;

    void Start()
    {
        fade = GameObject.FindObjectOfType<Fade>();
    }

    void Update()
    {
        if (fade.FadeOutEnd == true)
        {
            if(isToMain)
            {
                SceneManager.LoadScene("MainScene");
            }
            if(isToStart)
            {
                SceneManager.LoadScene("StartScene");
            }
            if(isToResult)
            {
                SceneManager.LoadScene("ResultScene");
            }
            if(isExit)
            {
                Application.Quit();
            }
        }
    }

    public void ToMain()
    {
        isToMain = true;
    }

    public void ToStart()
    {
        isToStart = true;
    }

    public void ToResult()
    {
        isToResult = true;
    }

    public void Exit()
    {
        isExit = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    Fade fade;

    bool isToMain = false;
    bool isToExplain = false;
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
                fade.FadeOutEnd = false;
            }
            if(isToExplain)
            {
                SceneManager.LoadScene("ExplainScene");
                fade.FadeOutEnd = false;
            }
            if(isToStart)
            {
                SceneManager.LoadScene("TitleScene");
                fade.FadeOutEnd = false;
            }
            if(isToResult)
            {
                SceneManager.LoadScene("ResultScene");
                fade.FadeOutEnd = false;
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

    public void ToExplain()
    {
        isToExplain = true;
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

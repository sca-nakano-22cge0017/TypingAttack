using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移制御
/// </summary>
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
        //フェードアウトが終わったら遷移
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

    /// <summary>
    /// メインゲームへ
    /// </summary>
    public void ToMain()
    {
        isToMain = true;
    }

    /// <summary>
    /// 説明画面へ
    /// </summary>
    public void ToExplain()
    {
        isToExplain = true;
    }

    /// <summary>
    /// タイトル画面へ
    /// </summary>
    public void ToStart()
    {
        isToStart = true;
    }

    /// <summary>
    /// リザルト画面へ
    /// </summary>
    public void ToResult()
    {
        isToResult = true;
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void Exit()
    {
        isExit = true;
    }
}

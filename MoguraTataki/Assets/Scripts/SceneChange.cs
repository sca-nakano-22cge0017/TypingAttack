using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �V�[���J�ڐ���
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
        //�t�F�[�h�A�E�g���I�������J��
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
    /// ���C���Q�[����
    /// </summary>
    public void ToMain()
    {
        isToMain = true;
    }

    /// <summary>
    /// ������ʂ�
    /// </summary>
    public void ToExplain()
    {
        isToExplain = true;
    }

    /// <summary>
    /// �^�C�g����ʂ�
    /// </summary>
    public void ToStart()
    {
        isToStart = true;
    }

    /// <summary>
    /// ���U���g��ʂ�
    /// </summary>
    public void ToResult()
    {
        isToResult = true;
    }

    /// <summary>
    /// �Q�[���I��
    /// </summary>
    public void Exit()
    {
        isExit = true;
    }
}

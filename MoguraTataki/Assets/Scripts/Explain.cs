using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 説明画面
/// </summary>
public class Explain : MonoBehaviour
{
    [SerializeField] Fade fade;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        //フェードイン
        fade.FadeIn();
    }

    void Update()
    {
        if (fade.FadeInEnd)
        {
            //スペースを押したら効果音再生＋フェードアウト
            if(Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.PlayOneShot(clip);
                fade.FadeOut();
            }
        }

        if(fade.FadeOutEnd)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

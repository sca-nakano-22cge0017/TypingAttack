using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �������
/// </summary>
public class Explain : MonoBehaviour
{
    [SerializeField] Fade fade;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        //�t�F�[�h�C��
        fade.FadeIn();
    }

    void Update()
    {
        if (fade.FadeInEnd)
        {
            //�X�y�[�X������������ʉ��Đ��{�t�F�[�h�A�E�g
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

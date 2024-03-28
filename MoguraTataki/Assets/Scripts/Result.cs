using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���U���g���
/// </summary>
public class Result : MonoBehaviour
{
    int score;
    int lastScore;
    int s = 0;

    [SerializeField] Text scoreText;
    [SerializeField] Text newScoreText;

    [SerializeField] Fade fade;

    void Start()
    {
        //�t�F�[�h�C��
        fade.FadeIn();

        newScoreText.enabled = false;
        
        //�O��̃X�R�A�擾
        lastScore = PlayerPrefs.GetInt("lastScore", 0);
        score = PlayerPrefs.GetInt("score");
    }

    void Update()
    {
        //�t�F�[�h���I�������
        if(fade.FadeInEnd)
        {
            if(s < score)
            {
                s++;
                
                //�X�y�[�X����������X�R�A�̃J�E���g�A�b�v���X�L�b�v����
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    s = score;
                }

                scoreText.text = s.ToString();
            }
            else if(s >= score)
            {
                //�O��̃X�R�A��荂��������
                if (lastScore < score)
                {
                    //�V�L�^�̕\��
                    newScoreText.enabled = true;

                    //�X�R�A�X�V
                    lastScore = score;
                    PlayerPrefs.SetInt("lastScore", lastScore);
                }
            }
        }
    }
}

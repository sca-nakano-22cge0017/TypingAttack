using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        fade.FadeIn();

        newScoreText.enabled = false;

        lastScore = PlayerPrefs.GetInt("lastScore", 0);
        score = PlayerPrefs.GetInt("score");
    }

    void Update()
    {
        if(fade.FadeInEnd)
        {
            if(s < score)
            {
                s++;
                
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    s = score;
                }

                scoreText.text = s.ToString();
            }
            else if(s >= score)
            {
                if (lastScore < score)
                {
                    newScoreText.enabled = true;
                }
                lastScore = score;
                PlayerPrefs.SetInt("lastScore", lastScore);
            }
        }
    }
}

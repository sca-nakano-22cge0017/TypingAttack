using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    int score;
    int lastScore;

    [SerializeField] Text scoreText;
    [SerializeField] Text newScoreText;

    void Start()
    {
        newScoreText.enabled = false;

        lastScore = PlayerPrefs.GetInt("lastScore", 0);
        score = PlayerPrefs.GetInt("score");
        scoreText.text = score.ToString();

        if(lastScore < score)
        {
            newScoreText.enabled = true;
        }
        else { newScoreText.enabled = false; }

        lastScore = score;
        PlayerPrefs.SetInt("lastScore", lastScore);
    }

    void Update()
    {
        
    }
}

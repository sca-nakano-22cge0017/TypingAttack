using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルト画面
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
        //フェードイン
        fade.FadeIn();

        newScoreText.enabled = false;
        
        //前回のスコア取得
        lastScore = PlayerPrefs.GetInt("lastScore", 0);
        score = PlayerPrefs.GetInt("score");
    }

    void Update()
    {
        //フェードが終わったら
        if(fade.FadeInEnd)
        {
            if(s < score)
            {
                s++;
                
                //スペースを押したらスコアのカウントアップをスキップする
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    s = score;
                }

                scoreText.text = s.ToString();
            }
            else if(s >= score)
            {
                //前回のスコアより高かったら
                if (lastScore < score)
                {
                    //新記録の表示
                    newScoreText.enabled = true;

                    //スコア更新
                    lastScore = score;
                    PlayerPrefs.SetInt("lastScore", lastScore);
                }
            }
        }
    }
}

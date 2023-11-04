using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Text textRoma;
    [SerializeField] Text textKanji;
    [SerializeField] Text countDownText;

    [SerializeField] Text scoreText;
    int _score;

    [SerializeField] Image time;
    [SerializeField] float limitTime;
    float _time;

    [SerializeField] Image[] hp;
    [SerializeField] int defaultHp;
    int _hp;

    [SerializeField] float magnificationRate;
    [SerializeField] Sprite[] enemySprite;
    [SerializeField] Image[] key;
    List<Vector3> pos = new();
    Vector3 particlePos;

    [SerializeField] ParticleSystem starEffect;

    public string[] typingText;
    public string[] typingTextRoma;
    public string[] typingTextKanji;
    int textNum;

    public string[] typingText2;
    public string[] typingTextRoma2;
    int textNum2;

    List<TextData> Texts = new();
    CSVLoader csvLoader;

    [SerializeField] float levelBounus;
    int level = 1;
    int count = 1;
    [SerializeField] int levelUpNum;

    char nowKey;
    int nowKeyNum;
    char nextKey;
    int nextKeyNum;
    Color nextKeyColor = new Color(0.3207547f, 0.3207547f, 0.3207547f, 1f);
    //Color nowKeyColor = new Color(0.4377358f, 0.7621055f, 1f, 1f);
    Color nowKeyColor = new Color(1f, 1f, 1f, 1f);
    char lateKey;

    enum STATE { WAIT = 0, PLAY, GAMEOVER, };
    STATE state = 0;

    void Start()
    {
        state = STATE.WAIT;

        _score = 0;
        time.fillAmount = 1;
        _hp = defaultHp;
        for(int h = 0; h < _hp; h++)
        {
            hp[h].enabled = true;
        }

        textNum = 0;

        text.text = "";
        textRoma.text = "";
        textKanji.text = "";

        for (int i = 0; i < key.Length; i++)
        {
            key[i].sprite = enemySprite[0];
            key[i].enabled = false;
        }

        Texts = csvLoader.texts;

        StartCoroutine(CountDown());
    }

    void Update()
    {
        if(state == STATE.PLAY)
        {
            //制限時間
            if(_time >= 0)
            {
                _time -= Time.deltaTime;
                time.fillAmount = _time / limitTime;
                time.color = new Color(1, _time / limitTime, _time / limitTime);

                if(_time <= 0)
                {
                    _hp--;
                    if(_hp > 0)
                    {
                        TextChange();
                    }
                }
            }

            if (Input.anyKeyDown)
            {
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        lateKey = code.ToString()[0];
                        if (code.ToString() == nowKey.ToString())
                        {
                            KeyToImage(nowKey).enabled = false;
                            KeyToImage(nextKey).enabled = false;
                            starEffect.Play();
                            KeyToImage(nextKey).color = new Color(1, 1, 1);

                            if (nextKeyNum < typingTextRoma[textNum].Length - 1)
                            {
                                nextKeyNum++;
                                nextKey = typingTextRoma[textNum][nextKeyNum];
                            }
                            if(nowKeyNum < typingTextRoma[textNum].Length - 1)
                            {
                                nowKeyNum++;
                                nowKey = typingTextRoma[textNum][nowKeyNum];
                            }

                            KeyToImage(nowKey).enabled = true;
                            KeyToImage(nowKey).color = nowKeyColor;
                            if (nowKey != nextKey)
                            {
                                KeyToImage(nextKey).enabled = true;
                                KeyToImage(nextKey).color = nextKeyColor;
                            }
                        }
                        break;
                    }
                }
            }

            if (nowKeyNum == typingTextRoma[textNum].Length - 1)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(code))
                        {
                            if (code.ToString() == nowKey.ToString() && _hp > 0)
                            {
                                TextChange();
                            }
                        }
                    }
                }
            }

            //HP管理
            for(int h = 0; h < defaultHp; h++)
            {
                hp[h].enabled = false;
            }
            for (int h = 0; h < _hp; h++)
            {
                hp[h].enabled = true;
            }
            if(_hp <= 0)
            {
                state = STATE.GAMEOVER;
            }
        }

        if(state == STATE.GAMEOVER)
        {
            PlayerPrefs.SetInt("score", _score);
            StartCoroutine(GameOver());
        }
    }

    /// <summary>
    /// 入力テキスト変更
    /// </summary>
    void TextChange()
    {
        for (int i = 0; i < key.Length; i++)
        {
            key[i].enabled = false;
        }

        //レベル変更
        if (count < levelUpNum)
        {
            count++;
        }
        else if (count == levelUpNum) { count = 0; level++; }

        if (level == 1)
        {

        }
        else if (level == 2)
        {
            typingText = typingText2;
            typingTextRoma = typingTextRoma2;
        }

        textNum = UnityEngine.Random.Range(0, typingTextRoma.Length);
        text.text = typingText[textNum];
        textRoma.text = typingTextRoma[textNum].ToLower();
        textKanji.text = typingTextKanji[textNum];

        nowKeyNum = 0;
        nextKeyNum = 1;
        nowKey = typingTextRoma[textNum][nowKeyNum];
        nextKey = typingTextRoma[textNum][nextKeyNum];

        KeyToImage(lateKey).enabled = false;
        KeyToImage(nowKey).enabled = true;
        KeyToImage(nowKey).color = nowKeyColor;
        KeyToImage(nextKey).enabled = true;
        KeyToImage(nextKey).color = nextKeyColor;

        ScoreUp();
        _time = limitTime;
        time.color = new Color(1, 1, 1);
    }

    void ScoreUp()
    {
        float bounus = levelBounus * level;
        if(_time / limitTime >= 0.8)
        {
            _score += Mathf.FloorToInt(1000 * bounus);
        }
        if (_time / limitTime >= 0.5 && _time / limitTime < 0.8)
        {
            _score += Mathf.FloorToInt(500 * bounus);
        }
        if (_time / limitTime >= 0 && _time / limitTime < 0.5)
        {
            _score += Mathf.FloorToInt(300 * bounus);
        }
        scoreText.text = _score.ToString();
    }

    IEnumerator GameOver()
    {
        countDownText.text = "GAMEOVER";
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("ResultScene");
    }

    Image KeyToImage(char c) //charで指定されたImageを返す
    {
        Image k;

        switch (c)
        {
            case 'A':
                k = key[0];
                break;
            case 'B':
                k = key[1];
                break;
            case 'C':
                k = key[2];
                break;
            case 'D':
                k = key[3];
                break;
            case 'E':
                k = key[4];
                break;
            case 'F':
                k = key[5];
                break;
            case 'G':
                k = key[6];
                break;
            case 'H':
                k = key[7];
                break;
            case 'I':
                k = key[8];
                break;
            case 'J':
                k = key[9];
                break;
            case 'K':
                k = key[10];
                break;
            case 'L':
                k = key[11];
                break;
            case 'M':
                k = key[12];
                break;
            case 'N':
                k = key[13];
                break;
            case 'O':
                k = key[14];
                break;
            case 'P':
                k = key[15];
                break;
            case 'Q':
                k = key[16];
                break;
            case 'R':
                k = key[17];
                break;
            case 'S':
                k = key[18];
                break;
            case 'T':
                k = key[19];
                break;
            case 'U':
                k = key[20];
                break;
            case 'V':
                k = key[21];
                break;
            case 'W':
                k = key[22];
                break;
            case 'X':
                k = key[23];
                break;
            case 'Y':
                k = key[24];
                break;
            case 'Z':
                k = key[25];
                break;
            default:
                k = key[0];
                break;
        }

        return k;
    }

    IEnumerator CountDown()
    {
        for(int i = 3; i >= 0; i--)
        {
            if(i > 0)
            {
                countDownText.text = i.ToString();
            }
            if(i == 0)
            {
                countDownText.text = "START!!";
            }
            yield return new WaitForSeconds(1f);
        }
        countDownText.text = "";
        state = STATE.PLAY;

        textNum = UnityEngine.Random.Range(0, typingTextRoma.Length);
        text.text = typingText[textNum];
        textRoma.text = typingTextRoma[textNum].ToLower();
        textKanji.text = typingTextKanji[textNum];

        nowKeyNum = 0;
        nextKeyNum = 1;
        nowKey = typingTextRoma[textNum][nowKeyNum];
        nextKey = typingTextRoma[textNum][nextKeyNum];
        lateKey = nowKey;

        KeyToImage(nowKey).enabled = true;
        KeyToImage(nowKey).color = nowKeyColor;
        KeyToImage(nextKey).enabled = true;
        KeyToImage(nextKey).color = nextKeyColor;

        _time = limitTime;
    }
}

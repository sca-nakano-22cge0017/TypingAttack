using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// メインゲーム制御
/// </summary>
public class MainGame : MonoBehaviour
{
    [SerializeField] Canvas m_canvas;

    [SerializeField] Text text;
    [SerializeField] Text textRoma;
    [SerializeField] Text textKanji;

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
    List<RectTransform> pos = new();
    Vector3 particlePos;

    [SerializeField] ParticleSystem starEffect;

    List<string> typingText;
    List<string> typingTextRoma;
    List<string> typingTextKanji;
    int textNum;

    List<TextData> Texts = new();
    CSVLoader csvLoader;
    bool isLoad = false;

    [SerializeField] float levelBounus;
    float bounus = 1;
    int level = 1;
    int count = 1;
    [SerializeField] int levelUpNum;

    char nowKey;
    int nowKeyNum;
    char nextKey;
    int nextKeyNum;
    Color nextKeyColor = new Color(0.3207547f, 0.3207547f, 0.3207547f, 1f);
    Color nowKeyColor = new Color(1f, 1f, 1f, 1f);
    char lateKey;

    [SerializeField] Image damage;
    Animator damageAnim;

    [SerializeField] Fade fade;

    [SerializeField] Text levelUpText;
    Animator levelUpAnim;

    [SerializeField] AudioClip typingSE;
    [SerializeField] AudioClip levelUpSE;
    [SerializeField] AudioClip damageSE;
    [SerializeField] AudioClip countDownSE;
    [SerializeField] AudioClip startSE;
    [SerializeField] AudioSource audioSource;

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

        text.text = "ひらがな";
        textRoma.text = "ローマ字";
        textKanji.text = "漢字";

        for (int i = 0; i < key.Length; i++)
        {
            key[i].sprite = enemySprite[0];
            key[i].enabled = false;
            pos.Add(key[i].rectTransform);
        }

        csvLoader = GetComponent<CSVLoader>();

        damageAnim = damage.GetComponent<Animator>();
        levelUpAnim = levelUpText.GetComponent<Animator>();

        fade.FadeIn();

        StartCoroutine(CountDown());
    }

    void Update()
    {
        //CSVロード
        if(csvLoader.IsLoad && !isLoad)
        {
            Texts = csvLoader.texts;
            isLoad = true;
        }

        if(state == STATE.PLAY && fade.FadeInEnd)
        {
            //制限時間処理
            if(_time >= 0)
            {
                _time -= Time.deltaTime;
                time.fillAmount = _time / limitTime;
                time.color = new Color(1, _time / limitTime, _time / limitTime);

                if(_time <= 0)
                {
                    _hp--;

                    //ダメージ効果音
                    audioSource.PlayOneShot(damageSE);
                    damageAnim.SetTrigger("Damage");
                    if (_hp > 0)
                    {
                        TextChange();
                    }
                }
            }

            //現在打つ単語の文字が、打つ文字列の最後の文字でなかったら
            if (nowKeyNum != typingTextRoma[textNum].Length - 1)
            {
                //何か文字を打ったら
                if(Input.anyKeyDown)
                {
                    audioSource.PlayOneShot(typingSE); //効果音再生

                    //打った文字の確認
                    foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(code))
                        {
                            //打った文字を保存
                            lateKey = code.ToString()[0];

                            //打った文字が正しければ
                            if (code.ToString() == nowKey.ToString())
                            {
                                //エフェクト生成
                                var t = GetWorldPositionFromRectPosition(KeyPos(code.ToString()[0]));
                                starEffect.transform.position = new Vector3(t.x, t.y, 0);
                                starEffect.Play();

                                //表示変更
                                KeyToImage(nowKey).enabled = false;
                                KeyToImage(nextKey).enabled = false;
                                KeyToImage(nextKey).color = new Color(1, 1, 1);

                                //文字を進める
                                if (nextKeyNum < typingTextRoma[textNum].Length - 1)
                                {
                                    nextKeyNum++;
                                    nextKey = typingTextRoma[textNum][nextKeyNum];
                                }
                                if (nowKeyNum < typingTextRoma[textNum].Length - 1)
                                {
                                    nowKeyNum++;
                                    nowKey = typingTextRoma[textNum][nowKeyNum];
                                }

                                //表示変更
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
            }

            //現在打つ単語の文字が、打つ文字列の最後の文字だったら
            if (nowKeyNum == typingTextRoma[textNum].Length - 1)
            {
                if (Input.anyKeyDown)
                {
                    audioSource.PlayOneShot(typingSE);

                    foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(code))
                        {
                            if (code.ToString() == nowKey.ToString() && _hp > 0)
                            {
                                //文字列変更
                                TextChange();

                                //成功回数増加
                                if (count < levelUpNum)
                                {
                                    count++;
                                }
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

        //ゲームオーバー処理
        if(state == STATE.GAMEOVER)
        {
            PlayerPrefs.SetInt("score", _score);
            StartCoroutine(GameOver());
            state = STATE.WAIT;
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
        if (count == levelUpNum)
        { 
            count = 0; 
            level++;

            audioSource.PlayOneShot(levelUpSE);
            levelUpText.text = "level" + level.ToString();
            levelUpAnim.SetTrigger("LevelUp");

            bounus = levelBounus * bounus;

            TextDataInput(level);
        }

        textNum = UnityEngine.Random.Range(0, typingTextRoma.Count);
        text.text = typingText[textNum];
        textRoma.text = typingTextRoma[textNum].ToLower();
        textKanji.text = typingTextKanji[textNum];

        nowKeyNum = 0;
        nextKeyNum = 1;
        nowKey = typingTextRoma[textNum][nowKeyNum];
        nextKey = typingTextRoma[textNum][nextKeyNum];

        //表示変更
        KeyToImage(lateKey).enabled = false;
        KeyToImage(nowKey).enabled = true;
        KeyToImage(nowKey).color = nowKeyColor;
        KeyToImage(nextKey).enabled = true;
        KeyToImage(nextKey).color = nextKeyColor;

        ScoreUp();
        _time = limitTime;
        time.color = new Color(1, 1, 1);
    }

    int maxScore = 999999999;

    void ScoreUp()
    {
        if(_score < maxScore)
        {
            if (_time / limitTime >= 0.8)
            {
                _score += Mathf.FloorToInt(1000 * bounus);
            }
            if (_time / limitTime >= 0.5 && _time / limitTime < 0.8)
            {
                _score += Mathf.FloorToInt(500 * bounus / 2);
            }
            if (_time / limitTime >= 0 && _time / limitTime < 0.5)
            {
                _score += Mathf.FloorToInt(300 * bounus / 4);
            }
        }
        else if(_score >= maxScore)
        {
            _score = maxScore;
        }
        scoreText.text = _score.ToString();
    }

    /// <summary>
    /// ゲームオーバー
    /// </summary>
    IEnumerator GameOver()
    {
        levelUpText.text = "GAMEOVER";
        levelUpAnim.SetTrigger("GameOver");

        yield return new WaitForSeconds(3);

        fade.FadeOut();

        while(!fade.FadeOutEnd)
        {
            yield return null;
        }

        if(fade.FadeOutEnd)
        {
            SceneManager.LoadScene("ResultScene");
        }    
    }

    //charで指定されたImageを返す
    Image KeyToImage(char c)
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

    //charで指定されたRectTransformを返す
    RectTransform KeyPos(char c)
    {
        RectTransform k;

        switch (c)
        {
            case 'A':
                k = pos[0];
                break;
            case 'B':
                k = pos[1];
                break;
            case 'C':
                k = pos[2];
                break;
            case 'D':
                k = pos[3];
                break;
            case 'E':
                k = pos[4];
                break;
            case 'F':
                k = pos[5];
                break;
            case 'G':
                k = pos[6];
                break;
            case 'H':
                k = pos[7];
                break;
            case 'I':
                k = pos[8];
                break;
            case 'J':
                k = pos[9];
                break;
            case 'K':
                k = pos[10];
                break;
            case 'L':
                k = pos[11];
                break;
            case 'M':
                k = pos[12];
                break;
            case 'N':
                k = pos[13];
                break;
            case 'O':
                k = pos[14];
                break;
            case 'P':
                k = pos[15];
                break;
            case 'Q':
                k = pos[16];
                break;
            case 'R':
                k = pos[17];
                break;
            case 'S':
                k = pos[18];
                break;
            case 'T':
                k = pos[19];
                break;
            case 'U':
                k = pos[20];
                break;
            case 'V':
                k = pos[21];
                break;
            case 'W':
                k = pos[22];
                break;
            case 'X':
                k = pos[23];
                break;
            case 'Y':
                k = pos[24];
                break;
            case 'Z':
                k = pos[25];
                break;
            default:
                k = pos[0];
                break;
        }

        return k;
    }

    /// <summary>
    /// テキストデータの入力
    /// </summary>
    /// <param name="level">レベル</param>
    void TextDataInput(int level)
    {
        List<string> hiragana = new();
        List<string> roma = new();
        List<string> kanji = new();

        foreach (var t in Texts)
        {
            if (t.level == level)
            {
                hiragana.Add(t.hiragana);
                roma.Add(t.roma);
                kanji.Add(t.kanji);
            }
        }
        typingText = hiragana;
        typingTextRoma = roma;
        typingTextKanji = kanji;
    }

    /// <summary>
    /// カウントダウン
    /// </summary>
    /// <returns></returns>
    IEnumerator CountDown()
    {
        while(!fade.FadeInEnd)
        {
            yield return null;
        }

        for (int i = 3; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            if (i > 0)
            {
                audioSource.PlayOneShot(countDownSE);
                levelUpAnim.SetTrigger("LevelUp");
                levelUpText.text = i.ToString();
            }
            if (i == 0)
            {
                audioSource.PlayOneShot(startSE);
                levelUpAnim.SetTrigger("LevelUp");
                levelUpText.text = "START!!";
            }
        }
        yield return new WaitForSeconds(1f);
        levelUpText.text = "";
        state = STATE.PLAY;

        TextDataInput(1);

        textNum = UnityEngine.Random.Range(0, typingTextRoma.Count);
        text.text = typingText[textNum];
        textRoma.text = typingTextRoma[textNum].ToLower();
        textKanji.text = typingTextKanji[textNum];

        nowKeyNum = 0;
        nextKeyNum = 1;
        nowKey = typingTextRoma[textNum][nowKeyNum];
        nextKey = typingTextRoma[textNum][nextKeyNum];
        lateKey = nowKey;

        //表示変更
        KeyToImage(nowKey).enabled = true;
        KeyToImage(nowKey).color = nowKeyColor;
        KeyToImage(nextKey).enabled = true;
        KeyToImage(nextKey).color = nextKeyColor;

        _time = limitTime;
    }

    private Vector3 GetWorldPositionFromRectPosition(RectTransform rect)
    {
        //UI座標からスクリーン座標に変換
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(m_canvas.worldCamera, rect.position);

        //ワールド座標
        Vector3 result = Vector3.zero;

        //スクリーン座標→ワールド座標に変換
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, screenPos, m_canvas.worldCamera, out result);

        return result;
    }
}

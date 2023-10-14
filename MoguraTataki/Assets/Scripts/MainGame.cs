using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Text textRoma;
    [SerializeField] Text countDownText;
    [SerializeField] Text scoreText;

    [SerializeField] Image time;
    [SerializeField] Image[] hp;
    [SerializeField] Image[] key;

    string typingText;
    string typingTextRoma;

    enum STATE { WAIT = 0, PLAY, GAMEOVER};
    STATE state = 0;

    void Start()
    {
        state = STATE.WAIT;

        typingText = "‚ ‚¢‚¤‚¦‚¨";
        typingTextRoma = "aiueo";

        text.text = typingText;
        textRoma.text = typingTextRoma;

        for (int i = 0; i < key.Length; i++)
        {
            key[i].enabled = false;
        }

        StartCoroutine(CountDown());
    }

    void Update()
    {
        if(state == STATE.PLAY)
        {
            EnemyDisp();
        }
    }

    void EnemyDisp()
    {
        int textLength = typingTextRoma.Length;
        for (int i = 0; i < textLength; i++)
        {
            char c = typingTextRoma[i];
            Image k = KeyToImage(c);
            k.enabled = true;
        }
    }

    Image KeyToImage(char c) //char‚ÅŽw’è‚³‚ê‚½Image‚ð•Ô‚·
    {
        Image k;

        switch (c)
        {
            case 'a':
                k = key[0];
                break;
            case 'b':
                k = key[1];
                break;
            case 'c':
                k = key[2];
                break;
            case 'd':
                k = key[3];
                break;
            case 'e':
                k = key[4];
                break;
            case 'f':
                k = key[5];
                break;
            case 'g':
                k = key[6];
                break;
            case 'h':
                k = key[7];
                break;
            case 'i':
                k = key[8];
                break;
            case 'j':
                k = key[9];
                break;
            case 'k':
                k = key[10];
                break;
            case 'l':
                k = key[11];
                break;
            case 'm':
                k = key[12];
                break;
            case 'n':
                k = key[13];
                break;
            case 'o':
                k = key[14];
                break;
            case 'p':
                k = key[15];
                break;
            case 'q':
                k = key[16];
                break;
            case 'r':
                k = key[17];
                break;
            case 's':
                k = key[18];
                break;
            case 't':
                k = key[19];
                break;
            case 'u':
                k = key[20];
                break;
            case 'v':
                k = key[21];
                break;
            case 'w':
                k = key[22];
                break;
            case 'x':
                k = key[23];
                break;
            case 'y':
                k = key[24];
                break;
            case 'z':
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
    }
}

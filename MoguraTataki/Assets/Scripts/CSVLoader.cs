using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextData
{
    public int level;
    public string hiragana;
    public string roma;
    public string kanji;
}

/// <summary>
/// CSV�̎擾
/// </summary>
public class CSVLoader : MonoBehaviour
{
    List<TextData> Texts = new();
    int level = 0;
    bool isLoad = false;

    public List<TextData> texts
    {
        get{ return Texts; }
    }

    public bool IsLoad
    {
        get { return isLoad;}
    }

    void Awake()
    {
        CSVLoad();
    }

    void CSVLoad()
    {
        //Resource�t�@�C������f�[�^���[�h
        var textsData = Resources.Load<TextAsset>("Texts");

        //��s���ɕ�����
        var lineSplit = textsData.text.Split("\n");

        for(var i = 0; i < lineSplit.Length; i++)
        {
            //�R���}�ŋ�؂�
            var line = lineSplit[i].Split(",");

            //������̍ŏ��Ɂulevel�v���܂܂�Ă����ꍇ�A���x���ύX������������
            if (line[0].Contains("level"))
            {
                //�u��
                line[0] = line[0].Replace("level", "");

                //���x����ύX
                level = int.Parse(line[0]);
            }
            //�P�������������Ȃ�
            else if(line[0] != "")
            {
                //List�쐬�@1�P�ꂠ����̃f�[�^������
                TextData data = new();

                //���x���E�Ђ炪�ȁE���[�}���E����������
                data.level = level;
                data.hiragana = line[0];
                data.roma = line[1];
                data.kanji = line[2];

                //�S�Ă̒P��̃f�[�^��Texts�ɒǉ�����
                Texts.Add(data);
            }
        }

        //�f�[�^���[�h����
        isLoad = true;
    }
}

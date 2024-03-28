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
/// CSVの取得
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
        //Resourceファイルからデータロード
        var textsData = Resources.Load<TextAsset>("Texts");

        //一行毎に分ける
        var lineSplit = textsData.text.Split("\n");

        for(var i = 0; i < lineSplit.Length; i++)
        {
            //コンマで区切る
            var line = lineSplit[i].Split(",");

            //文字列の最初に「level」が含まれていた場合、レベル変更を示す文字列
            if (line[0].Contains("level"))
            {
                //置換
                line[0] = line[0].Replace("level", "");

                //レベルを変更
                level = int.Parse(line[0]);
            }
            //単語を示す文字列なら
            else if(line[0] != "")
            {
                //List作成　1単語あたりのデータを入れる
                TextData data = new();

                //レベル・ひらがな・ローマ字・漢字を入れる
                data.level = level;
                data.hiragana = line[0];
                data.roma = line[1];
                data.kanji = line[2];

                //全ての単語のデータをTextsに追加する
                Texts.Add(data);
            }
        }

        //データロード完了
        isLoad = true;
    }
}

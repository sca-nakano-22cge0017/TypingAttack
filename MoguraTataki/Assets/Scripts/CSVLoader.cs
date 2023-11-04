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

public class CSVLoader : MonoBehaviour
{
    List<TextData> Texts = new();
    int level = 0;

    public List<TextData> texts
    {
        get{ return Texts; }
    }

    void Awake()
    {
        CSVLoad();
    }

    void CSVLoad()
    {
        var textsData = Resources.Load<TextAsset>("texts");

        var lineSplit = textsData.text.Split("\n");

        for(var i = 0; i < lineSplit.Length; i++)
        {
            var line = lineSplit[i].Split(",");

            if (line[0].Contains("level"))
            {
                line[0] = line[0].Replace("level", "");
                level = int.Parse(line[0]);
            }
            else if(line[0] != "")
            {
                TextData data = new();
                data.level = level;
                data.hiragana = line[0];
                data.roma = line[1];
                data.kanji = line[2];

                Texts.Add(data);
            }
        }
    }
}

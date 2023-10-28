using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ToStart()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void ToResult()
    {
        SceneManager.LoadScene("ResultScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

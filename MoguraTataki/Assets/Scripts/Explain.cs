using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explain : MonoBehaviour
{
    [SerializeField] Fade fade;
    [SerializeField] AudioClip clip;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        fade.FadeIn();
    }

    void Update()
    {
        if (fade.FadeInEnd)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.PlayOneShot(clip);
                fade.FadeOut();
            }
        }

        if(fade.FadeOutEnd)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}

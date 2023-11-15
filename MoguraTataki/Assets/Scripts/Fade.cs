using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] float speed;
    bool isFadeOut = false;
    bool isFadeIn = false;
    bool fadeOutEnd = false;
    bool fadeInEnd = false;
    
    public bool FadeOutEnd { get { return fadeOutEnd; } set{ fadeOutEnd = value; } }

    public bool FadeInEnd { get { return fadeInEnd; } set { fadeInEnd = value; } }

    void Start()
    {
        
    }

    void Update()
    {
        if(isFadeOut)
        {
            if(transform.localPosition.x > 0)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            
            if (transform.localPosition.x <= 0)
            {
                transform.localPosition = new Vector3(0, 0, 0);
                fadeOutEnd = true;
                isFadeOut = false;
            }
        }

        if(isFadeIn)
        {

            if (transform.localPosition.x > -1920)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }

            if (transform.localPosition.x <= -1920)
            {
                transform.localPosition = new Vector3(-1920, 0, 0);
                fadeInEnd = true;
                isFadeIn = false;
            }
        }
    }

    public void FadeOut()
    {
        isFadeOut = true;
        transform.localPosition = new Vector3(1920, 0, 0);
    }

    public void FadeIn()
    {
        isFadeIn = true;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}

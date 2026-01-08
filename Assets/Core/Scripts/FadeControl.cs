using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeControl : MonoBehaviour
{
    private Image fadeImage;
    public bool FTB_running;
    public bool FFB_running;

    private void Start()
    {   
        fadeImage = GetComponent<Image>();
        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;
        FTB_running = false;
        FFB_running = false;
    }

    public void FadeToBlack(float duration)
    {
        if(!FTB_running)
            StartCoroutine(FadeCoroutine(0, 1, duration));
    }

    public void FadeFromBlack(float duration)
    {
        if(!FFB_running)
            StartCoroutine(FadeCoroutine(1, 0, duration));
    }

    private IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float duration)
    {
        if(startAlpha == 0)
            FTB_running = true;
        else
            FFB_running = true;

        float elapsedTime = 0;
        Color color = fadeImage.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = endAlpha;
        fadeImage.color = color;

        if(startAlpha == 0)
            yield return new WaitForSeconds(0.5f);
        
        if(startAlpha == 0)
            FTB_running = false;
        else
            FFB_running = false;
    }
}
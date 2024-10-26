using System;
using System.Collections;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public Action OnFadedOut { get; set; }
    public Action OnFadedIn { get; set; }

    public static Fade Instance;

    public AnimationCurve fadeCurve;
    
    void Awake()
    {
        Instance = this;
    }
    public void Out(float time)
    {
        StartCoroutine(IOut(time));
    }
    
    public void In(float time)
    {
        StartCoroutine(IIn(time));
    }

    [ContextMenu("Fade Out")]
    private void OutEditor()
    {
        Out(2f);
    }

    [ContextMenu("Fade In")]
    private void InEditor()
    {
        In(2f);
    }

    IEnumerator IOut(float time)
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = Color.black;

        var timer = 0f;
        while (timer < time)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            //RenderSettings.fogDensity = 2 * timer / time;
            RenderSettings.fogDensity = fadeCurve.Evaluate(timer / time);
        }
        RenderSettings.fogDensity = 2f;
        
        OnFadedOut?.Invoke();
        OnFadedOut = null;
    }
    
    IEnumerator IIn(float time)
    {
        var timer = 0f;
        while (timer < time)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            // RenderSettings.fogDensity = 4*( 1 - (timer / time));
            RenderSettings.fogDensity = fadeCurve.Evaluate( 1- (timer / time));
        }
        
        RenderSettings.fog = false;
        
        OnFadedIn?.Invoke();
        OnFadedIn = null;
    }
}

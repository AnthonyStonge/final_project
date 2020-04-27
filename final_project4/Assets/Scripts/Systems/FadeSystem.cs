using Unity.Entities;
using UnityEngine;
using static FadeObject.FadeType;
using static GameVariables;

public class FadeSystem : SystemBase
{
    public delegate void FadeEvent();

    public static FadeEvent OnFadeStart;
    public static FadeEvent OnFadeEnd;
    private bool hasFadeStarted;

    protected override void OnCreate()
    {
        Enabled = false;

        OnFadeEnd = () =>
        {
            //Debug.Log("OnFadeEnd");
            this.Enabled = false;
            hasFadeStarted = false;
        };
        OnFadeStart = () =>
        {
            //Debug.Log("OnFadeStart");
            hasFadeStarted = true;
        };
    }

    //Only update if trying to fade
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        var fadeObject = UI.FadeObject;
        if (!hasFadeStarted)
            OnFadeStart?.Invoke();

        //Update texture info depending on fade component info
        Color c = fadeObject.Image.color;

        c.a = fadeObject.FadeValue;
        fadeObject.Image.color = c;

        //Update fade component info
        switch (fadeObject.Type)
        {
            case FadeIn:
                if (fadeObject.FadeValue <= 0)
                    OnFadeEnd?.Invoke();
                fadeObject.FadeValue -= fadeObject.Speed * deltaTime;
                break;
            case FadeOut:
                if (fadeObject.FadeValue >= 1)
                    OnFadeEnd?.Invoke();
                fadeObject.FadeValue += fadeObject.Speed * deltaTime;
                break;
        }
        UI.FadeObject = fadeObject;
    }
}
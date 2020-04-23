using Unity.Entities;
using UnityEngine;
using static GameVariables;

public class FadeSystem : SystemBase
{
    public delegate void FadeEvent();

    public static FadeEvent OnFadeStart;
    public static FadeEvent OnFadeEnd;

    private bool hasFadeStarted;

    protected override void OnCreate()
    {
        this.Enabled = false;

        OnFadeEnd = () =>
        {
            Debug.Log("OnFadeEnd");
            this.Enabled = false;
            hasFadeStarted = false;
        };
        OnFadeStart = () =>
        {
            Debug.Log("OnFadeStart");
            hasFadeStarted = true;
        };
    }

    //Only update if trying to fade
    protected override void OnUpdate()
    {
        if (!hasFadeStarted)
            OnFadeStart?.Invoke();

        //Update texture info depending on fade component info
        Color c = UI.FadeObject.Image.color;

        c.a = UI.FadeObject.FadeValue;
        UI.FadeObject.Image.color = c;

        //Update fade component info
        switch (UI.FadeObject.Type)
        {
            case FadeObject.FadeType.FadeIn:
                UI.FadeObject.FadeValue -= UI.FadeObject.Speed * Time.DeltaTime;
                break;
            case FadeObject.FadeType.FadeOut:
                UI.FadeObject.FadeValue += UI.FadeObject.Speed * Time.DeltaTime;
                break;
        }

        //Should system stop?
        if (UI.FadeObject.FadeValue <= 0 || UI.FadeObject.FadeValue >= 1)
            OnFadeEnd?.Invoke();
    }
}
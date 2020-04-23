using Unity.Entities;
using UnityEngine;
using static GameVariables;

public class FadeSystem : SystemBase
{
    protected override void OnCreate()
    {
        this.Enabled = false;
    }

    //Only update if trying to fade
    protected override void OnUpdate()
    {
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
            this.Enabled = false;
    }
}
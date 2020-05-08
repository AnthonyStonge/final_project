using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class ShakeCamSystem : SystemBase
{
    public delegate void ShakeEvent();

    public static ShakeEvent OnShakeStart;
    public static ShakeEvent OnShakeEnd;

    protected override void OnCreate()
    {
        this.Enabled = false;

        OnShakeEnd = () =>
        {
            //Debug.Log("OnFadeEnd");
            this.Enabled = false;
            GameVariables.CamNoiseProfile.m_AmplitudeGain = 0;
            GameVariables.CamNoiseProfile.m_FrequencyGain = 0;
        };
        OnShakeStart = () =>
        {
            //Debug.Log("OnFadeStart");
            GameVariables.CamNoiseProfile.m_AmplitudeGain = GameVariables.ShakeComponent.ShakeAmplitude;
            GameVariables.CamNoiseProfile.m_FrequencyGain = GameVariables.ShakeComponent.ShakeFrequency;
        };
    }

    //Only update if trying to fade
    protected override void OnUpdate()
    {
        OnShakeStart?.Invoke();
        
        if (GameVariables.ShakeComponent.CamShakeDuration <= 0)
            OnShakeEnd.Invoke();
        GameVariables.ShakeComponent.CamShakeDuration -= Time.DeltaTime;
        
    }
}

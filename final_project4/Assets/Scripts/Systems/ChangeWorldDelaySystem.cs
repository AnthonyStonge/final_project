using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class ChangeWorldDelaySystem : SystemBase
{
    private static float Timer;

    public delegate void OnChangeWorldEvent();

    public static OnChangeWorldEvent OnChangeWorld;

    protected override void OnCreate()
    {
        Enabled = false;
        OnChangeWorld = GlobalEvents.CameraEvents.FadeIn;
    }

    protected override void OnStartRunning()
    {
        //Fade out
        GlobalEvents.CameraEvents.FadeOut();
        
        //Block Systems
        GlobalEvents.GameEvents.DisableGameLogic(true);
    }

    protected override void OnStopRunning()
    {
        //UnBlock Systems
        GlobalEvents.GameEvents.DisableGameLogic(false);
    }

    protected override void OnUpdate()
    {
        //Decrement timer
        Timer -= Time.DeltaTime;

        if (Timer > 0)
            return;

        //
        OnChangeWorld.Invoke();
        
        //Clear Event
        OnChangeWorld = GlobalEvents.CameraEvents.FadeIn;
        
        Enabled = false;
    }

    public static void ChangeWorld(float delay)
    {
        Timer = delay;
        Unity.Entities.World.DefaultGameObjectInjectionWorld.GetExistingSystem<ChangeWorldDelaySystem>().Enabled = true;
    }
}

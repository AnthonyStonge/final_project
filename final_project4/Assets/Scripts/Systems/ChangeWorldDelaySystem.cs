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
        World.GetExistingSystem<LateInitializeManager>().Enabled = false;
        World.GetExistingSystem<TransformSimulationManager>().Enabled = false;
        World.GetExistingSystem<LateSimulationManager>().Enabled = false;
        World.GetExistingSystem<PresentationManager>().Enabled = false;
        World.GetExistingSystem<SimulationSystemGroup>().Enabled = false;
    }

    protected override void OnStopRunning()
    {
        //UnBlock Systems
        World.GetOrCreateSystem<LateInitializeManager>().Enabled = true;
        World.GetOrCreateSystem<TransformSimulationManager>().Enabled = true;
        World.GetOrCreateSystem<LateSimulationManager>().Enabled = true;
        World.GetOrCreateSystem<PresentationManager>().Enabled = true;
        World.GetOrCreateSystem<SimulationSystemGroup>().Enabled = true;
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

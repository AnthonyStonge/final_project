using Enums;
using EventStruct;
using Holder;
using Holders;
using Static.Events;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public static class GameInitializer
{
    public static void InitializeSystemWorkflow()
    {
        //Init archetypes (must be done before creating any entities)*
        StaticArchetypes.InitializeArchetypes();

        GameVariables.EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        PlayerHolder.Initialize();
        EnemyHolder.Initialize();
        WeaponHolder.Initialize();
        SoundHolder.Initialize();
        //ProjectileHolder.Initialize();
        
        //TODO REMOVE LINEs UNDER
        SoundManager.audioSource = MonoGameVariables.instance.playerAudioSource;
        SoundHolder.WeaponSounds[GunType.SHOTGUN][WeaponInfo.WeaponEventType.ON_SHOOT] =
            MonoGameVariables.instance.temporaryShotgunShotSound;
        SoundHolder.BulletSounds[BulletType.SHOTGUN_BULLET][BulletInfo.BulletCollisionType.ON_WALL] =
            MonoGameVariables.instance.temporaryShotgunShotSound;
        
        //Event init
        PlayerEvents.Initialize();
        GunEvents.Initialize();
        
        //Init holder?
        GameVariables.PlayerVars.Default = MonoGameVariables.instance.playerAssets;
        GameVariables.PlayerVars.Dash = MonoGameVariables.instance.playerDashAssets;
        // GameVariables.PlayerVars.Pistol = MonoGameVariables.instance.playerPistolAssets;
        GameVariables.PlayerVars.Default.PlayerAudioSource = MonoGameVariables.instance.playerAudioSource;
        
        //Init map
        MapInitializer.Initialize();

        var world = World.DefaultGameObjectInjectionWorld;
        
        //System Group Handles (From Unity)
        var initialization = world.GetOrCreateSystem<InitializationSystemGroup>();
        var transform = world.GetOrCreateSystem<TransformSystemGroup>();
        var lateSimulation = world.GetOrCreateSystem<LateSimulationSystemGroup>();
        var presentation = world.GetOrCreateSystem<PresentationSystemGroup>();

        //Managers
        var initializeManager = world.GetOrCreateSystem<InitializeManager>();
        var afterInitialization = world.GetOrCreateSystem<LateInitializeManager>();
        
        var transformSimulationManager = world.GetOrCreateSystem<TransformSimulationManager>();
        var lateSimulationManager = world.GetOrCreateSystem<LateSimulationManager>();

        var presentationManager = world.GetOrCreateSystem<PresentationManager>();
        
        //Adding systems
        initialization.AddSystemToUpdateList(initializeManager);
        initialization.AddSystemToUpdateList(afterInitialization);

        transform.AddSystemToUpdateList(transformSimulationManager);
        lateSimulation.AddSystemToUpdateList(lateSimulationManager);
        
        presentation.AddSystemToUpdateList(presentationManager);
        
        //Sorting
        initialization.SortSystemUpdateList();
        transform.SortSystemUpdateList();
        lateSimulation.SortSystemUpdateList();
        presentation.SortSystemUpdateList();
    }

    public static void OnDestroy()
    {
        EventsHolder.OnDestroy();
        
        //Holders
        PlayerHolder.OnDestroy();
        EnemyHolder.OnDestroy();
        WeaponHolder.OnDestroy();
        ProjectileHolder.OnDestroy();
    }

    public static void SetMainCamera(Camera cam)
    {
        GameVariables.MainCamera = cam;
    }
}

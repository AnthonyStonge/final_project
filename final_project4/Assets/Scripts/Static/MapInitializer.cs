using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static GameVariables;

public static class MapInitializer
{
    private static EntityManager entityManager;

    public static void Initialize()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        if (entityManager == null)
            return;

        InitializePlayer();
    }

    private static void InitializePlayer()
    {
        //Create player entity
        Entity player = entityManager.CreateEntity(StaticArchetypes.PlayerArchetype);
        entityManager.SetName(player, "Player");

        //Set position
        entityManager.SetComponentData(player, new Translation
        {
            Value = PlayerVars.SpawnPosition
        });
        entityManager.SetComponentData(player, new Rotation
        {
            Value = PlayerVars.SpawnRotation
        });
        entityManager.SetComponentData(player, new HealthData
        {
            Value = PlayerVars.Health
        });
        entityManager.SetComponentData(player, new SpeedData
        {
            Value = PlayerVars.Speed
        });
        entityManager.SetComponentData(player, new StateData
        {
            Value = StateActions.IDLE
        });
        entityManager.SetComponentData(player, new DashComponent
        {
            Distance = PlayerVars.DashDistanceBasic,
            Timer = new TimeTrackerComponent
            {
                ResetValue = PlayerVars.DashResetValue
            }
        });
        //TODO CHANGE MESH COMPONENT FOR AN "ANIMATOR"
        entityManager.SetSharedComponentData(player, new RenderMesh
        {
            mesh = MonoGameVariables.instance.PlayerMesh,
            material = MonoGameVariables.instance.playerMaterial
        });

        //Set info in GameVariables
        PlayerVars.Entity = player;
        PlayerVars.CurrentPosition = PlayerVars.SpawnPosition;
        PlayerVars.CurrentState = StateActions.IDLE;
        PlayerVars.IsAlive = PlayerVars.Health > 0;
    }
}
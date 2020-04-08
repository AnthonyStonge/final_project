using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

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
            Value = float3.zero
        });
        entityManager.SetComponentData(player, new Rotation
        {
            Value = quaternion.identity
        });
        entityManager.SetComponentData(player, new HealthData
        {
            Value = GameVariables.PlayerHealth
        });
        entityManager.SetComponentData(player, new SpeedData
        {
            Value = GameVariables.PlayerSpeed
        });
        //TODO CHANGE MESH COMPONENT FOR AN "ANIMATOR"
        entityManager.SetSharedComponentData(player, new RenderMesh
        {
            mesh = MonoGameVariables.instance.PlayerMesh,
            material = MonoGameVariables.instance.playerMaterial
        });

        //Set info in GameVariables
        GameVariables.PlayerEntity = player;
        GameVariables.PlayerCurrentPosition = GameVariables.PlayerSpawnPosition;
    }
}
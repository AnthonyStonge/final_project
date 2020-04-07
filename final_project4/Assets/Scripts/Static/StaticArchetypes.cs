using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public  class StaticArchetypes
{
    public static EntityManager entityManager;
    
    public static EntityArchetype PlayerArchetype;
    public static EntityArchetype EnemyArchetype;
    public static EntityArchetype GunArchetype;
    public static EntityArchetype BulletArchetype;

    public static void InitializeArchetypes()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        if (entityManager == null)
        {
            Debug.Log("EntityManager not found... Couldnt initialize Archetypes...");
            return;
        }

        InitPlayerArchetype();
        InitEnemyArchetype();
        InitGunArchetype();
        InitBulletArchetype();
    }

    private static void InitPlayerArchetype()
    {
        PlayerArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(Scale),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds)
            );
    }

    private static void InitEnemyArchetype()
    {
        EnemyArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(Scale),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds)
        );
    }

    private static void InitGunArchetype()
    {
        GunArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(Scale),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds)
        );
    }

    private static void InitBulletArchetype()
    {
        BulletArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(Scale),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds)
        );
    }
}

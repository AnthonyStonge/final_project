using UnityEngine;
using Unity.Entities;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;

public class StaticArchetypes
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
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(StateData),
            typeof(SpeedData),
            typeof(HealthData),
            typeof(TargetData),
            typeof(DashComponent),
            typeof(InputComponent),
            //typeof(PhysicsVelocity),
            //typeof(PhysicsDamping),
            //typeof(PhysicsMass),
            //typeof(PhysicsGravityFactor),
            //typeof(PhysicsCollider),

            //Tags
            typeof(PlayerTag)
        );
    }

    private static void InitEnemyArchetype()
    {
        EnemyArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds)
        );
    }

    //When using this archetype, dont forget to add a specific gun tag + gun component to the entity.
    private static void InitGunArchetype()
    {
        GunArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToParent),
            typeof(Parent),

            //Tags
            typeof(GunTag)
        );
    }

    private static void InitBulletArchetype()
    {
        BulletArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(LocalToWorld),
            typeof(Scale),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(SpeedData),
            typeof(TimeTrackerComponent),
            //typeof(PhysicsVelocity),
            //typeof(PhysicsDamping),
            //typeof(PhysicsMass),
            //typeof(PhysicsGravityFactor),
            typeof(PhysicsCollider),
            
            //Tags
            typeof(BulletTag)
        );
    }
}
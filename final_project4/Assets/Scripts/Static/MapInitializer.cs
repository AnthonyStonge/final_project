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
        InitializePlayerWeapon();
    }

    private static void InitializePlayer()
    {
        //Create player entity
        Entity player = entityManager.CreateEntity(StaticArchetypes.PlayerArchetype);
        entityManager.SetName(player, "Player");

        //Set Values
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
            Distance = PlayerVars.DefaultDashDistance,
            Timer = new TimeTrackerComponent
            {
                ResetValue = PlayerVars.DashResetTime
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

    private static void InitializePlayerWeapon()
    {
        //TODO INITIALIZE ANY WEAPON NOT ONLY PISTOL
        //Create player entity
        Entity weapon = entityManager.CreateEntity(StaticArchetypes.GunArchetype);
        entityManager.SetName(weapon, "PlayerWeapon");

        entityManager.AddComponent<PistolComponent>(weapon);

        //Set Values
        entityManager.SetComponentData(weapon, new Translation
        {
            //TODO PROBLEM IF PLAYER SPAWNS WITH A ROTATION
            Value = PlayerVars.SpawnPosition + PistolVars.PlayerOffset
        });
        entityManager.SetComponentData(weapon, new Rotation
        {
            Value = PlayerVars.SpawnRotation
        });
        entityManager.SetComponentData(weapon, new Parent
        {
            Value = PlayerVars.Entity
        });
        entityManager.SetComponentData(weapon, new PistolComponent
        {
            MagasineSize = PistolVars.MagazineSize,
            CurrentBulletInMagazine = PlayerVars.StartingBulletAmount,
            ReloadTime = new TimeTrackerComponent
            {
                ResetValue = PistolVars.ReloadTime
            },
            BetweenShotTime = new TimeTrackerComponent
            {
                ResetValue = PistolVars.BetweenShotTime
            }
        });
        entityManager.SetSharedComponentData(weapon, new RenderMesh
        {
            mesh = MonoGameVariables.instance.PistolMesh,
            material = MonoGameVariables.instance.PistolMaterial
        });

    }
}
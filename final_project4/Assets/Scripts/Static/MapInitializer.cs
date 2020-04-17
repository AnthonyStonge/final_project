using Holders;
﻿using Enums;
using Static.Events;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using static GameVariables;

public static class MapInitializer
{
    private static EntityManager entityManager;

    private static RenderMesh pistolRenderMesh;

    public static void Initialize()
    {
        entityManager = GameVariables.EntityManager;

        if (entityManager == null)
            return;

        //TODO CHANGE SPAWN POSITION HERE IF LOADING FROM SAVE
        //Get value from scriptable object
        float3 spawnPosition =
            PlayerVars.Default.UseDebugVariables
                ? PlayerVars.Default.StartingPosition.Value
                : PlayerVars.Default.DefaultSpawnPosition.Value;
        quaternion spawnRotation =
            PlayerVars.Default.UseDebugVariables
                ? PlayerVars.Default.StartingRotation.Value
                : PlayerVars.Default.DefaultSpawnRotation.Value;
        short spawnHealth = PlayerVars.Default.UseDebugVariables
            ? PlayerVars.Default.StartingHealth
            : PlayerVars.Default.DefaultHealth;

        //Create player
        PlayerEvents.OnPlayerSpawn.Invoke(spawnPosition, spawnRotation, spawnHealth);

        InitializePlayerWeapon();
    }

    private static void InitializePlayerWeapon()
    {
        //TODO INITIALIZE ANY WEAPON NOT ONLY PISTOL
        //Create player entity
        Entity weapon = entityManager.CreateEntity(StaticArchetypes.GunArchetype);
        entityManager.SetName(weapon, "Player Weapon");

        //Set Values
        entityManager.SetComponentData(weapon, new Translation
        {
            //TODO PROBLEM IF PLAYER SPAWNS WITH A ROTATION
            Value = PlayerVars.Default.DefaultSpawnPosition.Value + PistolVars.PlayerOffset
        });
        
        entityManager.SetComponentData(weapon, new Rotation
        {
            Value = PlayerVars.Default.DefaultSpawnRotation.Value

        });
        
        entityManager.SetComponentData(weapon, new Parent
        {
            Value = PlayerVars.Entity
        });
        
        entityManager.SetSharedComponentData(weapon, new RenderMesh
        {
            mesh = MonoGameVariables.instance.PistolMesh,
            material = MonoGameVariables.instance.PistolMaterial
        });
        
        entityManager.SetComponentData(weapon, new GunComponent
        {
            GunType = GunType.PISTOL,
            Bullet = ProjectileHolder.PistolPrefab,
            ResetReloadTime = 0.3f,
            MaxBulletInMagazine = 20,
            CurrentAmountBulletInMagazine = 2000,
            CurrentAmountBulletOnPlayer = 9999999
        });
    }
}
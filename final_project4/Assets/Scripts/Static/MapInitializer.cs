using Holders;
﻿using Enums;
using Holder;
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

        //Create player entity
        Entity player = entityManager.Instantiate(PlayerHolder.PlayerPrefabEntity);
        entityManager.SetComponentData(player, new Translation
        {
            Value = float3.zero    //TODO SET SPAWN POSITION
        });
        entityManager.SetComponentData(player, new Rotation
        {
            Value = quaternion.identity //TODO SET SPAWN ROTATION
        });
        PlayerVars.Entity = player;
        
        //Create weapons for player
        Entity pistol = entityManager.Instantiate(WeaponHolder.WeaponPrefabs[GunType.PISTOL]);
        entityManager.SetComponentData(pistol, new Parent
        {
            Value = player
        });
        
        Entity shotgun = entityManager.Instantiate(WeaponHolder.WeaponPrefabs[GunType.SHOTGUN]);
        entityManager.SetComponentData(shotgun, new Parent
        {
            Value = player
        });
        entityManager.SetEnabled(shotgun, false);
        

        //InitializePlayerWeapon();
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
            BulletPrefab = ProjectileHolder.PistolPrefab,
            ResetReloadTime = 0.3f,
            MaxBulletInMagazine = 20,
            CurrentAmountBulletInMagazine = 2000,
            CurrentAmountBulletOnPlayer = 9999999
        });
    }
}
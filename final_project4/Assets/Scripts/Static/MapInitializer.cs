
using Enums;
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
        Entity player = entityManager.Instantiate(PlayerHolder.PlayerDict[PlayerType.Player]);
        entityManager.SetComponentData(player, new Translation
        {
            Value = float3.zero    //TODO SET SPAWN POSITION
        });
        entityManager.SetComponentData(player, new Rotation
        {
            Value = quaternion.identity //TODO SET SPAWN ROTATION
        });
        PlayerVars.Entity = player;
        PlayerVars.CurrentWeaponHeld = WeaponType.Pistol;
        
        //Create weapons for player
        Entity pistol = entityManager.Instantiate(WeaponHolder.WeaponDict[WeaponType.Pistol]);
        entityManager.SetComponentData(pistol, new Parent
        {
            Value = player
        });
        //PlayerVars.PlayerWeaponEntities.Add(WeaponType.Pistol, pistol);
        
        Entity shotgun = entityManager.Instantiate(WeaponHolder.WeaponDict[WeaponType.Shotgun]);
        entityManager.SetComponentData(shotgun, new Parent
        {
            Value = player
        });
        entityManager.SetEnabled(shotgun, false);
        //PlayerVars.PlayerWeaponEntities.Add(WeaponType.Shotgun, shotgun);
        

        //InitializePlayerWeapon();
    }

    private static void InitializePlayerWeapon()
    {
       /* //TODO INITIALIZE ANY WEAPON NOT ONLY PISTOL
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
            weaponType = WeaponType.PISTOL,
            BulletPrefab = ProjectileHolder.PistolPrefab,
            ResetReloadTime = 0.3f,
            MaxBulletInMagazine = 20,
            CurrentAmountBulletInMagazine = 2000,
            CurrentAmountBulletOnPlayer = 9999999
        });*/
    }
}
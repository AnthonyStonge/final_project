
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
        PlayerVars.PlayerWeaponEntities.Add(WeaponType.Pistol, pistol);
        
        Entity shotgun = entityManager.Instantiate(WeaponHolder.WeaponDict[WeaponType.Shotgun]);
        entityManager.SetComponentData(shotgun, new Parent
        {
            Value = player
        });
        entityManager.SetEnabled(shotgun, false);
        PlayerVars.PlayerWeaponEntities.Add(WeaponType.Shotgun, shotgun);
    }
}
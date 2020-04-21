using Enums;
using Unity.Assertions;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static GameVariables;

public static class PlayerInitializer
{
    public static void Initialize()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Assert.IsNotNull(entityManager);

        //Create player entity
        Entity player = entityManager.Instantiate(PlayerHolder.PlayerPrefabDict[PlayerType.Player]);
        entityManager.SetComponentData(player, new Translation
        {
            Value = float3.zero //TODO SET SPAWN POSITION
        });
        entityManager.SetComponentData(player, new Rotation
        {
            Value = quaternion.identity //TODO SET SPAWN ROTATION
        });
        Player.Entity = player;
        Player.CurrentWeaponHeld = WeaponType.Pistol;
    }
}
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
        
        if (Player.Entity != Entity.Null)
            entityManager.DestroyEntity(Player.Entity);
        
        Assert.IsNotNull(entityManager);

        //Create player entity
        Entity player = entityManager.Instantiate(PlayerHolder.PlayerPrefabDict[PlayerType.Player]);
        
        //TODO SET SPAWN POSITION AND ROTATION
        
        Player.Entity = player;
        Player.CurrentWeaponHeld = WeaponType.Pistol;
    }
    public static void SetPosition()
    {
        
    }

    
    public static void SetDefault<T>(T Component)
    {
        
    }

    public static void ResetToDefault()
    {
    }
}
using Enums;
using Unity.Assertions;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using static GameVariables;

public class WeaponInitializer
{
    public static void Initialize()
    {
        if (Player.PlayerWeaponEntities.Count > 0)
            Reset();

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Assert.IsNotNull(entityManager);
        //Create normal weapons for player
        Entity pistol = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[WeaponType.Pistol]);
        entityManager.SetComponentData(pistol, new Parent
        {
            Value = Player.Entity
        });
        Player.PlayerWeaponEntities.Add(WeaponType.Pistol, pistol);

        Entity shotgun = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[WeaponType.Shotgun]);
        entityManager.SetComponentData(shotgun, new Parent
        {
            Value = Player.Entity
        });
        entityManager.SetEnabled(shotgun, false);
        Player.PlayerWeaponEntities.Add(WeaponType.Shotgun, shotgun);

        Entity machinegun = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[WeaponType.Machinegun]);
        entityManager.SetComponentData(machinegun, new Parent
        {
            Value = Player.Entity
        });
        entityManager.SetEnabled(machinegun, false);
        Player.PlayerWeaponEntities.Add(WeaponType.Machinegun, machinegun);

        //Create hell weapons for player
        Entity hellPistol = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[WeaponType.HellPistol]);
        entityManager.SetComponentData(hellPistol, new Parent
        {
            Value = Player.Entity
        });
        entityManager.SetEnabled(hellPistol, false);
        Player.PlayerHellWeaponEntities.Add(WeaponType.HellPistol, hellPistol);

        Entity hellShotgun = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[WeaponType.HellShotgun]);
        entityManager.SetComponentData(hellShotgun, new Parent
        {
            Value = Player.Entity
        });
        entityManager.SetEnabled(hellShotgun, false);
        Player.PlayerHellWeaponEntities.Add(WeaponType.HellShotgun, hellShotgun);

        Entity hellMachineGun = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[WeaponType.HellMachinegun]);
        entityManager.SetComponentData(hellMachineGun, new Parent
        {
            Value = Player.Entity
        });
        entityManager.SetEnabled(hellMachineGun, false);
        Player.PlayerHellWeaponEntities.Add(WeaponType.HellMachinegun, hellMachineGun);

        //Set initial player weapons
        Player.PlayerCurrentWeapons.Add(WeaponType.Pistol);
        UIManager.SetWeaponType(WeaponType.Pistol);
    }

    public static void Reset()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        foreach (var playerWeaponEntity in Player.PlayerWeaponEntities)
        {
            entityManager.DestroyEntity(playerWeaponEntity.Value);
        }

        Player.PlayerWeaponEntities.Clear();
    }
}
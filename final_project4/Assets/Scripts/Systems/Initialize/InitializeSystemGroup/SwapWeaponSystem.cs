using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class SwapWeaponSystem : SystemBase
{
    private static EntityManager manager;

    private static float swapWeaponTimer;
    private static float delaySwapWeapon = 0.02f; //TODO CHOOSE A GOOD TIMER DELAY

    protected override void OnCreate()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        //Get player inputs
        InputComponent inputs = EntityManager.GetComponentData<InputComponent>(GameVariables.Player.Entity);

        if (!inputs.Enabled)
            return;

        //Delay to swap weapon < 0
        if (swapWeaponTimer > 0)
        {
            swapWeaponTimer -= Time.DeltaTime;
            return;
        }

        WeaponType weaponDesired = GameVariables.Player.CurrentWeaponHeld;

        //Check for inputs to swap weapon
        if (EventsHolder.LevelEvents.CurrentLevel != MapType.Level_Hell)
        {
            if (inputs.SwapWeapon1)
                TryGetWeaponType(out weaponDesired, WeaponType.Pistol);
            else if (inputs.SwapWeapon2)
                TryGetWeaponType(out weaponDesired, WeaponType.Machinegun);
            else if (inputs.SwapWeapon3)
                TryGetWeaponType(out weaponDesired, WeaponType.Shotgun);
            else if (inputs.MouseWheel.y > 0)
                GetPreviousWeapon(out weaponDesired);
            else if (inputs.MouseWheel.y < 0)
                GetNextWeapon(out weaponDesired);
        }
        else
        {
            if (inputs.SwapWeapon1)
                weaponDesired = WeaponType.HellPistol;
            else if (inputs.SwapWeapon2)
                weaponDesired = WeaponType.HellMachinegun;
            else if (inputs.SwapWeapon3)
                weaponDesired = WeaponType.HellShotgun;
            /*else if (inputs.MouseWheel.y > 0)
                GetPreviousWeapon(out weaponDesired);
            else if (inputs.MouseWheel.y < 0)
                GetNextWeapon(out weaponDesired);*/
        }

        //Make sure weapon actually swapped
        if (weaponDesired == GameVariables.Player.CurrentWeaponHeld)
            return;

        SwapWeapon(weaponDesired);
    }

    private void SwapWeapon(WeaponType type)
    {
        swapWeaponTimer = delaySwapWeapon;

        //Add event to NativeList
        EventsHolder.WeaponEvents.Add(new WeaponInfo
        {
            Parent = GameVariables.Player.Entity,
            WeaponType = type,
            EventType = WeaponInfo.WeaponEventType.ON_SWAP
        });

        //Activate/Deactivate weapons
        Entity currentWeaponEntity = EventsHolder.LevelEvents.CurrentLevel != MapType.Level_Hell
            ? GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]
            : GameVariables.Player.PlayerHellWeaponEntities[GameVariables.Player.CurrentWeaponHeld];
        Entity desiredWeaponEntity = EventsHolder.LevelEvents.CurrentLevel != MapType.Level_Hell
            ? GameVariables.Player.PlayerWeaponEntities[type]
            : GameVariables.Player.PlayerHellWeaponEntities[type];

        EntityManager.SetEnabled(currentWeaponEntity, false);
        EntityManager.SetEnabled(desiredWeaponEntity, true);

        //Set Cooldown to shoot after swap
        GunComponent gun = EntityManager.GetComponentData<GunComponent>(desiredWeaponEntity);
        gun.SwapTimer = gun.OnSwapDelayToShoot;
        EntityManager.SetComponentData(desiredWeaponEntity, gun);

        //Set CurrentGunType
        GameVariables.Player.CurrentWeaponHeld = type;
    }

    public static void SwapWeaponBetweenWorld(WeaponType type, MapType previous, MapType current)
    {
        swapWeaponTimer = delaySwapWeapon;

        //Add event to NativeList
        EventsHolder.WeaponEvents.Add(new WeaponInfo
        {
            Parent = GameVariables.Player.Entity,
            WeaponType = type,
            EventType = WeaponInfo.WeaponEventType.ON_SWAP
        });

        //Activate/Deactivate weapons
        Entity currentWeaponEntity = previous != MapType.Level_Hell
            ? GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]
            : GameVariables.Player.PlayerHellWeaponEntities[GameVariables.Player.CurrentWeaponHeld];
        Entity desiredWeaponEntity = current != MapType.Level_Hell
            ? GameVariables.Player.PlayerWeaponEntities[type]
            : GameVariables.Player.PlayerHellWeaponEntities[type];

        manager.SetEnabled(currentWeaponEntity, false);
        manager.SetEnabled(desiredWeaponEntity, true);

        //Set Cooldown to shoot after swap
        GunComponent gun = manager.GetComponentData<GunComponent>(desiredWeaponEntity);
        gun.SwapTimer = gun.OnSwapDelayToShoot;
        manager.SetComponentData(desiredWeaponEntity, gun);

        //Set CurrentGunType
        GameVariables.Player.CurrentWeaponHeld = type;
    }

    private bool TryGetWeaponType(out WeaponType type, WeaponType typeDesired)
    {
        type = GameVariables.Player.CurrentWeaponHeld;

        //Get Weapon on Player
        List<WeaponType> playerCurrentWeapons = GameVariables.Player.PlayerCurrentWeapons;

        //Make sure Player has TypeDesired
        if (!playerCurrentWeapons.Contains(typeDesired))
            return false;

        type = typeDesired;
        return true;
    }

    private bool GetNextWeapon(out WeaponType type)
    {
        type = GameVariables.Player.CurrentWeaponHeld;

        //Get Weapon on Player
        List<WeaponType> playerCurrentWeapons = GameVariables.Player.PlayerCurrentWeapons;

        //Make sure theres more than one weapon
        if (playerCurrentWeapons.Count <= 1)
            return false;

        //Get current index
        int currentIndex = playerCurrentWeapons.IndexOf(type);

        //Increase / Clamp
        currentIndex++;
        currentIndex %= playerCurrentWeapons.Count;

        //Get type with new index
        type = playerCurrentWeapons[currentIndex];

        return true;
    }

    private bool GetPreviousWeapon(out WeaponType type)
    {
        type = GameVariables.Player.CurrentWeaponHeld;

        //Get Weapon on Player
        List<WeaponType> playerCurrentWeapons = GameVariables.Player.PlayerCurrentWeapons;

        //Make sure theres more than one weapon
        if (playerCurrentWeapons.Count <= 1)
            return false;

        //Get current index
        int currentIndex = playerCurrentWeapons.IndexOf(type);

        //Decrease / Clamp
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = playerCurrentWeapons.Count - 1;

        //Get type with new index
        type = playerCurrentWeapons[currentIndex];

        return true;
    }
}
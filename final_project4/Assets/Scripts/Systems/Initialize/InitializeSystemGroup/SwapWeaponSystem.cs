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
    private int playerWeaponCount;
    private float swapWeaponTimer;
    private float delaySwapWeapon = 0.02f;    //TODO CHOOSE A GOOD TIMER DELAY

    protected override void OnCreate()
    {
        playerWeaponCount = GameVariables.Player.PlayerWeaponEntities.Count;
        // gunEnumLength = Enum.GetNames(typeof(WeaponType)).Length;
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
        if (inputs.SwapWeapon1)
            TryGetWeaponType(out weaponDesired, WeaponType.Pistol);
        else if (inputs.SwapWeapon2)
            TryGetWeaponType(out weaponDesired, WeaponType.Shotgun);
        else if (inputs.SwapWeapon3)
            TryGetWeaponType(out weaponDesired, WeaponType.Machinegun);
        else if (inputs.MouseWheel.y > 0)
            GetPreviousWeapon(out weaponDesired);
        else if (inputs.MouseWheel.y < 0)
            GetNextWeapon(out weaponDesired);

        //Make sure weapon actually swapped
        if(weaponDesired == GameVariables.Player.CurrentWeaponHeld)
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
        Entity currentWeaponEntity = GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld];
        Entity desiredWeaponEntity = GameVariables.Player.PlayerWeaponEntities[type];

        EntityManager.SetEnabled(currentWeaponEntity, false);
        EntityManager.SetEnabled(desiredWeaponEntity, true);
        
        //Set Cooldown to shoot after swap
        GunComponent gun = EntityManager.GetComponentData<GunComponent>(desiredWeaponEntity);
        gun.SwapTimer = gun.OnSwapDelayToShoot;
        EntityManager.SetComponentData(desiredWeaponEntity, gun);

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
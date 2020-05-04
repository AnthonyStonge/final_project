using System;
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

        //TODO Change order, should check first if player want to change gun, then query the array
        var weapontypes = GameVariables.Player.PlayerWeaponTypes;
        int index = weapontypes.IndexOf(GameVariables.Player.CurrentWeaponHeld);
        
        if (inputs.MouseWheel.y > 0)
            index++;
        else if (inputs.MouseWheel.y < 0)
            index--;
        //If player doesn't want to change weapon
        else return;
        
        if (index < 0)
        {
            index = weapontypes.Count - 1;
        }
        else if (index >= weapontypes.Count)
        {
            index = 0;
        }
        SwapWeapon(weapontypes[index]);
    }

    private void SwapWeapon(WeaponType type)
    {
        swapWeaponTimer = delaySwapWeapon;

        //Add event to NativeList
        EventsHolder.WeaponEvents.Add(new WeaponInfo
        {
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
}
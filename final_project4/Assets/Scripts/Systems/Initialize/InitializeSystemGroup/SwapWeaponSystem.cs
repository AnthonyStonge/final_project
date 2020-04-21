using System;
using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class SwapWeaponSystem : SystemBase
{
    private int gunEnumLength;

    protected override void OnCreate()
    {
        gunEnumLength = Enum.GetNames(typeof(WeaponType)).Length;
    }

    protected override void OnUpdate()
    {
        //Get player inputs
        InputComponent inputs = EntityManager.GetComponentData<InputComponent>(GameVariables.Player.Entity);

        if (!inputs.Enabled)
            return;

        //Number > mouse wheel (override)
        if (inputs.WeaponTypeDesired != GameVariables.Player.CurrentWeaponHeld)
        {
            SwapWeapon(inputs.WeaponTypeDesired);
        }
        else if (inputs.MouseWheel.y > 0)
        {
            //Get next weapon
            WeaponType desired = (int) GameVariables.Player.CurrentWeaponHeld + 1 > gunEnumLength - 1
                ? 0
                : GameVariables.Player.CurrentWeaponHeld + 1;

            SwapWeapon(desired);
        }
        else if (inputs.MouseWheel.y < 0)
        {
            //Get previous weapon
            WeaponType desired = (int) GameVariables.Player.CurrentWeaponHeld - 1 < 0
                ? (WeaponType) gunEnumLength - 1
                : GameVariables.Player.CurrentWeaponHeld - 1;

            SwapWeapon(desired);
        }
    }

    private void SwapWeapon(WeaponType type)
    {
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

        //Set CurrentGunType
        GameVariables.Player.CurrentWeaponHeld = type;
    }
}
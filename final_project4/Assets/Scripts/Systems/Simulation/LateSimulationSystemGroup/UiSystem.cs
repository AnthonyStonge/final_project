using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
[DisableAutoCreation]
public class UiSystem : SystemBase
{
    private GunComponent gunComponent;

    protected override void OnUpdate()
    {
        if (!GameVariables.Ui.GunName) return;
        if(GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]);

        switch (gunComponent.WeaponType)
        {
            //TODO Worst logic ever made in my life, I'm dying - Marc-Antoine GIrard
            
            case WeaponType.Pistol:
                GameVariables.Ui.ShotgunImage.enabled = false;
                GameVariables.Ui.PistolImage.enabled = true;
                
                break;
            case WeaponType.Shotgun:
                GameVariables.Ui.ShotgunImage.enabled = true;
                GameVariables.Ui.PistolImage.enabled = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        GameVariables.Ui.NbBulletInMagazine.text = "" + gunComponent.CurrentAmountBulletInMagazine;
        GameVariables.Ui.NbBulletOnPlayer.text = "" + gunComponent.CurrentAmountBulletOnPlayer;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
[DisableAutoCreation]
public class UISystem : SystemBase
{
    private GunComponent gunComponent;

    protected override void OnUpdate()
    {
        if (!GameVariables.UI.GunName) return;
        if(GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]);

        switch (gunComponent.WeaponType)
        {
            //TODO Worst logic ever made in my life, I'm dying - Marc-Antoine GIrard
            
            case WeaponType.Pistol:
                GameVariables.UI.ShotgunImage.enabled = false;
                GameVariables.UI.PistolImage.enabled = true;
                
                break;
            case WeaponType.Shotgun:
                GameVariables.UI.ShotgunImage.enabled = true;
                GameVariables.UI.PistolImage.enabled = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        GameVariables.UI.NbBulletInMagazine.text = gunComponent.CurrentAmountBulletInMagazine.ToString();
        GameVariables.UI.NbBulletOnPlayer.text = gunComponent.CurrentAmountBulletOnPlayer.ToString();
    }
}

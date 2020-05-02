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
    private LifeData lifeData;

    protected override void OnUpdate()
    {
        
        if (!GameVariables.UI.GunName) return;
        
        if(GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]);
        lifeData = EntityManager.GetComponentData<LifeData>(GameVariables.Player.Entity);


        switch (gunComponent.WeaponType)
        {
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
        GameVariables.UI.lifeRect.sizeDelta = new Vector2(PlayerUiWidth(PlayerLifePourcent(lifeData),  GameVariables.UI.lifeBgRect.rect.size.x), GameVariables.UI.lifeBgRect.sizeDelta.y);

    }
    private static float PlayerLifePourcent(LifeData lifeData)
    {
        return (lifeData.Value.Value * 100) / lifeData.Value.Max;
    }
    private static float PlayerUiWidth(float pourcentOfLife, float width)
    {
        return (width * pourcentOfLife) / 100;
    }
}

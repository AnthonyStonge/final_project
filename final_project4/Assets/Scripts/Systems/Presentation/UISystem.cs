using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;
[DisableAutoCreation]
public class UISystem : SystemBase
{
    private GunComponent gunComponent;
    private LifeComponent lifeComponent;

    protected override void OnUpdate()
    {
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            if(info.Parent != GameVariables.Player.Entity)
                continue;

            switch (info.EventType)
            {
                case WeaponInfo.WeaponEventType.ON_SHOOT:
                    UIManager.OnShoot();
                    break;
                case WeaponInfo.WeaponEventType.ON_RELOAD:
                    Debug.Log($"Reload with {info.AmountBulletsInMagazine}");
                    UIManager.OnReload(info.AmountBulletsInMagazine);
                    break;
                case WeaponInfo.WeaponEventType.ON_SWAP:
                    UIManager.SetWeaponType(info.WeaponType);
                    break;
            }
        }

        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            if (info.HitEntity != GameVariables.Player.Entity)
                return;
            
            UIManager.OnPlayerHit();
        }
        
        
        if (!GameVariables.UI.GunName) return;
        
        if(GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]);
        lifeComponent = EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity);


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
        }
        GameVariables.UI.NbBulletInMagazine.text = gunComponent.CurrentAmountBulletInMagazine.ToString();
        GameVariables.UI.NbBulletOnPlayer.text = gunComponent.CurrentAmountBulletOnPlayer.ToString();
        GameVariables.UI.lifeRect.sizeDelta = new Vector2(PlayerUiWidth(PlayerLifePourcent(lifeComponent),  GameVariables.UI.lifeBgRect.rect.size.x), GameVariables.UI.lifeBgRect.sizeDelta.y);

    }
    private static float PlayerLifePourcent(LifeComponent lifeComponent)
    {
        return (lifeComponent.Life.Value * 100) / lifeComponent.Life.Max;
    }
    private static float PlayerUiWidth(float pourcentOfLife, float width)
    {
        return (width * pourcentOfLife) / 100;
    }
}

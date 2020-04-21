using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class UiSystem : SystemBase
{
    private GunComponent gunComponent;
    protected override void OnCreate()
    {
        
    }
    protected override void OnUpdate()
    {
        if(GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]);
        GameVariables.Ui.GunName.text = "Gun: " + gunComponent.WeaponType;
        GameVariables.Ui.NbBulletInMagazine.text = "InMagazin: " + gunComponent.CurrentAmountBulletInMagazine;
        GameVariables.Ui.NbBulletOnPlayer.text = "OnPlayer: " + gunComponent.CurrentAmountBulletOnPlayer;
    }
}

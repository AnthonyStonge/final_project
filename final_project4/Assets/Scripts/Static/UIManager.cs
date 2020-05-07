using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    private static Dictionary<WeaponType, UI_WeaponLink> UI_Weapons =
        new Dictionary<WeaponType, UI_WeaponLink>();

    private static WeaponType CurrentWeaponTypeHeld = WeaponType.Pistol;

    private static bool DontUpdateUI;
    private static bool DisplayInfiniteText;

    public static void Initialize()
    {
        foreach (UI_WeaponLink link in MonoGameVariables.Instance.WeaponLinks)
        {
            UI_Weapons.Add(link.Type, link);
        }

        //Make sure every weapon are off
        foreach (UI_WeaponLink link in UI_Weapons.Values)
        {
            link.WeaponImg.SetActive(false);
            link.BulletsConainter.SetActive(false);
            foreach (Image image in link.BulletsImages)
            {
                image.gameObject.SetActive(true);
            }
        }

        //Make sure texts are off
        MonoGameVariables.Instance.BulletsNormalText.gameObject.SetActive(false);
        MonoGameVariables.Instance.BulletsInfiniteText.gameObject.SetActive(false);

        //Set Hearth Index at number of hearth
        MonoGameVariables.Instance.Hearths.HearthIndexAt =
            (ushort) (MonoGameVariables.Instance.Hearths.HearthImages.Count - 1);
        
        //Make sure Hell timers are off
        MonoGameVariables.Instance.Hell_Timer01.gameObject.SetActive(false);
        MonoGameVariables.Instance.Hell_Timer02.gameObject.SetActive(false);
    }

    public static void OnDestroy()
    {
    }

    public static void SetWeaponType(WeaponType type)
    {
        //Make sure UI has an image for that
        if (!UI_Weapons.ContainsKey(type))
        {
#if UNITY_EDITOR
            Debug.LogError($"No UI for Weapon {type}");
#endif
            DontUpdateUI = true;
            return;
        }

        DontUpdateUI = false;

        if (UI_Weapons.ContainsKey(CurrentWeaponTypeHeld))
        {
            //Toggle off Weapon image
            UI_Weapons[CurrentWeaponTypeHeld].WeaponImg.SetActive(false);

            //Toggle off BulletsContainer
            UI_Weapons[CurrentWeaponTypeHeld].BulletsConainter.SetActive(false);
        }

        //Toggle on new Weapon image
        UI_Weapons[type].WeaponImg.SetActive(true);

        //Toggle on new BulletsContainer
        UI_Weapons[type].BulletsConainter.SetActive(true);

        //Change text
        RefreshBulletsText();

        CurrentWeaponTypeHeld = type;
    }

    public static void OnShoot()
    {
        if (DontUpdateUI)
            return;

        //Get IndexAt
        ushort index = UI_Weapons[CurrentWeaponTypeHeld].BulletIndexAt;

        //Make sure there are still bullets to turn off
        if (index >= UI_Weapons[CurrentWeaponTypeHeld].BulletsImages.Count)
            return;

        //Get Bullet Image color
        Color c = UI_Weapons[CurrentWeaponTypeHeld].BulletsImages[index].color;

        //Set new color
        c.a = 0.4f;
        UI_Weapons[CurrentWeaponTypeHeld].BulletsImages[index].color = c;

        UI_Weapons[CurrentWeaponTypeHeld].BulletIndexAt++;
    }

    public static void OnReload()
    {
        if (DontUpdateUI)
            return;

        //Get IndexAt
        ushort index = UI_Weapons[CurrentWeaponTypeHeld].BulletIndexAt;

        //Make sure there are still bullets to reload
        if (index == 0)
            return;

        if (index >= UI_Weapons[CurrentWeaponTypeHeld].BulletsImages.Count)
            index = (ushort) (UI_Weapons[CurrentWeaponTypeHeld].BulletsImages.Count - 1);

        for (int i = index; i >= 0; i--)
        {
            //Get Bullet Image color
            Color c = UI_Weapons[CurrentWeaponTypeHeld].BulletsImages[i].color;

            //Set new color
            c.a = 1;
            UI_Weapons[CurrentWeaponTypeHeld].BulletsImages[i].color = c;
        }

        UI_Weapons[CurrentWeaponTypeHeld].BulletIndexAt = 0;

        //Change text
        RefreshBulletsText();
    }

    //TODO REDUCE COST OF FUNCTION LOL
    private static void RefreshBulletsText()
    {
        if (DontUpdateUI)
            return;
        
        //Get Player Current GunComponent
        Entity currentWeaponEntity = EventsHolder.LevelEvents.CurrentLevel != MapType.Level_Hell
            ? GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]
            : GameVariables.Player.PlayerHellWeaponEntities[GameVariables.Player.CurrentWeaponHeld];
        GunComponent gun =
            World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<GunComponent>(currentWeaponEntity);

        //Figure out which text should be displayed
        if (gun.HasInfiniteAmmo)
        {
            if (!MonoGameVariables.Instance.BulletsInfiniteText.gameObject.activeSelf)
                ToggleText(true);
            return;
        }

        if (!MonoGameVariables.Instance.BulletsNormalText.gameObject.activeSelf)
            ToggleText(false);

        MonoGameVariables.Instance.BulletsNormalText.text =
            $"{gun.CurrentAmountBulletOnPlayer}/{gun.MaxBulletOnPlayer}";
    }

    private static void ToggleText(bool isWeaponInfiniteAmmo)
    {
        MonoGameVariables.Instance.BulletsInfiniteText.gameObject.SetActive(isWeaponInfiniteAmmo);
        MonoGameVariables.Instance.BulletsNormalText.gameObject.SetActive(!isWeaponInfiniteAmmo);
    }

    public static void RefreshPlayerHp(bool hasLifeDecreased)
    {
        //Get player LifeComponent
        LifeComponent life =
            World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LifeComponent>(GameVariables.Player
                .Entity);

        ushort index = (ushort) life.Life.Value;

        //Make sure life at exists in images
        if (index >= MonoGameVariables.Instance.Hearths.HearthImages.Count)
        {
#if UNITY_EDITOR
            Debug.Log($"Index {index} doesnt exists... Put more hearth in the UI");
#endif
            return;
        }

        //Get color
        Color c = MonoGameVariables.Instance.Hearths.HearthImages[index].color;

        //Set new color
        if (hasLifeDecreased)
            c.a = 0.15f;
        else
            c.a = 1;
        MonoGameVariables.Instance.Hearths.HearthImages[index].color = c;
    }

    public static void OnPlayerHit()
    {
        RefreshPlayerHp(true);
    }

    public static void OnPlayerPickupHealth()
    {
        RefreshPlayerHp(false);
    }

    public static void ResetPlayerHealth()
    {
        for (int i = 0; i < MonoGameVariables.Instance.Hearths.HearthImages.Count; i++)
        {
            Color c = MonoGameVariables.Instance.Hearths.HearthImages[i].color;
            c.a = 1;
            MonoGameVariables.Instance.Hearths.HearthImages[i].color = c;
        }

        MonoGameVariables.Instance.Hearths.HearthIndexAt =
            (ushort) (MonoGameVariables.Instance.Hearths.HearthImages.Count - 1);
    }

    public static void ToggleHellTimers(bool activate)
    {
        MonoGameVariables.Instance.Hell_Timer01.gameObject.SetActive(activate);
        MonoGameVariables.Instance.Hell_Timer02.gameObject.SetActive(activate);
    }

    public static void SetTimeOnHellTimers(float time)
    {
        TimeSpan timeFormat = TimeSpan.FromSeconds(time);
        string s = timeFormat.ToString("ss'.'ff");
        
        //Set timers text
        MonoGameVariables.Instance.Hell_Timer01.text = s;
        MonoGameVariables.Instance.Hell_Timer02.text = s;
    }
}
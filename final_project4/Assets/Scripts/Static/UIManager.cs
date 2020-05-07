using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public static class UIManager
{
    private static Dictionary<WeaponType, UI_WeaponLink> UI_Weapons =
        new Dictionary<WeaponType, UI_WeaponLink>();

    private static WeaponType CurrentWeaponTypeHeld = WeaponType.Pistol;

    private static bool DontUpdateUI;

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
    }

    public static void OnPlayerHit()
    {
    }

    public static void OnPlayerPickupHealth()
    {
        //Toggle on a hearth image
    }

    public static void OnPlayerPickupAmmo()
    {
        //Increase CurrentAmmo text
    }

    public static void OnLoadHellLevel()
    {
        //Toggle timer on

        //Toggle infinite ammo
    }

    public static void OnUnloadHellLevel()
    {
        //Toggle off hell UI
    }
}
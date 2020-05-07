using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Enums;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class MonoGameVariables : MonoBehaviour
{
    #region Singleton

    public static MonoGameVariables Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public Transform CameraTransform;

    [Header("UI")] 
    public List<UI_WeaponLink> WeaponLinks;
    public UI_Health Hearths;
    public TextMeshProUGUI BulletsNormalText;
    public TextMeshProUGUI BulletsInfiniteText;

}

[Serializable]
public class UI_WeaponLink
{
    public WeaponType Type;
    public GameObject WeaponImg;

    public GameObject BulletsConainter;
    public List<Image> BulletsImages;

    [HideInInspector] public ushort BulletIndexAt;
}

[Serializable]
public class UI_Health
{
    public List<Image> HearthImages;
    [HideInInspector] public ushort HearthIndexAt;
}
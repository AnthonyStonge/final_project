using System;
using System.Collections.Generic;
using Enums;
using TMPro;
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

    public TextMeshPro Hell_Timer01;
    public TextMeshPro Hell_Timer02;
    public TextMeshPro Hell_ExplainText;
    public TextMeshPro Hell_WaitingTimer;
    public TextMeshPro Hell_WinText;

}

[Serializable]
public class UI_WeaponLink
{
    public WeaponType Type;

    public GameObject BulletsConainter;

    public Material Lit;
    public Material UnLit;
    public List<Image> BulletsImages;

    [HideInInspector] public ushort BulletIndexAt;
}

[Serializable]
public class UI_Health
{
    public Material Lit;
    public Material UnLit;
    public List<Image> HearthImages;
    [HideInInspector] public ushort HearthIndexAt;
}
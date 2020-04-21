using System;
using Enums;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct GunComponent : IComponentData
{
    [Header("Variables")]
    public WeaponType WeaponType;
    
    [Space(5)]
    public int MaxBulletInMagazine;
    public int MaxBulletOnPlayer;

    [Space(3)] 
    public float ResetBetweenShotTime;
    public float ResetReloadTime;

    [Header("Internal Variables")]
    public Entity BulletPrefab;

    [Header("Debug Variables")]
    [Tooltip("The amount of bullet in the magazine when the entity spawns")]
    public int CurrentAmountBulletInMagazine;
    [Tooltip("The amount of bullet on the entity when it spawns")]
    public int CurrentAmountBulletOnPlayer;
    
    [HideInInspector] public float BetweenShotTime;
    [HideInInspector] public float ReloadTime;
    
    [HideInInspector] public bool IsBetweenShot => BetweenShotTime > 0;
    [HideInInspector] public bool IsReloading => ReloadTime > 0;
}

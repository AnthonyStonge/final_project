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
    [Tooltip("Must be above 1 frame so that ParentSystem can update its position")]
    [Range(0.02f, 100f)]public float OnSwapDelayToShoot;
    
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
    [HideInInspector] public float SwapTimer;
    
    [HideInInspector] public bool IsBetweenShot => BetweenShotTime > 0;
    [HideInInspector] public bool IsReloading => ReloadTime > 0;
}

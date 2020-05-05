using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;
using Object = UnityEngine.Object;
public struct Effect
{
    public VisualEffect VisualEffect;
    public int MaxAmount;
}

public static class VisualEffectHolder
{
    
    public static Dictionary<int, Effect> Effects;
    public static Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, int>> WeaponEffects;
    public static Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, int>> BulletEffects;

    public static int PropertyTexturePosition;
    public static int PropertyTextureRotation;
    public static int PropertyCount;
    
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;
    
    public static void Initialize()
    {
        numberOfAssetsToLoad = 1;
        currentNumberOfLoadedAssets = 0;

        //Initialize Dictionnary
        Effects = new Dictionary<int, Effect>();
        WeaponEffects = new Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, int>>();
        BulletEffects = new Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, int>>();
        
        //Weapons
        WeaponEffects = new Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, int>>();
        for (int i = 0; i < Enum.GetNames(typeof(WeaponType)).Length; i++)
        {
            WeaponEffects.Add((WeaponType) i, new Dictionary<WeaponInfo.WeaponEventType, int>());
        }

        //Bullets
        BulletEffects = new Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, int>>();
        for (int i = 0; i < Enum.GetNames(typeof(ProjectileType)).Length; i++)
        {
            BulletEffects.Add((ProjectileType) i, new Dictionary<BulletInfo.BulletCollisionType, int>());
        }
        PropertyTexturePosition = Shader.PropertyToID("positions");
        PropertyTextureRotation = Shader.PropertyToID("rotations");
        PropertyCount = Shader.PropertyToID("count");
        
    }

    public static void LoadAssets()
    {
        Addressables.LoadAssetAsync<EffectsContainer>("EffectsContainer").Completed += obj =>
        {
            ExtractDataFromContainer(obj.Result);
            currentNumberOfLoadedAssets++;
        };
    }
    
    
    private static void ExtractDataFromContainer(EffectsContainer container)
    {
        int nextEffectID = 0;

        //Instatiate VFX Parent
        GameObject parent = new GameObject("VFX Container");
        
        foreach (EffectLinks links in container.Links)
        {
            // Instantiate VFX in Parent GameObject
            var effect = Object.Instantiate(links.Effect, parent.transform);
            
            //Add VFX to dictionary
            Effects.Add(nextEffectID, new Effect
            {
                VisualEffect = effect,
                MaxAmount = links.MaxAmount
            });

            //Weapons
            foreach (EffectLinks.WeaponLinks weapon in links.Weapons)
            {
                //Add to weapon dictionary
                if (!WeaponEffects[weapon.WeaponType].ContainsKey(weapon.EventType))
                    WeaponEffects[weapon.WeaponType].Add(weapon.EventType, nextEffectID);
                
#if UNITY_EDITOR
                else Debug.LogWarning($"You tried to add multiple Visual effects for {weapon.WeaponType} {weapon.EventType} action. \n" +
                                   $" Current Effect: {Effects[WeaponEffects[weapon.WeaponType][weapon.EventType]].VisualEffect.name} \n" +
                                   $"Desired Effect: {links.Effect.name} \n");
#endif
            }

            //Bullets
            foreach (EffectLinks.BulletLinks bullet in links.Bullets)
            {
                //Add to bullet dictionary
                if (!BulletEffects[bullet.BulletType].ContainsKey(bullet.CollisionType))
                    BulletEffects[bullet.BulletType].Add(bullet.CollisionType, nextEffectID);
#if UNITY_EDITOR
                else Debug.LogWarning($"You tried to add multiple Visual effects for {bullet.BulletType} {bullet.CollisionType} action. \n" +
                                    $" Current Effect: {Effects[BulletEffects[bullet.BulletType][bullet.CollisionType]].VisualEffect.name} \n" +
                                    $"Desired Effect: {links.Effect.name} \n");
#endif
            }
            //Increment ID
            nextEffectID++;
        }
    }
    
    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

public static class VisualEffectHolder
{

    private static string[] ScriptableVFXName =
    {
        "PistolVFX"
    };

    public static ConcurrentDictionary<ProjectileType, VisualEffect> ProjectileVFXDict;
    
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;
    
    public static void Initialize()
    {
        numberOfAssetsToLoad = ScriptableVFXName.Length;
        currentNumberOfLoadedAssets = 0;
        
        ProjectileVFXDict = new ConcurrentDictionary<ProjectileType, VisualEffect>();
    }

    public static void LoadAssets()
    {
        foreach (var name in ScriptableVFXName)
        {
            Addressables.LoadAssetAsync<BulletVFXScriptable>(name).Completed += obj =>
            {
                var script = obj.Result;
                ProjectileVFXDict.TryAdd(script.WeaponType, script.VFX);
                currentNumberOfLoadedAssets++;
            };
        }
    }
    
    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }
}

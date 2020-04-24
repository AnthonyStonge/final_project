using System.Collections.Concurrent;
using Enums;
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

    public static int PropertyTexture;
    public static int PropertyCount;
    
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;
    
    public static void Initialize()
    {
        numberOfAssetsToLoad = ScriptableVFXName.Length;
        currentNumberOfLoadedAssets = 0;

        PropertyTexture = Shader.PropertyToID("positions");
        PropertyCount = Shader.PropertyToID("count");
        
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

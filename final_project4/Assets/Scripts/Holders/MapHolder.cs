using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapInfo
{
    public float3 SpawnPosition;
    public Material NeonMat;

    public Dictionary<ushort, Portal> Portals = new Dictionary<ushort, Portal>();
    
    public struct Portal
    {
        //Personal info
        public ushort Id;
        public float3 Position;
        public quaternion Rotation;
        
        //When going into, where is it going
        public MapType MapTypeLeadingTo;
        public ushort PortalIdLeadingTo;
    }
}

public static class MapHolder
{
    public static ConcurrentDictionary<MapType, Entity> MapPrefabDict = 
        new ConcurrentDictionary<MapType, Entity>();

    public static Dictionary<MapType, MapInfo> MapsInfo =
        new Dictionary<MapType, MapInfo>();
    public static List<BlobAssetStore> BloblAssetList = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        currentNumberOfLoadedAssets = 0;
        foreach (var i in Enum.GetNames(typeof(MapType)))
        {
            numberOfAssetsToLoad++;
        }
    }

    public static void LoadAssets()
    {
        Addressables.LoadAssetAsync<MapContainerScriptable>("MapContainer").Completed += obj =>
        {
            MapContainerScriptable container = obj.Result;

            foreach (MapContainerScriptable.MapObjectLinks map in container.Links)
            {
                Entity mapEntity = ECSUtility.ConvertGameObjectPrefab(map.Prefab, out BlobAssetStore blob);
                if (blob != null)
                {
                    BloblAssetList.Add(blob);
                }
               
                MapPrefabDict.TryAdd(map.Type, mapEntity);
                currentNumberOfLoadedAssets++;
            }
        };
    }

    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }

    public static void OnDestroy()
    {
        BloblAssetList.ForEach(i => { i.Dispose(); });
    }
}
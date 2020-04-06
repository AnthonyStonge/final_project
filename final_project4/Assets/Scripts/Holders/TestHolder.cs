using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.ResourceManagement.AsyncOperations;


public static class TestHolder
{
    public static List<Mesh> meshes;
    public static Material mat;
    
    private static int counter;
    
    //private static Dictionary<Type, Dictionary<string, object>>  = new Dictionary<Type, Dictionary<string, object>>();
    
    
    public static void LoadAssets()
    {
        meshes = new List<Mesh>();
        Addressables.LoadAssetAsync<Material>("ChickMat").Completed += onLoadDoneMat;
        Addressables.LoadAssetAsync<Mesh>("Chick0").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick1").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick2").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick3").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick4").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick5").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick6").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick7").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick8").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick9").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick10").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick11").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick12").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick13").Completed += onLoadDone;
        Addressables.LoadAssetAsync<Mesh>("Chick14").Completed += onLoadDone;
    }

    public static void LoadAsset<T>(string key)
    { 
        Addressables.LoadAssetAsync<T>(key).CompletedTypeless += (obj) =>
        {
            OnTypelessLoadDone<T>(obj, key);
        };
    }

    private static void OnTypelessLoadDone<T>(AsyncOperationHandle obj, string key)
    {
        
    }
    
    private static void onLoadDone(AsyncOperationHandle<Mesh> obj)
    {
        meshes.Add(obj.Result);
        counter++;
        if (counter == 16)
        {
            StartFlowSystem();
        }
    } 
    
    private static void onLoadDoneMat(AsyncOperationHandle<Material> obj)
    {
       mat = obj.Result;
       counter++;
       if (counter == 16)
       {
           StartFlowSystem();
       }
    }

    private static void StartFlowSystem()
    {
        var presentation = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PresentationSystemGroup>();
        var fs = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<FlowSystem>();
        presentation.AddSystemToUpdateList(fs);
    }
}

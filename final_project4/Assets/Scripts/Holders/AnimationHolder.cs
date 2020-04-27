using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public static class AnimationHolder
{
    public static Dictionary<Animation.AnimationType, Mesh[]> AnimationFrames;
    public static Dictionary<Animation.AnimationType, Material> MeshMaterials;
    public static NativeList<int> AnimationsLength; //Access with Animation.AnimationType to get a value
    
    //Animation Batch Dictionary
    public static List<int> AnimatedGroupsLength;
    

    private static int currentNumberOfLoadedAssets = 0;
    private static int numberOfAssetsToLoad = 1;


    public static void Initialize()
    {
        AnimationFrames = new Dictionary<Animation.AnimationType, Mesh[]>();
        MeshMaterials = new Dictionary<Animation.AnimationType, Material>();
        AnimationsLength = new NativeList<int>(Allocator.Persistent);
        AnimatedGroupsLength = new List<int>();
        
        //Init all groups for 0
        for (int i = 0; i < 15; i++)
        {
            AnimatedGroupsLength.Add(0);
        }

        //TODO STESS TEST
        BlobAssetStore blob;
        Entity prefab = ECSUtility.ConvertGameObjectPrefab(MonoGameVariables.Instance.prefab, out blob);
        blob.Dispose();

        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

        float3 position = float3.zero;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                for (int k = 0; k < 100; k++)
                {
                    //Create Entity
                    Entity e = manager.Instantiate(prefab);

                    //Set Position
                    manager.SetComponentData(e, new Translation
                    {
                        Value = position
                    });
                    manager.AddSharedComponentData(e, new AnimationBatch
                    {
                        BatchId = AddAnimatedObject()
                    });

                    position.z += 3;
                }

                position.z = 0;
                position.y += 3;
            }

            position.y = 0;
            position.x += 3;
        }
    }

    public static void OnDestroy()
    {
        AnimationsLength.Dispose();
    }

    public static void LoadAssets()
    {
        Addressables.LoadAssetAsync<AnimationsContainer>("AnimationsContainer").Completed += handle =>
        {
            ExtractDataFromContainer(handle.Result);
            ExtractAnimationsLength();
            currentNumberOfLoadedAssets++;
        };
    }

    private static void ExtractDataFromContainer(AnimationsContainer container)
    {
        foreach (Animation animation in container.Animations)
        {
            //TODO MAKE SURE THERES NO DUPLICATES
            AnimationFrames.Add(animation.Type, animation.Frames.ToArray());
            MeshMaterials.Add(animation.Type, animation.Material);
        }
    }

    private static void ExtractAnimationsLength()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Animation.AnimationType)).Length; i++)
        {
            if (AnimationFrames.TryGetValue((Animation.AnimationType) i, out Mesh[] frames))
            {
                AnimationsLength.Add(frames.Length);
            }
        }
    }

    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }

    public static int AddAnimatedObject()
    {
        //Get groups with the least obj
        int indexSmallest = 0;
        for (int i = 1; i < AnimatedGroupsLength.Count - 1; i++)
        {
            if (AnimatedGroupsLength[i] < AnimatedGroupsLength[indexSmallest])
                indexSmallest = i;
        }
        
        //Increment number
        AnimatedGroupsLength[indexSmallest]++;

        return indexSmallest;
    }
    
    //TODO IMPLEMENT ADD BY BATCH (LIKE 10-50)
}
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Type = Enums.Type;

public static class AnimationHolder
{
    public struct Animation
    {
        public Mesh[] Frames;
        public Material Material;
    }

    public static ConcurrentDictionary<Type, Dictionary<State, Animation>> Animations =
        new ConcurrentDictionary<Type, Dictionary<State, Animation>>();

    //Animation Batch Dictionary
    public static List<int> AnimatedGroupsLength;

    private static int currentNumberOfLoadedAssets = 0;
    private static int numberOfAssetsToLoad = 1;


    public static void Initialize()
    {
        AnimatedGroupsLength = new List<int>();

        //Init all groups for 0
        for (int i = 0; i < 100; i++)
        {
            AnimatedGroupsLength.Add(0);
        }
    }

    public static void LoadAssets()
    {
        Addressables.LoadAssetAsync<AnimationsContainer>("AnimationsContainer").Completed += handle =>
        {
            ExtractDataFromContainer(handle.Result);
            currentNumberOfLoadedAssets++;
        };
    }

    private static void ExtractDataFromContainer(AnimationsContainer container)
    {
        //TODO MAKE SURE THERES NO DUPLICATES
        foreach (AnimationScriptableObject animation in container.Animations)
        {
            Animations.TryAdd(animation.Type, new Dictionary<State, Animation>());
            if (Animations[animation.Type].ContainsKey(animation.State))
            {
                Debug.LogWarning($"Duplicate Animation state {animation.Type}. Check AnimationContainer");
                continue;
            }
            Animations[animation.Type].Add(animation.State, new Animation
            {
                Frames = animation.Frames.ToArray(),
                Material = animation.Material
            });
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
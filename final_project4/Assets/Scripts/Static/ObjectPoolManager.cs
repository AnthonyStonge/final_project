using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ObjectPoolManager
{
    private static Dictionary<Type, object> dict = new Dictionary<Type, object>();

    //Unsafe
    public static void Add<T>(ObjectPool<T> value)
    {
        dict.Add(typeof(T), value);
    }

    //Unsafe
    public static void Put<T>(ObjectPool<T> value)
    {
        dict[typeof(T)] = value;
    }

    //Unsafe
    public static ObjectPool<T> Get<T>()
    {
        return (ObjectPool<T>) dict[typeof(T)];
    }

    public static bool TryGet<T>(out ObjectPool<T> value)
    {
        object tmp;
        if (dict.TryGetValue(typeof(T), out tmp))
        {
            value = (ObjectPool<T>) tmp;
            return true;
        }

        value = default;
        return false;
    }

    public static void CreateNewPool<T>(int amountToSpawn, int maxAmountInPool, Func<T> func)
    {
        if (dict.ContainsKey(typeof(T)))
        {
            Debug.LogError("Can't create Object Pool for this type. Already exist");
            return;
        }

        ObjectPool<T> newPool = new ObjectPool<T>(amountToSpawn, maxAmountInPool, func);
        Add(newPool);
    }

    public static void DestroyAllPool()
    {
        dict.Clear();
    }

    public static T GetPooledObject<T>()
    {
        if (dict.ContainsKey(typeof(T)))
        {
            return ((ObjectPool<T>) dict[typeof(T)]).GetObjectFromPool();
        }

        return default;
    }

    public static void AddBackToPool<T>(T value)
    {
        if (dict.ContainsKey(typeof(T)))
        {
             ((ObjectPool<T>) dict[typeof(T)]).AddBackToPool(value);
        }
    }
}
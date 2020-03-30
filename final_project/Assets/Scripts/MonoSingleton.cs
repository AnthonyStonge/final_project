using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static T instance { get; private set; }
    protected bool toDestroy = false;

    protected void Awake()
    {
        if (instance != null)
        {
            toDestroy = true;
            DestroyImmediate(this);
        }
        else
        {
            instance = (T) this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

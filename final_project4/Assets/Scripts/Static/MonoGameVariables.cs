using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class MonoGameVariables : MonoBehaviour
{
    #region Singleton

    public static MonoGameVariables Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public Transform CameraTransform;
}
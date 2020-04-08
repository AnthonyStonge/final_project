using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoGameVariables : MonoBehaviour
{
    #region Singleton

    public static MonoGameVariables instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    public Mesh PlayerMesh;
    public Material playerMaterial;
}
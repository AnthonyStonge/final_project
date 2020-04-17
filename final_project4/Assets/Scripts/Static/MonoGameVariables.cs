using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    [Header("Player")]
    public PlayerAssetsScriptableObject playerAssets;
    public DashScriptableObject playerDashAssets;
    public PistolScriptableObject playerPistolAssets;
    public AudioSource playerAudioSource;

    [Header("Bullets")] 

    public GameObject pistolBullet;
    [Header("Extra")]
    public GameObject Player;

    public Mesh PistolMesh;
    public Material PistolMaterial;

    public Mesh BulletMesh;
    public Material BulletMaterial;

    public CinemachineTargetGroup TargetGroupCamera;
}
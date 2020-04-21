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
    
    [Header("Prefabs")]
    public GameObject Player;
    public GameObject Enemy1;

    [Header("Prefabs Weapons")]
    public GameObject Pistol;
    public GameObject Shotgun;

    [Header("Prefabs Bullets")] 
    public GameObject PistolBullet;
    public GameObject ShotgunBullet;

    [Header("Extra")] public AudioClip temporaryShotgunShotSound;

    public Mesh PistolMesh;
    public Material PistolMaterial;

    public Mesh BulletMesh;
    public Material BulletMaterial;

    public CinemachineTargetGroup TargetGroupCamera;
}
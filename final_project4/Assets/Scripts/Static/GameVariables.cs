using System.Collections.Generic;
using Cinemachine;
using Enums;
using EventStruct;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public static class GameVariables
{
    public static bool InputEnabled = true;
    public static EntityManager EntityManager;
    public static Camera MainCamera;
    public static Transform MouseToTransform;
    public static GameState StartingState = GameState.GAME;
    public static Dictionary<MapType, ScriptableGrid> Grids = new Dictionary<MapType, ScriptableGrid>();
    public static CinemachineBasicMultiChannelPerlin CamNoiseProfile;
    public static ShakeCamComponent ShakeComponent;
    
    public static float InvincibleDashTime = 1.0f;
    public static float InvincibleDeathTime = 5.0f;
    public static float InvincibleSpawnTime = 5.0f;
    public static float InvicibleHitTime = 1.5f;

    public static class Player
    {
        //Player general infos (Can change during gameplay)
        public static Entity Entity;

        public static Dictionary<WeaponType, Entity> PlayerWeaponEntities = new Dictionary<WeaponType, Entity>();
        public static List<WeaponType> PlayerWeaponTypes = new List<WeaponType>();
        public static WeaponType CurrentWeaponHeld;

        //Unity linker
        public static Transform Transform;
    }
    public static class UI
    {
        public static TextMeshProUGUI NbBulletInMagazine;
        public static TextMeshProUGUI NbBulletOnPlayer;
        public static TextMeshProUGUI GunName;
        public static Image PistolImage;
        public static Image ShotgunImage;
        public static RectTransform lifeRect;
        public static RectTransform lifeBgRect;
        public static FadeObject FadeObject;

        public static GameObject PausedMenu;
    }

    public static class Interactables
    {
        public static Interactable? CurrentInteractableSelected;
        public static Interactable? PreviousInteractableSelected;
        
        public struct Interactable
        {
            public Entity Entity;
            public InteractableObjectType ObjectType;
        }
    }
}

using System.Collections.Generic;
using Cinemachine;
using Enums;
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
    public static ScriptableGrid grid;
    public static CinemachineBasicMultiChannelPerlin CamNoiseProfile;
    public static ShakeCamComponent ShakeComponent;
    public static class Player
    {
        //Player general infos (Can change during gameplay)
        public static Entity Entity;

        public static Dictionary<WeaponType, Entity> PlayerWeaponEntities = new Dictionary<WeaponType, Entity>();
        public static WeaponType CurrentWeaponHeld;

        public static ushort AmountLife;

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
    }
}

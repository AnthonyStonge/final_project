using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public static class GameVariables
{
    public static bool InputEnabled = true;
    public static EntityManager EntityManager;
    public static Camera MainCamera;
    public static Transform MouseToTransform;

    public static GameState StartingState = GameState.INTRO;
    
    public static class PlayerVars
    {
        //Player static info
        public static PlayerAssetsScriptableObject Default;
        public static DashScriptableObject Dash;
        // public static PistolScriptableObject Pistol;
        
        //Player general infos (Can change during gameplay)
        public static Entity Entity;
        public static float3 CurrentPosition;
        public static StateActions CurrentState;
        public static float CurrentSpeed;
        public static short CurrentHealth;
        public static bool IsAlive;

        public static Dictionary<GunType, Entity> PlayerWeaponEntities = new Dictionary<GunType, Entity>();
        public static GunType CurrentWeaponHeld;

        //Unity linker
        public static Transform Transform;
    }

    public static class PistolVars
    {
        public static float3 PlayerOffset = new float3(0, 0, 0);
        public static int MagazineSize = 100;
        public static float ReloadTime = 3.33f;
        public static float BetweenShotTime = 0.03f;

        public static class Bullet
        {
            public static float Speed = 100f;
            public static float LifeTime = 2.0f;
            public static Mesh mesh;
            public static Material mat;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public static class GameVariables
{
    public static bool InputEnabled = true;

    public static Camera MainCamera;
    public static Transform MouseToTransform;

    public static EntityArchetype BulletArchetype;
    public static Mesh BulletMesh;
    public static Material BulletMat;
    
    public static class PlayerVars
    {
        //Player initialization infos
        public static float3 SpawnPosition = float3.zero;
        public static quaternion SpawnRotation = quaternion.identity;
        public static int StartingBulletAmount = 13;

        public static float Speed = 20f;
        public static short Health = 3;
        
        //Player general infos
        public static Entity Entity;
        public static float3 CurrentPosition;
        public static StateActions CurrentState;
        public static bool IsAlive;

        //Dash infos
        public static readonly float DefaultDashDistance = 5f; //Value to comeback to when dash distance changes
        public static float DashResetTime = 1f;
        
        //Unity linker
        public static Transform Transform;
    }

    public static class CameraVars
    {
        public static float3 PlayerOffset = new float3(0, 5, 0);
    }

    public static class PistolVars
    {
        public static float3 PlayerOffset = new float3(0, 0, 0);
        public static int MagazineSize = 2400;
        public static float ReloadTime = 0.5f;
        public static float BetweenShotTime = 0.01f;

        public static class Bullet
        {
            public static float Speed = 300f;
            public static float LifeTime = 2.0f;
            public static Mesh mesh;
            public static Material mat;
        }
    }
}

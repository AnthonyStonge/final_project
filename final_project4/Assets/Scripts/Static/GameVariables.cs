using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public static class GameVariables
{
    public static bool InputEnabled = true;

    public static Camera MainCamera;

    public static EntityArchetype BulletArchetype;
    public static Mesh BulletMesh;
    public static Material BulletMat;
    
    public static class PlayerVars
    {
        //Player initialization infos
        public static float3 SpawnPosition = float3.zero;
        public static quaternion SpawnRotation = quaternion.identity;

        public static float Speed = 2f;
        public static short Health = 3;
        
        //Player general infos
        public static Entity Entity;
        public static float3 CurrentPosition;
        public static StateActions CurrentState;
        public static bool IsAlive;
        
        //Dash infos
        public static readonly float DashDistanceBasic = 5f; //Value to comeback to when dash distance changes
        public static float DashResetValue = 1f;
    }
    
    

    
}

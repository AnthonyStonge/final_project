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
        public static float3 Position;
        public static float3 MousePos;
        public static bool IsDead;
    }
    
    
    //Player initialization infos
    public static float3 PlayerSpawnPosition = float3.zero;
    public static quaternion PlayerSpawnRotation = quaternion.identity;

    public static float PlayerSpeed = 2f;
    public static short PlayerHealth = 3;
    
    //Player general infos
    public static Entity PlayerEntity;
    public static float3 PlayerCurrentPosition;
}

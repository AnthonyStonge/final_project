using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Mathematics;
using UnityEngine;

namespace EventStruct
{
    //Create one per weapon that has been fired (not per bullet)
    public struct WeaponInfo
    {
        public WeaponType weaponType;
        public WeaponEventType EventType;
        
        //Transform of the weapon
        public float3 Position;
        public quaternion Rotation;
        
        //TODO CREATE FIELD FOR WEAPON SWAPPED TO???
        
        public enum WeaponEventType
        {
            NONE,
            ON_SHOOT,
            ON_RELOAD,
            ON_SWAP
        }
    }

    //Create one per bullet that collided with something
    public struct BulletInfo
    {
        public ProjectileType projectileType;
        public BulletCollisionType CollisionType;

        //Transform of the CollisionHit
        public float3 HitPosition;
        public quaternion HitRotation;

        public enum BulletCollisionType
        {
            NONE,
            ON_WALL,
            ON_PLAYER,
            ON_ENEMY
        }
    }

    public struct PlayerInfo
    {
        //Event position
        public float3 Position;
        public quaternion Rotation;
        
        public enum PlayerEventType
        {
            NONE,
            ON_SPAWN,
            ON_RESPAWN,
            ON_DIE,
            ON_TAKING_DAMAGE
        }
    }
}

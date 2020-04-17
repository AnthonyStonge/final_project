﻿using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Mathematics;
using UnityEngine;

namespace EventStruct
{
    //Create one per weapon that has been fired (not per bullet)
    public struct WeaponInfo
    {
        public GunType GunType;
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
        public BulletType BulletType;

        //Transform of the CollisionHit
        public float3 HitPosition;
        public quaternion HitRotation;
        
        //Add vfx/sound type field for effects on different surfaces
    }
}
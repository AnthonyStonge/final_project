using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Mathematics;
using UnityEngine;

namespace EventStruct
{
    public struct BulletInfo
    {
        //Transform
        public float3 Position;
        public quaternion Rotation;
        
        //Mesh
        
        //BulletType
        public BulletType Type;
    }

    public struct SoundInfo
    {
        //SoundType
        public SoundType Type;
    }
}

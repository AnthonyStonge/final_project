using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(ProjectileHitDetectionSystem))]
//[UpdateBefore(typeof(TransformSystemGroup))]
public class PostTransformGroupBarrier : EntityCommandBufferSystem
{

}

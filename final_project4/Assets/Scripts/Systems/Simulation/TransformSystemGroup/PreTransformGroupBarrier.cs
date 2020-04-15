using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

[UpdateAfter(typeof(ProjectileHitDetectionSystem))]
public class PreTransformGroupBarrier : EntityCommandBufferSystem {}

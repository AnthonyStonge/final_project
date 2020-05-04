using System.Diagnostics;
using Unity.Entities;
 using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Debug = UnityEngine.Debug;
[DisableAutoCreation]
[UpdateAfter(typeof(ProjectileHitDetectionSystem))]
 public class TranslateSystem : SystemBase
 {
     protected override void OnUpdate()
     {
         float dt = Time.DeltaTime;
         
         Entities.ForEach((ref Translation translation, ref BulletPreviousPositionData previousPositionData, in DamageProjectile projectile, in LocalToWorld localToWorld) =>
         {
             previousPositionData.Value = translation.Value;
             translation.Value += localToWorld.Forward * projectile.Speed * dt;
         }).ScheduleParallel();
     }
 }
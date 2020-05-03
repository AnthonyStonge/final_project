using System.Diagnostics;
using Unity.Entities;
 using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Debug = UnityEngine.Debug;
[DisableAutoCreation]
 public class TranslateSystem : SystemBase
 {
     protected override void OnUpdate()
     {
         float dt = Time.DeltaTime;
         
         Entities.ForEach((ref Translation translation, ref DamageProjectile projectile, in LocalToWorld localToWorld) =>
         {
             projectile.PreviousPosition = translation.Value;
             translation.Value += localToWorld.Forward * projectile.Speed * dt;
         }).ScheduleParallel();
     }
 }
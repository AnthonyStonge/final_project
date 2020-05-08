using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateAfter(typeof(ProjectileHitDetectionSystem))]
 public class TranslateSystem : SystemBase
 {
     protected override void OnUpdate()
     {
         float dt = Time.DeltaTime;
         
         Entities.ForEach((ref Translation translation, ref BulletPreviousPositionData previousPositionData,ref Rotation rotation, in DamageProjectile projectile, in LocalToWorld localToWorld) =>
         {
             previousPositionData.Value = translation.Value;
             translation.Value += math.forward(rotation.Value) * projectile.Speed * dt;
         }).ScheduleParallel();
     }
 }
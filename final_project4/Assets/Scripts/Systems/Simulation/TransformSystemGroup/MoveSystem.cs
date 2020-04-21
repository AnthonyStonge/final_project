using Unity.Entities;
 using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
 [DisableAutoCreation]
 [UpdateAfter(typeof(RotateEnemySystem))]
 public class MoveSystem : SystemBase
 {
     //TODO CHANGE TO 3 SYSTEMS
     protected override void OnUpdate()
     {
         float dt = Time.DeltaTime;
         /*Entities.WithNone<PlayerTag>().ForEach(
             (ref Translation translation, in SpeedData speedData, in Rotation rotation) =>
         {
             translation.Value += math.forward(rotation.Value) * speedData.Value * dt;
         }).ScheduleParallel();*/
 
         //TODO With physicVelocity, the translation happens after, but when?
         Entities.WithAll<PlayerTag>().ForEach((ref PhysicsVelocity physicsVelocity, in SpeedData speedData, in InputComponent ic) =>
         {
             physicsVelocity.Linear.xz = math.normalizesafe(ic.Move) * speedData.Value * dt;
         }).Schedule();
         
         Entities.ForEach((ref Translation translation, ref DamageProjectile projectile, in LocalToWorld localToWorld) =>
         {
             translation.Value += localToWorld.Forward * projectile.Speed * dt;
         }).ScheduleParallel();
     }
 }
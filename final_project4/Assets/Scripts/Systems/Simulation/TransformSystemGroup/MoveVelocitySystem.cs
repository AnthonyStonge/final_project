using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using static Unity.Mathematics.math;
[UpdateBefore(typeof(BuildPhysicsWorld))]
public class MoveVelocitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;

        Entities.WithAll<PlayerTag>().ForEach((ref PhysicsVelocity physicsVelocity, in SpeedData speedData, in InputComponent ic) =>
        {
            if(ic.Enabled)
                physicsVelocity.Linear.xz = math.normalizesafe(ic.Move) * speedData.Value * dt;
        }).Schedule();
    }
}
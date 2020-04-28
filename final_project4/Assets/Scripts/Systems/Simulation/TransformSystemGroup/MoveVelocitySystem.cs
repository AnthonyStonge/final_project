using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using static Unity.Mathematics.math;
using float2 = Unity.Mathematics.float2;

[UpdateBefore(typeof(BuildPhysicsWorld))]
public class MoveVelocitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;

        Entities.WithAll<PlayerTag>().ForEach(
            (ref PhysicsVelocity physicsVelocity, ref StateData state, in SpeedData speedData, in InputComponent ic) =>
            {
                if (ic.Enabled)
                {
                    physicsVelocity.Linear.xz = math.normalizesafe(ic.Move) * speedData.Value * dt;
                    
                    //If inputs to move, change state
                    if (!ic.Move.Equals(float2.zero))
                    {
                        state.Value = StateActions.MOVING;
                    }
                }
            }).Schedule();
    }
}
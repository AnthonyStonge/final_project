using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Physics;
using float2 = Unity.Mathematics.float2;

[DisableAutoCreation]
public class MoveVelocitySystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;

        Entities.WithoutBurst().WithAll<PlayerTag>().ForEach(
            (Entity entity, ref PhysicsVelocity physicsVelocity, ref StateData state, in SpeedData speedData, in InputComponent ic) =>
            {
                if (!ic.Enabled) return;
                
                physicsVelocity.Linear.xz = ic.Move * speedData.Value * dt;
                    
                //If inputs to move, change state
                if (!ic.Move.Equals(float2.zero))
                {
                    EventsHolder.StateEvents.Add(new StateInfo
                    {
                        Entity = entity,
                        Action = StateInfo.ActionType.TryChange,
                        DesiredState = State.Running
                    });
                }
            }).Schedule();
    }
}
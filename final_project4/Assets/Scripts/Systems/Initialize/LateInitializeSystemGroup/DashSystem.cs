using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;


[DisableAutoCreation]
[UpdateAfter(typeof(StateDyingSystem))]
public class DashSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        Entities.WithAll<PlayerTag>().ForEach(
            (ref PhysicsVelocity physicsVelocity, ref DashComponent dashComponent, ref InputComponent ic,
                in PhysicsMass physicsMass, in Rotation rotation) =>
            {
                //translation.Value.xz += math.normalizesafe(ic.Move) * speedData.Value * dt;
                if (ic.Dash && dashComponent.CurrentCooldownTime <= 0)
                {
                    dashComponent.CurrentDashTime = dashComponent.DashTime;
                    dashComponent.CurrentCooldownTime = dashComponent.CooldownTime;
                    dashComponent.InputDuringDash = ic.Move;
                    dashComponent.TargetDuringDash = rotation.Value;
                    
                    //Disable input during dash
                    GlobalEvents.PlayerEvents.LockUserInputs(ref ic);
                }

                else if (dashComponent.CurrentCooldownTime > 0)
                {
                    dashComponent.CurrentDashTime -= dt;
                    dashComponent.CurrentCooldownTime -= dt;
                    if (dashComponent.CurrentDashTime > 0)
                    {
                        float2 velocity = float2.zero;
                        if (dashComponent.InputDuringDash.x == 0f && dashComponent.InputDuringDash.y == 0f)
                            velocity = math.forward(dashComponent.TargetDuringDash).xz * dashComponent.Speed * 100 * dt; 
                        else
                            velocity = math.normalizesafe(dashComponent.InputDuringDash) * dashComponent.Speed * 100 * dt;
                        
                        physicsVelocity.Linear.xz = velocity;
                    }
                    else
                    {
                        GlobalEvents.PlayerEvents.UnlockUserInputs(ref ic);
                    }
                }
            }).Schedule();
    }
}
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
        Entities.WithAll<PlayerTag>().ForEach((ref PhysicsVelocity physicsVelocity, ref DashComponent dashComponent,ref InputComponent ic, in PhysicsMass physicsMass, in Rotation rotation) =>
        {
            //translation.Value.xz += math.normalizesafe(ic.Move) * speedData.Value * dt;
            if (ic.Dash && dashComponent.CurrentCooldownTime <= 0)
            {
                dashComponent.CurrentDashTime = dashComponent.DashTime;
                dashComponent.CurrentCooldownTime = dashComponent.CooldownTime;
                dashComponent.InputDuringDash = ic.Move;
                dashComponent.TargetDuringDash = rotation.Value;
                ic.Enabled = false;
            }
            
            else if(dashComponent.CurrentCooldownTime > 0)
            {
                dashComponent.CurrentDashTime -= dt;
                dashComponent.CurrentCooldownTime -= dt;
                if (dashComponent.CurrentDashTime > 0)
                {
                    if (dashComponent.InputDuringDash.x == 0f && dashComponent.InputDuringDash.y == 0f)
                    {
                        physicsVelocity.ApplyLinearImpulse(physicsMass,
                            math.forward(dashComponent.TargetDuringDash) * dashComponent.Speed * 100 * dt);
                    }
                    else
                    {
                        float2 velocity = math.normalizesafe(dashComponent.InputDuringDash) * dashComponent.Speed * 100 * dt;
                        physicsVelocity.Linear.xz = velocity;
                    }
                }
                else
                {
                    ic.Enabled = true;
                }

            }
        }).Run();
    }
}
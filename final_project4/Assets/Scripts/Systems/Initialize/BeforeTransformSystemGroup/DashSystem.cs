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
            if (ic.Dash && dashComponent.DashSkillTimer <= 0)
            {
                
                dashComponent.DashTimer = dashComponent.ResetDashTimer;
                dashComponent.DashSkillTimer = dashComponent.ResetDashSkill;
                dashComponent.InputDuringDash = ic.Move;
                dashComponent.TargetDuringDash = rotation.Value;
                ic.Enabled = false;
            } 
            
            else if(dashComponent.DashSkillTimer > 0)
            {
                dashComponent.DashTimer -= dt;
                dashComponent.DashSkillTimer -= dt;
                if (dashComponent.DashTimer > 0)
                {
                    if (dashComponent.InputDuringDash.x == 0f && dashComponent.InputDuringDash.y == 0f)
                    {
                        physicsVelocity.ApplyLinearImpulse(physicsMass,
                            math.forward(dashComponent.TargetDuringDash) * 3500 * dt);
                    }
                    else
                    {
                        float2 bob = math.normalizesafe(dashComponent.InputDuringDash) * 3500 * dt;
                        physicsVelocity.Linear.xz += bob;
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
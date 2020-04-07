using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class StateDashingSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created StateDashingSystem System...");
        
        //TODO ADD TO GROUP SYSTEM
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated StateDashingSystem System...");
        
        //Act on all entities with DashComponent, StateComponent and PhysicsVelocity
        Entities.ForEach((ref StateData state, ref DashComponent dash, ref Rotation rotation, ref PhysicsVelocity velocity) =>
        {
            if (state.Value == StateActions.DASHING)
            {
                //Make sure dash isnt in cooldown
                if (dash.Timer.Available)
                {
                    dash.Timer.Reset();
                    
                    //Get Forward vector
                    float3 forward = math.forward(rotation.Value);

                    velocity.Linear += forward * dash.Distance;
                }
            }
        }).ScheduleParallel();
    }
}

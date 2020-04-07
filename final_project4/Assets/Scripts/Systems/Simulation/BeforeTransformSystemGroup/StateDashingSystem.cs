using Unity.Entities;
using Unity.Entities.CodeGeneratedJobForEach;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(StateDyingSystem))]
public class StateDashingSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created StateDashingSystem System...");
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated StateDashingSystem System...");
        
        //Act on all entities with DashComponent, StateComponent and PhysicsVelocity
        Entities.ForEach((ref PhysicsVelocity velocity, in StateData state, in DashComponent dash, in Rotation rotation) =>
        {
            if (state.Value == StateActions.DASHING)
            {
                //Make sure dash isnt in cooldown
                if (dash.Timer.Available)
                {
                    //Debug.Log("Dashed");
                    dash.Timer.Reset();
                    
                    //Get Forward vector
                    float3 forward = math.forward(rotation.Value);

                    velocity.Linear += forward * dash.Distance;
                }
            }
        }).ScheduleParallel();
    }
}

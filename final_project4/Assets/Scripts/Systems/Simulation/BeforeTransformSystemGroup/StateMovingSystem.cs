using Unity.Entities;
using UnityEngine;

public class StateMovingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Act on all entities with PlayerStateComponent (key links)
        //Compare if key has been it. If yes -> set info in component
        Entities.ForEach((ref PlayerStateComponent keys) =>
        {
            if (Input.GetKeyDown(keys.Forward_Key))
            {
            }

            if (Input.GetKeyDown(keys.Backward_Key))
            {
            }

            if (Input.GetKeyDown(keys.Left_Key))
            {
            }

            if (Input.GetKeyDown(keys.Right_Key))
            {
            }

            if (Input.GetKeyDown(keys.Attack_Key))
            {
            }

            if (Input.GetKeyDown(keys.Reload_Key))
            {
            }

            if (Input.GetKeyDown(keys.Dash_Key))
            {
            }
        }).ScheduleParallel();
    }
}
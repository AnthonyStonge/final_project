using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(EnemyTargetSystem))]
[UpdateAfter(typeof(StateIdleSystem))]
public class StateMovingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Act on all entities with PlayerStateComponent (key links)
        //Compare if key has been it. If yes -> set info in component
        Entities.ForEach((ref PlayerStateComponent keys) =>
        {
            
        }).ScheduleParallel();
    }
}
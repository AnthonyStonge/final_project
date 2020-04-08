using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(InputSystem))]
public class PlayerTargetSystem : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        //Act on all entities with Target, Input and PlayerTag
        Entities.WithAll<PlayerTag>().ForEach((ref TargetData target, ref InputComponent inputs) =>
        {
            
            //Set target to mouse position
            target.Value = inputs.Mouse;
        }).Run();    //TODO CHANGE TO ScheduleParallel() IF MULTIPLE PLAYERS
    }
}
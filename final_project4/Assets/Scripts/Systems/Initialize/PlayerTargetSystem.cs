using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
//TODO ADD TO GROUP SYSTEM
[UpdateAfter(typeof(InputSystem))]
public class PlayerTargetSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created PlayerTargetSystem System...");
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated PlayerTargetSystem System...");
        
        //Act on all entities with Target, Input and PlayerTag
        Entities.WithAll<PlayerTag>().ForEach((ref TargetData target, ref InputComponent inputs) =>
        {
            //Set target to mouse position
            target.Value = inputs.Mouse;
        }).Run();    //TODO CHANGE TO ScheduleParallel() IF MULTIPLE PLAYERS
    }
}
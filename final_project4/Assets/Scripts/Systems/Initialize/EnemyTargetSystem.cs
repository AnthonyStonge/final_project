using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
//TODO ADD TO GROUP SYSTEM
//[UpdateInGroup(typeof())]
public class EnemyTargetSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created EnemyTargetSystem System...");
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated EnemyTargetSystem System...");

        //Act on all entities with Target and EnemyTag. Update 
        Entities.WithAll<EnemyTag>().ForEach((ref TargetData target) =>
        {
            //TODO IMPLEMENT WITH NEXT NODE ENEMY SHOULD GO TO
        }).ScheduleParallel();
    }
}
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class DecrementTimeSystem : SystemBase
{
    protected override void OnCreate()
    {
    } 

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        //Act on all entities with TimeTrackerComponent and decrement Time.DeltaTime
        Entities.ForEach((ref TimeTrackerComponent time) =>
        {
            if (!time.Available)
            {
                //Debug.Log(deltaTime);
                time.Current -= deltaTime;
            }
        }).ScheduleParallel();
    }
}
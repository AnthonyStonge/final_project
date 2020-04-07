using Unity.Entities;

public class DecrementTimeSystem : SystemBase
{
    protected override void OnCreate()
    {
        //Debug.Log("Created DecrementTimeSystem System...");
        
        //TODO ADD TO SYSTEM GROUP
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated DecrementTimeSystem System...");
        
        float deltaTime = Time.DeltaTime;

        //Act on all entities with TimeTrackerComponent and decrement Time.DeltaTime
        Entities.ForEach((ref TimeTrackerComponent time) =>
        {
            if (time.Current > 0)
                time.Current -= deltaTime;
        }).ScheduleParallel();
    }
}
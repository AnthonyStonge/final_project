using Unity.Entities;
using UnityEngine;

public class StateDyingSystem : SystemBase
{
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        Debug.Log("Created Health System...");
        this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnStartRunning()
    {
        Debug.Log("Started Health System...");
        //Add system to the group it belongs to
        SimulationSystemGroup simulation = World.GetOrCreateSystem<SimulationSystemGroup>();
        simulation.AddSystemToUpdateList(this);
        simulation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        Debug.Log("On HealthUpdate...");
        //Act on all entities with HealthData.
        Entities.ForEach((ref HealthData health, ref StateData state) =>
        {
            //If health <= 0 -> set state to dying
            if (health.Value <= 0)
                state.Value = StateActions.DYING;
            else
                Debug.Log("Still alive");
        }).ScheduleParallel();
    }
}
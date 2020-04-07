using Unity.Entities;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateAfter(typeof(StateAttackingSystem))]
[UpdateAfter(typeof(UpdatePlayerStateSystem))]
public class StateDyingSystem : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        //Debug.Log("On HealthUpdate...");
        ////Act on all entities with HealthData.
        Entities.ForEach((ref StateData state, in HealthData health) =>
        {
            //If health <= 0 -> set state to dying
            if (health.Value <= 0)
                state.Value = StateActions.DYING;
        }).ScheduleParallel();
    }
}
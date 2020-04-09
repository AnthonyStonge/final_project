using Unity.Entities;

[DisableAutoCreation]
public class StateIdleSystem : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Updated Idle System...");
        
        //Act on all StateData
        Entities.ForEach((ref StateData state) =>
        {
            //Reset all states to Idle until proven opposite
            state.Value = StateActions.IDLE;
        }).ScheduleParallel();
    }
}

using Unity.Entities;

public class StateIdleSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Act on all StateData
        Entities.ForEach((ref StateData state) =>
        {
            //Reset all states to Idle until proven opposite
            state.Value = FellowActions.IDLE;
        }).ScheduleParallel();
    }
}

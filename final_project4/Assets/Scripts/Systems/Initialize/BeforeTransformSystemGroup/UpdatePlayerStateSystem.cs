using Unity.Entities;

[DisableAutoCreation]
[UpdateAfter(typeof(PlayerTargetSystem))]
[UpdateAfter(typeof(DecrementTimeSystem))]
public class UpdatePlayerStateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref StateData stateData, in InputComponent ic) =>
        {
            if (ic.Shoot)
            {
                stateData.Value = StateActions.ATTACKING;
            }
        }).Schedule();
    }
}

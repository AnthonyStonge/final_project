using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(RotateEnemySystem))]
public class MoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        Entities.ForEach(
            (ref Translation translation, in SpeedData speedData, in ForwardData forward) =>
            {
                translation.Value += forward.Value * speedData.Value * dt;
            }).ScheduleParallel();

        //TODO UPDATE PLAYER POSITION IN ANOTHER SYSTEM FOR PROPERTY SAKE
        Entities.WithAll<PlayerTag>().ForEach((in Translation trans) =>
        {
            GameVariables.PlayerVars.CurrentPosition = trans.Value;
        }).Run();
    }
}
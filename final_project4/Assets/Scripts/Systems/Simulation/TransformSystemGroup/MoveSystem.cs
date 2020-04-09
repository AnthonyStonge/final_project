using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.PlayerLoop;
[DisableAutoCreation]
[UpdateAfter(typeof(RotateEnemySystem))]
public class MoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        Entities.WithNone<PlayerTag>().ForEach(
            (ref Translation translation, in SpeedData speedData, in LocalToWorld localToWorld) =>
            {
                translation.Value += localToWorld.Forward * speedData.Value * dt;
            }).ScheduleParallel();

        Entities.WithAll<PlayerTag>().ForEach((ref Translation translation, in SpeedData speedData, in InputComponent ic) =>
            {
                translation.Value += new float3(ic.Move.x, 0f, ic.Move.y) * speedData.Value * dt;
            }).Schedule();
    }
}
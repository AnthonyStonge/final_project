using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class NewBitchSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref InputComponent ic) => {}).Schedule();
    }
}
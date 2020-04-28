using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

[DisableAutoCreation]
public class AnimationSystem : SystemBase
{
    private EntityCommandBufferSystem entityCommandBuffer;
    private int batchIdToUpdate;

    protected override void OnCreate()
    {
        entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        //Create ECB
        EntityCommandBuffer.Concurrent ecb = entityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        //For each entity, swap frame to next one
        Entities.WithoutBurst().WithSharedComponentFilter(new AnimationBatch {BatchId = batchIdToUpdate}).ForEach(
            (Entity e, int entityInQueryIndex, ref AnimationData animation, in StateData state, in TypeData type) =>
            {
                //Increment frame at + Clamp it
                animation.MeshIndexAt++;
                animation.MeshIndexAt %= (ushort) AnimationHolder.Animations[type.Value][state.Value].Frames.Length;

                ecb.SetSharedComponent(entityInQueryIndex, e, new RenderMesh
                {
                    mesh = AnimationHolder.Animations[type.Value][state.Value].Frames[animation.MeshIndexAt],
                    material = AnimationHolder.Animations[type.Value][state.Value].Material
                });
            }).ScheduleParallel();

        entityCommandBuffer.AddJobHandleForProducer(Dependency);

        batchIdToUpdate++;
        batchIdToUpdate %= AnimationHolder.AnimatedGroupsLength.Count;
    }
}
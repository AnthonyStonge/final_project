using Unity.Entities;
using Unity.Rendering;

[DisableAutoCreation]
public class AnimationSystem : SystemBase
{
    private EntityCommandBufferSystem entityCommandBuffer;
    public static int BatchIdToUpdate;

    protected override void OnCreate()
    {
        entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        //Create ECB
        EntityCommandBuffer.Concurrent ecb = entityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        //For each entity, swap frame to next one
        Entities.WithoutBurst().WithSharedComponentFilter(new AnimationBatch {BatchId = BatchIdToUpdate}).ForEach(
            (Entity e, int entityInQueryIndex, ref AnimationData animation, in StateComponent state, in TypeData type) =>
            {
                //Make sure animation exists for this type/state
                if (!AnimationHolder.Animations.ContainsKey(type.Value) ||
                    !AnimationHolder.Animations[type.Value].ContainsKey(state.CurrentState))
                    return;

                //Increment frame at + Clamp it
                animation.MeshIndexAt++;
                animation.MeshIndexAt %= (ushort) AnimationHolder.Animations[type.Value][state.CurrentState].Frames.Length;

                ecb.SetSharedComponent(entityInQueryIndex, e, new RenderMesh
                {
                    mesh = AnimationHolder.Animations[type.Value][state.CurrentState].Frames[animation.MeshIndexAt],
                    material = AnimationHolder.Animations[type.Value][state.CurrentState].Material
                });
            }).ScheduleParallel();

        entityCommandBuffer.AddJobHandleForProducer(Dependency);

        BatchIdToUpdate++;
        BatchIdToUpdate %= AnimationHolder.AnimatedGroupsLength.Count;
    }
}
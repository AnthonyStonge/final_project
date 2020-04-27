using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

[DisableAutoCreation]
public class AnimationSystem : SystemBase
{
    private EntityManager entityManager;
    private EntityCommandBufferSystem entityCommandBuffer;

    private EntityQuery query;
    private int batchIdToUpdate;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        //Create ECB
        EntityCommandBuffer.Concurrent ecb = entityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        Container c = new Container
        {
            AnimationsLength = AnimationHolder.AnimationsLength
        };

        //For each entity, swap frame to next one
        Entities.WithoutBurst().WithSharedComponentFilter(new AnimationBatch {BatchId = batchIdToUpdate}).ForEach(
            (Entity e, int entityInQueryIndex, ref AnimationComponent animation) =>
            {
                //Increment frame at + Clamp it
                animation.MeshIndexAt++;
                animation.MeshIndexAt %= (ushort) c.AnimationsLength[(int) animation.AnimationType];

                ecb.SetSharedComponent(entityInQueryIndex, e, new RenderMesh
                {
                    mesh = AnimationHolder.AnimationFrames[animation.AnimationType][animation.MeshIndexAt],
                    material = AnimationHolder.MeshMaterials[animation.AnimationType]
                });
            }).ScheduleParallel();
        
        entityCommandBuffer.AddJobHandleForProducer(Dependency);

        batchIdToUpdate++;
        batchIdToUpdate %= AnimationHolder.AnimatedGroupsLength.Count;
    }

    struct Container
    {
        [ReadOnly] public NativeList<int> AnimationsLength;
    }
}
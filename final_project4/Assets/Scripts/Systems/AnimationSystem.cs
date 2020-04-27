using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Rendering;

[DisableAutoCreation]
public class AnimationSystem : SystemBase
{
    private NativeQueue<FrameInfo> framesToSwap;
    private EntityCommandBufferSystem entityCommandBuffer;

    private EntityManager entityManager;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        framesToSwap = new NativeQueue<FrameInfo>(Allocator.Persistent);
        entityCommandBuffer = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnDestroy()
    {
        framesToSwap.Dispose();
    }

    protected override void OnUpdate()
    {
        //Create parallel writer
        NativeQueue<FrameInfo>.ParallelWriter objectsToSwapFrame = framesToSwap.AsParallelWriter();

        //Create ECB
        EntityCommandBuffer.Concurrent ecb = entityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        Container c = new Container
        {
            AnimationLength = AnimationHolder.AnimationsLength
        };
        float deltaTime = Time.DeltaTime;
        
        //Get all AnimationComponents and decrement timer. If time to change frame, add to Queue
        JobHandle job = Entities.ForEach((Entity e, ref AnimationComponent animation) =>
        {
            animation.Timer += deltaTime;

            if (animation.Timer >= animation.TimeBetweenFrame)
            {
                //Decrement timer
                //animation.Timer -= animation.TimeBetweenFrame;
                animation.Timer = 0;
                
                //Increment FrameAt
                animation.MeshIndexAt++;
                
                //Clamp frame at
                animation.MeshIndexAt %= (ushort) c.AnimationLength[(int)animation.AnimationType];
                
                //Add to Queue
                objectsToSwapFrame.Enqueue(new FrameInfo
                {
                    e = e,
                    Type = animation.AnimationType,
                    indexAt = animation.MeshIndexAt
                });
            }
        }).ScheduleParallel(Dependency);
        
        //TODO Dont create a blocker here...
        job.Complete();

        while (framesToSwap.TryDequeue(out FrameInfo info))
        {
            entityManager.SetSharedComponentData(info.e, new RenderMesh
            {
                mesh = AnimationHolder.AnimationFrames[info.Type][info.indexAt],
                material = AnimationHolder.MeshMaterials[info.Type]
            });
        }
        
        //Change ShareComponent for each Entity in the Queue
        
        //Take all Entities with the same ShareComponent in Queue and swap them together as a query
    }
    
    public struct FrameInfo
    {
        public Entity e;
        public Animation.AnimationType Type;
        public ushort indexAt;
    }

    struct Container
    {
        [ReadOnly] public NativeList<int> AnimationLength;
    }
}
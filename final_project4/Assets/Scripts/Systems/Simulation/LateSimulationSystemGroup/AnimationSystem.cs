using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using AnimationInfo = EventStruct.AnimationInfo;

[DisableAutoCreation]
public class AnimationSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem entityCommandBuffer;
    public static int BatchIdToUpdate;

    // private bool isUpdateAnimationFrame;
    private float timer;
    private float resetTimer = 0.02f;

    protected override void OnCreate()
    {
        entityCommandBuffer = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        timer -= Time.DeltaTime;
        
        while (timer <= 0)
        {
            //Create parallel writer
            NativeQueue<AnimationInfo>.ParallelWriter events = EventsHolder.AnimationEvents.AsParallelWriter();

            //Create ECB
            EntityCommandBuffer.Concurrent ecb = entityCommandBuffer.CreateCommandBuffer().ToConcurrent();

            //For each entity, swap frame to next one
            Entities.WithoutBurst().WithSharedComponentFilter(new AnimationBatch {BatchId = BatchIdToUpdate}).ForEach(
                (Entity e, int entityInQueryIndex, ref AnimationData animation, in StateComponent state,
                    in TypeData type) =>
                {
                    //Make sure animation exists for this type/state
                    if (!AnimationHolder.Animations.ContainsKey(type.Value) ||
                        !AnimationHolder.Animations[type.Value].ContainsKey(state.CurrentAnimationState))
                        return;

                    //Increment frame at
                    animation.MeshIndexAt++;
                    short animationLength = (short) AnimationHolder.Animations[type.Value][state.CurrentAnimationState].Frames.Length;
                    
                    //If reached end of animation -> Create Event
                    if (animation.MeshIndexAt == animationLength)
                    {
                        events.Enqueue(new AnimationInfo
                        {
                            Entity = e,
                            Type = AnimationInfo.EventType.OnAnimationEnd,
                            NewState = state.CurrentAnimationState
                        });

                        if (state.CurrentAnimationState == State.Dying)
                            animation.MeshIndexAt--;
                    }
                    
                    //Clamp frame at
                    animation.MeshIndexAt %= animationLength;

                    ecb.SetSharedComponent(entityInQueryIndex, e, new RenderMesh
                    {
                        mesh = AnimationHolder.Animations[type.Value][state.CurrentAnimationState]
                            .Frames[animation.MeshIndexAt],
                        material = AnimationHolder.Animations[type.Value][state.CurrentAnimationState].Material
                    });
                }).ScheduleParallel(Dependency).Complete();

            entityCommandBuffer.AddJobHandleForProducer(Dependency);

            //Reset Timer
            // isUpdateAnimationFrame = false;
            timer += resetTimer;

            //Increment batch
            BatchIdToUpdate++;
            BatchIdToUpdate %= AnimationHolder.AnimatedGroupsLength.Count;
        }
    }

}
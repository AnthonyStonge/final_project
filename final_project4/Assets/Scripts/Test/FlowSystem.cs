using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[AlwaysUpdateSystem]
[DisableAutoCreation]
public class FlowSystem : SystemBase
{
    private EntityManager entityManager;

    
#pragma warning disable 649
    private EntityArchetype entityArchetype;
#pragma warning restore 649
    
    private static List<RenderMesh> renderMeshFrames;

    private EntityQuery query;
    private EntityQuery frameTagQuery;

    private static bool toUpdate;
    private static float secondCounter;
    private static int counter;
    private static float3 value = new float3(5f, 5f, 5f);

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var tmpQuery = new EntityQueryDesc
        {
            None = new ComponentType[] {typeof(ChangedFrameTag)},
            All = new ComponentType[] {typeof(RenderMesh)}
        };

        query = GetEntityQuery(tmpQuery);
        frameTagQuery = GetEntityQuery(typeof(ChangedFrameTag));
        
        counter = 0;
        secondCounter = 0;
        renderMeshFrames = new List<RenderMesh>();
    }
    
    protected override void OnUpdate()
    {
        if (Input.GetButtonDown("Jump"))
        {
            CreateEntities();
        }

        secondCounter += Time.DeltaTime;
        if (secondCounter >= 0.04166f )
        {
            toUpdate = true;
        }

        if (toUpdate)
        {
            toUpdate = false;
            secondCounter = 0f;
            for (int i = 0; i < renderMeshFrames.Count - 1; i++)
            {
                query.SetSharedComponentFilter(renderMeshFrames[i]);
                //Store entities
                NativeArray<Entity> arrAnim1 = query.ToEntityArray(Allocator.TempJob);
                //Modify RenderMesh to next frame
                entityManager.SetSharedComponentData(query, renderMeshFrames[i+1]);
                //Add Tag
                entityManager.AddComponent<ChangedFrameTag>(arrAnim1);
                //Dispose
                arrAnim1.Dispose();
            }
            query.SetSharedComponentFilter(renderMeshFrames[renderMeshFrames.Count - 1]);
            entityManager.SetSharedComponentData(query, renderMeshFrames[0]);
            
            entityManager.RemoveComponent<ChangedFrameTag>(frameTagQuery);
        }
    }

    protected override void OnStopRunning()
    {
    }

    protected override void OnDestroy()
    {
    }

    private void CreateEntities()
    {
        var rnd = new Random(12345);
        counter++;
        for (int i = 0; i < 2000; i++)
        {
            CreateEntity(rnd.NextFloat3(-value * counter, value * counter), renderMeshFrames[rnd.NextInt(0,renderMeshFrames.Count - 1)]);
        }
    }

    private void CreateEntity(float3 newPosition, RenderMesh renderMesh)
    {
        Entity entity = entityManager.CreateEntity(entityArchetype);

        entityManager.SetComponentData(entity, new Translation
        {
            Value = new float3(newPosition)
        });
        entityManager.SetSharedComponentData(entity, renderMesh);
    }
}
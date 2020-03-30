using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Mesh eMesh;
    [SerializeField] private Material eMat;
    private void Start()
    {
        MakeEntity();
    }

    private void MakeEntity()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld));
        
        Entity e = entityManager.CreateEntity(archetype);
        
        entityManager.AddComponentData(e, new Translation {Value = new float3(2f, 0f, 4f)});
        entityManager.AddSharedComponentData(e, new RenderMesh{mesh = eMesh, material = eMat});
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    public Mesh meshTest;
    public Material mat;
    void Start()
    {
        EntityArchetype archetype = new EntityArchetype();

        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype bob = em.CreateArchetype(typeof(StateData), typeof(ForwardData), typeof(SpeedData), typeof(LocalToWorld), typeof(Rotation), typeof(TargetData), typeof(RenderBounds), typeof(Translation), typeof(PlayerTag), typeof(InputComponent), typeof(RenderMesh));
        Entity bob2 = em.CreateEntity(bob);
        em.SetComponentData(bob2, new SpeedData
        {
            Value = 2f
        });
        em.SetSharedComponentData(bob2, new RenderMesh 
        {
            mesh = meshTest,
            material = mat
        });
    }
}

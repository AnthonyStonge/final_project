using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public Mesh meshTest;
    public Material mat;
    void Start()
    {
        EntityArchetype archetype = new EntityArchetype();

        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype bob = em.CreateArchetype(typeof(LocalToWorld), typeof(Rotation), typeof(TargetData), typeof(RenderBounds), typeof(Translation), typeof(PlayerTag), typeof(InputComponent), typeof(RenderMesh));
        Entity bob2 = em.CreateEntity(bob);
        em.SetSharedComponentData(bob2, new RenderMesh 
        {
            mesh = meshTest,
            material = mat
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

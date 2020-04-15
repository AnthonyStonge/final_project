using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
public class NewSpawner : MonoBehaviour
{
    private EntityManager em;
    public Material mat;
    public Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    void Update()
    {

        createEntity(0);

    }
    private void createEntity(int i)
    {
        EntityArchetype ea = em.CreateArchetype(typeof(Translation), typeof(RenderMesh), typeof(LocalToWorld), typeof(RenderBounds));
        Entity e = em.CreateEntity(ea);
       
        em.SetComponentData(e, new Translation
        {
            Value = new float3(i,0,0)
        });
        em.SetSharedComponentData(e, new RenderMesh
        {
            mesh = mesh,
            material = mat
        });

    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EntityArchetype archetype = new EntityArchetype();

        EntityManager em = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityArchetype bob = em.CreateArchetype(typeof(Rotation), typeof(TargetData), typeof(RenderBounds), typeof(Translation), typeof(PlayerTag));
        em.CreateEntity(bob);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

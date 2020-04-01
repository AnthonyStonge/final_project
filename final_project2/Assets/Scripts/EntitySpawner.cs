using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private EntityManager entityManager;

    private EntityArchetype archPISSS;
    
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        archPISSS = entityManager.CreateArchetype(typeof(InputComponent));
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            CreateEntity();
        }
    }

    private void CreateEntity()
    {
        Entity e = entityManager.CreateEntity(archPISSS);
        entityManager.SetComponentData(e, new InputComponent
        {
            dash = false,
            interact = false,
            inventory = 0,
            move = new float2(0.0f, 0.0f),
            pause = false
        });
        Debug.Log("Entity Created");
    }
    
}

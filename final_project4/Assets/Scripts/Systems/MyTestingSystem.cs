using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MyTestingSystem : SystemBase
{
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        /*this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //Create entity
        Entity e = this.entityManager.CreateEntity();

        this.entityManager.AddComponentData(e, new HealthData
        {
            Value = 10
        });*/
    }

    protected override void OnStartRunning()
    {
    }

    protected override void OnUpdate()
    {
    }
}
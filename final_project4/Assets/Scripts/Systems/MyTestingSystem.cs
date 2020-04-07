using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class MyTestingSystem : SystemBase
{
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        /*Debug.Log("Creating ur shitty entity");
        this.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //Create entity
        Entity e = this.entityManager.CreateEntity();

        this.entityManager.AddComponentData(e, new HealthData
        {
            Value = 0
        });
        this.entityManager.AddComponentData(e, new StateData
        {
            Value = FellowActions.IDLE
        });*/
    }

    protected override void OnStartRunning()
    {
    }

    protected override void OnUpdate()
    {
    }
}
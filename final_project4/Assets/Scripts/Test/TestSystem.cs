using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class TestSystem : SystemBase
{
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        Entity enemy = entityManager.CreateEntity();
        Entity gun = entityManager.CreateEntity();

        entityManager.AddComponentData(enemy, new StateMachine
        {
            state = true
        });
        
        entityManager.AddComponentData(gun, new MachineGun
        {
            bullets = 100,
            fire = true,
            parent = enemy
        });

        /*  ref MachineGun machineGun = ref bb.ConstructRoot<MachineGun>();
          machineGun.bullets = 50;
          machineGun.fire = true;
          ref LinkEntity link = ref bb.Allocate(ref machineGun.link);*/
        /*BlobBuilder bb = new BlobBuilder(Allocator.Temp);
        ref MachineGun machineGun = ref bb.ConstructRoot<MachineGun>();
        ref StateMachine stateMachine = ref bb.Allocate(ref machineGun.parentState);

        entityManager.AddComponentData(enemy, new StateMachine
        {
            state = true
        });

        entityManager.AddComponentData(gun, machineGun);
        
        
        stateMachine = entityManager.GetComponentData<StateMachine>(enemy);

        Debug.Log(entityManager.GetComponentData<MachineGun>(gun).parentState);*/

        //stateMachine.state = true;

        /*BlobAssetReference<MachineGun> blobRef = bb.CreateBlobAssetReference<MachineGun>(Allocator.Persistent);
        machineGun = ref blobRef.Value;

        bb.Dispose();*/
    }


    protected override void OnUpdate()
    {
        Debug.Log("Before");
        Entities.ForEach((ref MachineGun mg) =>
        {
            Debug.Log("In Loop");
        }).ScheduleParallel();
        Debug.Log("After");
    }
}
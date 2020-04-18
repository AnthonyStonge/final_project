using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class TemporaryEnemySpawnerSystem : SystemBase
{
    private EntityManager entityManager;
    private Random random;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        random = new Random(69);
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < 10; i++)
            {
                //Spawn enemy at random position (-50 -> 50)
                float3 spawnPosition = new float3(random.NextInt(-50, 50), 0, random.NextInt(-50, 50));

                Entity enemy = entityManager.Instantiate(EnemyHolder.EnemyPrefabsEntities[EnemyType.GABICHOU]);
                entityManager.SetComponentData(enemy, new Translation
                {
                    Value = spawnPosition
                });
            }
        }
    }
}
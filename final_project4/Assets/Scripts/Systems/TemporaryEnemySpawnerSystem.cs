using System.Collections.Generic;
using System.Linq;
using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
using Type = Enums.Type;

[DisableAutoCreation]
public class TemporaryEnemySpawnerSystem : SystemBase
{
    private static EntityManager entityManager;

    private static ushort batchFilter = 0;
    private static NativeList<EnemySpawnerInfo> copySpawner = new NativeList<EnemySpawnerInfo>(Allocator.Persistent);
    
    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    }
    public static void InitializeDefaultEnemySpawn(MapType type)
    {
        //enemySpawner.Clear();

        copySpawner.Clear();
        if (GameVariables.Grids.ContainsKey(type))
        {
            for (int i = 0; i < GameVariables.Grids[type].enemySpawner.Count; i++)
            {
                copySpawner.Add(GameVariables.Grids[type].enemySpawner[i]);

                var spawnerTest = copySpawner[i];
                spawnerTest.currentEnnemySpawn++;
                spawnerTest.currentTime = Random.Range(copySpawner[i].TimeRangeBetweenSpawn.x,
                    copySpawner[i].TimeRangeBetweenSpawn.y);
                copySpawner[i] = spawnerTest;
            }

            foreach (var spawner in GameVariables.Grids[type].enemyPreSpawner)
            {
                System.Array enemyType = System.Enum.GetValues(typeof(Type));
                Type randomEnemyType = (Type) enemyType.GetValue(Random.Range(0, enemyType.Length));
                switch (spawner.EnemyType)
                {
                    case Type.Chicken:
                        CreateEnemy(Type.Chicken, out Entity chicken, spawner.spawnerPos, -1);
                        CreateWeapon(WeaponType.ChickenWeapon, chicken);
                        break;
                    case Type.Gorilla:
                        CreateEnemy(Type.Gorilla, out Entity gorilla, spawner.spawnerPos, -1);
                        CreateWeapon(WeaponType.GorillaWeapon, gorilla);
                        break;
                    case Type.Pig:
                        CreateEnemy(Type.Pig, out Entity pig, spawner.spawnerPos, -1);
                        CreateWeapon(WeaponType.PigWeapon, pig);
                        break;
                    case Type.Rat:
                        CreateEnemy(Type.Rat, out Entity rat, spawner.spawnerPos,-1);
                        // CreateWeapon(WeaponType.RatWeapon, rat);
                        break;
                }

                EventsHolder.LevelEvents.NbEnemy++;
            }
        }

    }
    protected override void OnUpdate()
    {
        if (GameVariables.Grids.ContainsKey(EventsHolder.LevelEvents.CurrentLevel))
        {
            //var enemySpawner = GameVariables.Grids[EventsHolder.LevelEvents.CurrentLevel].enemySpawner;
            for (int i = 0; i < copySpawner.Length; i++)
            {
                if (copySpawner[i].NbEnnemyMax >= copySpawner[i].currentEnnemySpawn)
                {
                    if (copySpawner[i].currentTime <= 0)
                    {
                        //System.Array enemyType = System.Enum.GetValues(typeof(Type));
                        //Type randomEnemyType = (Type) enemyType.GetValue(Random.Range(0, enemyType.Length));
                        switch (copySpawner[i].EnemyType)
                        {
                            case Type.Chicken:
                                CreateEnemy(Type.Chicken, out Entity chicken, copySpawner[i].spawnerPos, i);
                                CreateWeapon(WeaponType.ChickenWeapon, chicken);
                                break;
                            case Type.Gorilla:
                                CreateEnemy(Type.Gorilla, out Entity gorilla, copySpawner[i].spawnerPos, i);
                                CreateWeapon(WeaponType.GorillaWeapon, gorilla);
                                break;
                            case Type.Pig:
                                CreateEnemy(Type.Pig, out Entity pig, copySpawner[i].spawnerPos, i);
                                CreateWeapon(WeaponType.PigWeapon, pig);
                                break;
                            case Type.Rat:
                                CreateEnemy(Type.Rat, out Entity rat, copySpawner[i].spawnerPos, i);
                                // CreateWeapon(WeaponType.RatWeapon, rat);
                                break;
                        }
                        var spawnerTest = copySpawner[i];
                        spawnerTest.currentEnnemySpawn++;
                        spawnerTest.currentTime = Random.Range(copySpawner[i].TimeRangeBetweenSpawn.x,
                            copySpawner[i].TimeRangeBetweenSpawn.y);
                        copySpawner[i] = spawnerTest;
                        EventsHolder.LevelEvents.NbEnemy++;
                    }
                    else
                    {
                        var spawnerTest = copySpawner[i];
                        spawnerTest.currentTime -= Time.DeltaTime;
                        copySpawner[i] = spawnerTest;
                        //Debug.Log(copySpawner[i].currentTime);
                    }
                }
            }
        }
    }

    private static void CreateEnemy(Type type, out Entity e, in int2 spawnPosition, int i)
    {
        e = Entity.Null;

        //Make sure desired type exists
        if (!EnemyHolder.EnemyPrefabDict.ContainsKey(type))
            return;

        e = entityManager.Instantiate(EnemyHolder.EnemyPrefabDict[type]);

        //Set position
        entityManager.SetComponentData(e, new Translation
        {
            Value = new float3(spawnPosition.x, -1, spawnPosition.y)
        });
        float2 translation = entityManager.GetComponentData<Translation>(e).Value.xz;
        if (i != -1)
        {
            entityManager.AddComponentData(e, new PathFollowComponent
            {
                //BeginWalk = true,
                EnemyState = EnemyState.Attack,
                WonderingPosition = (int2)(translation + (copySpawner[i].EnemySpawnDirection * new int2(copySpawner[i].Distance, copySpawner[i].Distance)))
            });
        }
        else
        {
            entityManager.AddComponentData(e, new PathFollowComponent
            {
                pathIndex = -1,
                EnemyState = EnemyState.Wondering
            });
        }

       /* //Set BatchFilter
        entityManager.AddSharedComponentData(e, new BatchFilter
        {
            Value = batchFilter
        });
*/
        //Set AnimationBatch
        entityManager.AddSharedComponentData(e, new AnimationBatch
        {
            BatchId = AnimationHolder.AddAnimatedObject()
        });
        //Clamp batch filter
       /* batchFilter++;
        batchFilter %= 8; //TODO REMOVE MAGIC NUMBER*/
    }

    private static void CreateWeapon(WeaponType type, Entity parent)
    {
        Entity e = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[type]);
        ECSUtility.MergeEntitiesTogether(entityManager, parent, e);
    }
    
    protected override void OnDestroy()
    {
        copySpawner.Dispose();
    }
}
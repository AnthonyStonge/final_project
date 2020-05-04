using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
using Type = Enums.Type;

public class TemporaryEnemySpawnerSystem : SystemBase
{
    private static EntityManager entityManager;

    private static ushort batchFilter = 0;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    public static void initializeDefaultEnemySpawn(MapType type)
    {
        foreach (var spawner in GameVariables.Grids[type].enemyPreSpawner)
        {
            System.Array test = System.Enum.GetValues(typeof(Type));
            Type plsWork = (Type) test.GetValue(Random.Range(0, test.Length));
            switch (plsWork)
            {
                case Type.Chicken:
                    CreateEnemy(Type.Chicken, out Entity chicken, spawner.spawnerPos);
                    CreateWeapon(WeaponType.Pistol, chicken);
                    break;
                case Type.Gorilla:
                    CreateEnemy(Type.Gorilla, out Entity gorilla, spawner.spawnerPos);
                    CreateWeapon(WeaponType.Pistol, gorilla);
                    break;
                case Type.Pig:
                    CreateEnemy(Type.Pig, out Entity pig, spawner.spawnerPos);
                    CreateWeapon(WeaponType.Pistol, pig);
                    break;
                case Type.Rat:
                    CreateEnemy(Type.Rat, out Entity rat, spawner.spawnerPos);
                    CreateWeapon(WeaponType.Pistol, rat);
                    break;
            }
            
        }
        
    }
    protected override void OnUpdate()
    {
        if (GameVariables.Grids.ContainsKey(EventsHolder.LevelEvents.CurrentLevel))
        {
            var enemySpawner = GameVariables.Grids[EventsHolder.LevelEvents.CurrentLevel].enemySpawner;
            for (int i = 0; i < enemySpawner.Count; i++)
            {
                if (enemySpawner[i].currentTime <= 0 && enemySpawner[i].NbEnnemyMax <= enemySpawner[i].currentEnnemySpawn)
                {
                    switch (enemySpawner[i].EnemyType)
                    {
                        case Type.Chicken:
                            CreateEnemy(Type.Chicken, out Entity chicken, enemySpawner[i].spawnerPos);
                            CreateWeapon(WeaponType.Pistol, chicken);
                            break;
                        case Type.Gorilla:
                            CreateEnemy(Type.Gorilla, out Entity gorilla, enemySpawner[i].spawnerPos);
                            CreateWeapon(WeaponType.Pistol, gorilla);
                            break;
                        case Type.Pig:
                            CreateEnemy(Type.Pig, out Entity pig, enemySpawner[i].spawnerPos);
                            CreateWeapon(WeaponType.Pistol, pig);
                            break;
                        case Type.Rat:
                            CreateEnemy(Type.Rat, out Entity rat, enemySpawner[i].spawnerPos);
                            CreateWeapon(WeaponType.Pistol, rat);
                            break;
                    }
                    var spawnerTest = enemySpawner[i];
                    spawnerTest.currentEnnemySpawn++;
                    spawnerTest.currentTime = Random.Range(enemySpawner[i].TimeRangeBetweenSpawn.x,
                        enemySpawner[i].TimeRangeBetweenSpawn.y);
                    enemySpawner[i] = spawnerTest;
                }
                else
                {
                    var spawnerTest = enemySpawner[i];
                    spawnerTest.currentEnnemySpawn += 1;
                    spawnerTest.currentTime -= Time.DeltaTime;
                    enemySpawner[i] = spawnerTest;
                }
            }
        }
        /*if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < 5; i++)
            {
                CreateEnemy(Type.Rat, out Entity rat);
                if (rat != Entity.Null)
                    CreateWeapon(WeaponType.RatWeapon, rat);
                CreateEnemy(Type.Chicken, out Entity chicken);
                if (chicken != Entity.Null)
                    CreateWeapon(WeaponType.ChickenWeapon, chicken);
                CreateEnemy(Type.Pig, out Entity pig);
                if (pig != Entity.Null)
                    CreateWeapon(WeaponType.PigWeapon, pig);
                CreateEnemy(Type.Gorilla, out Entity gorilla);
                if (gorilla != Entity.Null)
                    CreateWeapon(WeaponType.GorillaWeapon, gorilla);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Pig, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.PigWeapon, e);
            }

        if (Input.GetKeyDown(KeyCode.O))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Rat, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.RatWeapon, e);
            }

        if (Input.GetKeyDown(KeyCode.I))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Chicken, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.ChickenWeapon, e);
            }

        if (Input.GetKeyDown(KeyCode.U))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Gorilla, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.Shotgun, e);
            }*/

    }

    private static void CreateEnemy(Type type, out Entity e, in int2 spawnPosition)
    {
        e = Entity.Null;

        //Make sure desired type exists
        if (!EnemyHolder.EnemyPrefabDict.ContainsKey(type))
            return;

        e = entityManager.Instantiate(EnemyHolder.EnemyPrefabDict[type]);

        //Set position
        entityManager.SetComponentData(e, new Translation
        {
            Value = new float3(spawnPosition.x, 0, spawnPosition.y)
        });

        //Set BatchFilter
        entityManager.AddSharedComponentData(e, new BatchFilter
        {
            Value = batchFilter
        });

        //Set AnimationBatch
        entityManager.AddSharedComponentData(e, new AnimationBatch
        {
            BatchId = AnimationHolder.AddAnimatedObject()
        });

        //Set PathFollowComponent
        entityManager.AddComponentData(e, new PathFollowComponent
        {
            pathIndex = -1,
            EnemyState = EnemyState.Wondering
        });

        //Clamp batch filter
        batchFilter++;
        batchFilter %= 8; //TODO REMOVE MAGIC NUMBER
    }

    private static void CreateWeapon(WeaponType type, Entity parent)
    {
        Entity e = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[type]);

        entityManager.SetComponentData(e, new Parent
        {
            Value = parent
        });
    }
}
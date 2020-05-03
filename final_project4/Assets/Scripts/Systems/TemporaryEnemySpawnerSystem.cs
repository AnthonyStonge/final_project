using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class TemporaryEnemySpawnerSystem : SystemBase
{
    private EntityManager entityManager;

    private ushort batchFilter = 0;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Pig, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.Pistol, e);
            }

        if (Input.GetKeyDown(KeyCode.O))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Rat, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.Pistol, e);
            }

        if (Input.GetKeyDown(KeyCode.I))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Chicken, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.Pistol, e);
            }

        if (Input.GetKeyDown(KeyCode.U))
            for (int i = 0; i < 10; i++)
            {
                CreateEnemy(Type.Gorilla, out Entity e);
                if (e != Entity.Null)
                    CreateWeapon(WeaponType.Pistol, e);
            }
    }

    private void CreateEnemy(Type type, out Entity e)
    {
        e = Entity.Null;

        //Make sure desired type exists
        if (!EnemyHolder.EnemyPrefabDict.ContainsKey(type))
            return;

        e = entityManager.Instantiate(EnemyHolder.EnemyPrefabDict[type]);

        //Set position
        entityManager.SetComponentData(e, new Translation
        {
            Value = new float3(Random.Range(1, 100), 0, Random.Range(1, 100))
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
            EnemyReachedTarget = true,
            ennemyState = EnnemyState.Wondering
        });

        //Clamp batch filter
        batchFilter++;
        batchFilter %= 8; //TODO REMOVE MAGIC NUMBER
    }

    private void CreateWeapon(WeaponType type, Entity parent)
    {
        Entity e = entityManager.Instantiate(WeaponHolder.WeaponPrefabDict[type]);

        entityManager.SetComponentData(e, new Parent
        {
            Value = parent
        });
    }
}
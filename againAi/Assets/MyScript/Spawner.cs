﻿using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Material = UnityEngine.Material;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    private EntityManager em;
    public GameObject EnemyGO;
    public Material mat;
    public Mesh mesh;
    public Material playerMat;
    public Mesh playerMesh;
    public ushort batch;
    public static Entity en;
    private BlobAssetStore blobAssetStore;
    // Start is called before the first frame update
    void Start()
    {
        blobAssetStore = new BlobAssetStore();
        en = GameObjectConversionUtility.ConvertGameObjectHierarchy(EnemyGO,
            GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore));
        batch = 0;
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        createPlayer();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < 50; i++)
            {
                createEntity(i % 50);
            }
            
        }
    }
    private void createEntity(int i)
    {
        EntityArchetype ea = em.CreateArchetype(typeof(Translation), typeof(RenderMesh), typeof(LocalToWorld), typeof(RenderBounds), typeof(PathFollow), typeof(PathFindingComponent), typeof(PathPosition), typeof(PhysicsCollider), typeof(PhysicsVelocity), typeof(PhysicsMass));
        Entity e = em.Instantiate(en);
        em.SetComponentData(e, new PathFindingComponent()
        {
            startPos = new int2(-1, -1),
            endPos = new int2(Random.Range(0,20), Random.Range(0,20))
        });
        em.SetComponentData(e, new Translation
        {
            Value = new float3(i,0,0)
        });
       
        /*em.SetSharedComponentData(e, new RenderMesh
        {
            mesh = mesh,
            material = mat
        });*/
        em.SetComponentData(e, new PathFollow
        {
            pathIndex = -1,
        });
        em.AddSharedComponentData(e, new BatchFilter
        {
            Value = batch++
        });
        batch %= 8;
    }
    private void createPlayer()
    {
        EntityArchetype player = em.CreateArchetype(typeof(Translation), typeof(RenderMesh), typeof(LocalToWorld), typeof(RenderBounds), typeof(PlayerTag));
        Entity ePlayer = em.CreateEntity(player);
        em.SetComponentData(ePlayer, new Translation
        {
            Value = new float3(Random.Range(0,50),0,Random.Range(0,50))
        });
        em.SetSharedComponentData(ePlayer, new RenderMesh
        {
            mesh = playerMesh,
            material = playerMat
        });

    }
    
    private void createFlockAgent()
    {
        EntityArchetype agent = em.CreateArchetype(typeof(Translation), typeof(RenderMesh), typeof(LocalToWorld), typeof(RenderBounds), typeof(PhysicsCollider), typeof(Rotation), typeof(PhysicsVelocity));
        Entity eAgent = em.CreateEntity(agent);
        em.SetComponentData(eAgent, new Translation
        {
            Value = new float3(Random.Range(0,50),0,Random.Range(0,50))
        });
        em.SetComponentData(eAgent, new Rotation()
        {
            Value = quaternion.identity
        });
        em.SetComponentData(eAgent, new PhysicsCollider()
        {
            
        });
        em.SetSharedComponentData(eAgent, new RenderMesh
        {
            mesh = playerMesh,
            material = playerMat
        });
        em.SetSharedComponentData(eAgent, new BatchFilter
        {
            Value = 1
        });
        BlobAssetReference<Unity.Physics.Collider> collider = Unity.Physics.BoxCollider.Create(
            new BoxGeometry
            {
                Size = new float3(1),
                Orientation = quaternion.identity
            },
            new CollisionFilter
            {
                BelongsTo = 1u << 0,
                CollidesWith = 1u << 2,
                GroupIndex = 0
            }
        );

        em.SetComponentData(eAgent, new PhysicsCollider
        {
            Value = Unity.Physics.SphereCollider.Create(
                new SphereGeometry()
                {
                    Center = float3.zero,
                    Radius = 2f
                    //Size = new float3(1),
                    //Orientation = quaternion.identity
                },
                new CollisionFilter
                {
                    BelongsTo = 1u << 0,
                    CollidesWith = 1u << 2,
                    GroupIndex = 0
                }
            )
        });

    }
    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
    // Update is called once per frame

}

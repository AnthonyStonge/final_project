using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
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
    private int spawnYCounter;
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
            spawnYCounter = 0;
            for (int i = 0; i < 1; i++)
            {
                createEntity(i % 25, spawnYCounter);
                if (i % 25 == 0)
                {
                    spawnYCounter++;
                }
            }
            
        }
    }
    private void createEntity(int i, int j)
    {
        Entity e = em.Instantiate(en);
        em.SetComponentData(e, new PathFindingComponent()
        {
            startPos = new int2(-1, -1),
        });
        em.SetComponentData(e, new Translation
        {
            Value = new float3(Random.Range(0,100),0,Random.Range(0,100))
        });
        em.AddComponentData(e, new PathFollowComponent
        {
            pathIndex = -1,
            EnemyReachedTarget = true
        });
        em.AddSharedComponentData(e, new BatchFilter
        {
            Value = batch++
        });
        em.AddSharedComponentData(e, new AnimationBatch
        {
            BatchId = AnimationHolder.AddAnimatedObject()
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
    
    /*private void createFlockAgent()
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

    }*/
    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }
    // Update is called once per frame

}

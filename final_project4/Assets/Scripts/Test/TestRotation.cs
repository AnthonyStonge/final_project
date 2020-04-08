using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    public Mesh playerMesh;
    public Material playerMat;

    public Mesh gunMesh;
    public Material gunMat;

    public Mesh bulletMesh;
    public Material bulletMat;

    private EntityArchetype playerArchetype;
    private EntityArchetype pistolArchetype;
    private EntityArchetype bulletArchetype;
    private Entity playerEntity;
    private Entity gunEntity;

    private EntityManager em;
    void Start()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        CreateArchetypes();
        //CreatePlayer();
        CreatePistol();
        GameVariables.BulletArchetype = bulletArchetype;
        GameVariables.BulletMesh = bulletMesh;
        GameVariables.BulletMat = bulletMat;
    }

    private void CreatePlayer()
    {
        playerEntity = em.CreateEntity(playerArchetype);
        
        em.SetComponentData(playerEntity, new SpeedData
        {
            Value = 2f
        });
        em.SetSharedComponentData(playerEntity, new RenderMesh
        {
            mesh = playerMesh,
            material = playerMat
        });
    }

    private void CreatePistol()
    {
        GunComponent newPistolComponent = new GunComponent
        {
            BetweenShotsTime = new TimeTrackerComponent(0.5f),
            ReloadTime = new TimeTrackerComponent(2f),
            BulletAmountPerShot = 1,
            MagasineSize = 30
        };
        
        gunEntity = em.CreateEntity(pistolArchetype);
        
        em.SetComponentData(gunEntity, new Translation
        {
            Value = new float3(0.3752352f, 0.1035824f, 0.3668232f)
        });
        
        em.SetComponentData(gunEntity, new NonUniformScale
        {
            Value = new float3(.2f, .2f, 1f)
        });
        
        em.SetComponentData(gunEntity, newPistolComponent);
        em.SetComponentData(gunEntity,new Parent
        {
            Value = playerEntity
        });
        
        em.SetSharedComponentData(gunEntity, new RenderMesh
        {
            mesh = gunMesh,
            material = gunMat
        });
    }

    private void CreateArchetypes()
    {
        playerArchetype = em.CreateArchetype(
            typeof(StateData),
            typeof(ForwardData), 
            typeof(SpeedData),
            typeof(LocalToWorld), 
            typeof(Rotation), 
            typeof(TargetData), 
            typeof(RenderBounds), 
            typeof(Translation),
            typeof(PlayerTag), 
            typeof(InputComponent), 
            typeof(RenderMesh));
        
        pistolArchetype = em.CreateArchetype(
            typeof(GunComponent), 
            typeof(NonUniformScale),
            typeof(PistolTag),
            typeof(Translation), 
            typeof(Rotation), 
            typeof(RenderMesh), 
            typeof(RenderBounds), 
            typeof(LocalToWorld),
            typeof(LocalToParent),
            typeof(Parent));

        bulletArchetype = em.CreateArchetype(
            typeof(Translation),
            typeof(Rotation),
            typeof(LocalToWorld),
            typeof(RenderBounds),
            typeof(SpeedData),
            typeof(ForwardData),
            typeof(BulletTag)
        );
    }
}
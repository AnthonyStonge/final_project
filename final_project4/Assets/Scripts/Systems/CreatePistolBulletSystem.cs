using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using EventStruct;

[UpdateAfter(typeof(PistolSystem))]
public class CreatePistolBulletSystem : SystemBase
{
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        //Create all entity required
        NativeArray<Entity> entities = entityManager.CreateEntity(StaticArchetypes.BulletArchetype,
            EventsHolder.PistolBulletToShoot.Length, Allocator.TempJob);

        //TODO INITIALIZE COMPONENTS WITH PARALLEL
        for (int i = 0; i < EventsHolder.PistolBulletToShoot.Length; i++)
        {
            Entity e = entities[i];
            BulletInfo info = EventsHolder.PistolBulletToShoot[i];
            
            entityManager.SetComponentData(e, new Translation
            {
                Value = info.Position
            });
            entityManager.SetComponentData(e, new Rotation
            {
                Value = info.Rotation
            });
            entityManager.SetSharedComponentData(e, new RenderMesh
            {
                mesh = GameVariables.PistolVars.Bullet.mesh,
                material = GameVariables.PistolVars.Bullet.mat
            });
            entityManager.SetComponentData(e, new DamageProjectile
            {
                Speed = GameVariables.PistolVars.Bullet.Speed
            });

            
            entityManager.SetName(e, "Pistol Bullet");
        }

        entities.Dispose();
    }
}
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static GameVariables;

[DisableAutoCreation]
public class PistolSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endECB;

    protected override void OnCreate()
    {
        endECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer.Concurrent ecb = endECB.CreateCommandBuffer().ToConcurrent();

        float deltaTime = Time.DeltaTime;

        StateActions state = PlayerVars.CurrentState;

        int magazineSize = PistolVars.MagazineSize;
        float betweenShotTime = PistolVars.BetweenShotTime;
        float reloadTime = PistolVars.ReloadTime;

        Entities.ForEach((int entityInQueryIndex, ref PistolComponent pistol, in LocalToWorld trans) =>
        {
            if (pistol.IsReloading)
            {
                pistol.ReloadTime -= deltaTime;
                if (pistol.ReloadTime <= 0)
                {
                    pistol.CurrentBulletInMagazine = magazineSize;
                }
            }
            else if (state == StateActions.ATTACKING && !pistol.IsBetweenShot)
            {
                pistol.BetweenShotTime = betweenShotTime;
                pistol.CurrentBulletInMagazine--;
                if (pistol.CurrentBulletInMagazine == 0)
                {
                    pistol.ReloadTime = reloadTime;
                }

                CreateBullet(ecb, entityInQueryIndex, pistol.bullet, trans);
            }
            else if (pistol.IsBetweenShot)
            {
                pistol.BetweenShotTime -= deltaTime;
            }
        }).ScheduleParallel();

        endECB.AddJobHandleForProducer(Dependency);
    }

    private static void CreateBullet(EntityCommandBuffer.Concurrent ecb, int index, Entity e, in LocalToWorld trans)
    {
        Entity entity = ecb.Instantiate(index, e);
        ecb.RemoveComponent(index, entity, typeof(Disabled));
        //Debug.Log("Creating entity... ID: " + entity);
        
        ecb.SetComponent(index, entity, new Scale
        {
            Value = 0.35f
        });
        ecb.SetComponent(index, entity, new Translation
        {
            Value = trans.Position
        });
        ecb.SetComponent(index, entity, new Rotation
        {
            Value = trans.Rotation
        });
        
        // BlobAssetReference<Unity.Physics.Collider> collider = Unity.Physics.BoxCollider.Create(
        // BlobAssetReference<Unity.Physics.Collider> collider = Unity.Physics.BoxCollider.Create(
        //     new BoxGeometry
        //     {
        //         Size = new float3(1),
        //         Orientation = quaternion.identity
        //     },
        //     new CollisionFilter
        //     {
        //         BelongsTo = 1u << 0,
        //         CollidesWith = 1u << 2,
        //         GroupIndex = 0
        //     }
        // );

        // ecb.SetComponent(index, entity, new PhysicsCollider
        // {
        //     Value = collider
        // });
    }
}


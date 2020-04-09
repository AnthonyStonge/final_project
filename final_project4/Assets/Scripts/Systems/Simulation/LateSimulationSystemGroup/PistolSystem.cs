using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static GameVariables;

[DisableAutoCreation]
public class PistolSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endECB;
    private static RenderMesh pistolRender;
    protected override void OnCreate()
    {
        endECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        pistolRender = new RenderMesh
        {
            mesh = PistolVars.Bullet.mesh,
            material = PistolVars.Bullet.mat
        };
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer.Concurrent ecb = endECB.CreateCommandBuffer().ToConcurrent();
        Entities.ForEach((int entityInQueryIndex, ref PistolComponent pistol, in Translation trans, in Rotation rot) =>
        {
           /* if (PlayerVars.CurrentState == StateActions.ATTACKING && pistol.CanShoot)
            {
                Debug.Log("PistolShooting");
                pistol.BetweenShotTime.Reset();
                pistol.CurrentBulletInMagazine--;
                CreateBullet(ecb, entityInQueryIndex, trans, rot);
            }*/
        }).ScheduleParallel();

        endECB.AddJobHandleForProducer(Dependency);
    }

    private static void CreateBullet(EntityCommandBuffer.Concurrent ecb, int index, in Translation trans, in Rotation rot)
    {
        Entity e = ecb.CreateEntity(index, StaticArchetypes.BulletArchetype);
        //ecb.SetSharedComponent(index, e, pistolRender);
        ecb.SetComponent(index, e, new TimeTrackerComponent(PistolVars.Bullet.LifeTime));
        ecb.SetComponent(index, e, new SpeedData
        {
            Value = PistolVars.Bullet.Speed
        });
        ecb.SetComponent(index, e, new Translation
        {
            Value = trans.Value
        });
        ecb.SetComponent(index, e, new Rotation
        {
            Value = rot.Value
        });
    }
}

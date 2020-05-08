using Enums;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

public struct Boost : IComponentData
{
    public float Time;
}

public class SecretSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float dt = Time.DeltaTime;
        Entities.WithStructuralChanges().WithoutBurst().ForEach((Entity entity, ref Boost boost) =>
        {
            Debug.Log("wot");
            boost.Time -= dt;
            GameVariables.Boost = 10;
            var gun = GameVariables.Player.PlayerWeaponEntities[WeaponType.Shotgun];
            GunComponent gunComponent = EntityManager.GetComponentData<GunComponent>(gun);
            gunComponent.CurrentAmountBulletInMagazine = gunComponent.MaxBulletInMagazine;
            if (boost.Time <= 0)
            {
                GameVariables.Boost = 1;
                EntityManager.DestroyEntity(entity);
                gunComponent.HasInfiniteAmmo = false;
                gunComponent.ReloadTime *= 4f;
                gunComponent.BetweenShotTime *= 4f;
            }
            EntityManager.SetComponentData(gun, gunComponent);
        }).Run();
    }
}
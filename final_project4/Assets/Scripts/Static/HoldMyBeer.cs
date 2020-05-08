using Enums;
using Unity.Entities;
using UnityEngine;
namespace Static
{
    public static class HoldMyBeer
    {
        private static bool E;
        private static bool C;
        private static bool S;
        public static void ImBoosting(EntityManager entityManager, bool on)
        {
            if (!on)
            {
                E = false;
                C = false;
                S = false;
                return;
            }
            if (E || Input.GetKeyDown(KeyCode.E))
            {
                E = true;
                if (C || Input.GetKeyDown(KeyCode.C))
                {
                    C = true;
                    if (S || Input.GetKeyDown(KeyCode.S))
                    {
                        S = true;
                    }
                }
            }
            if (S)
            {
                var e = entityManager.CreateEntity(new ComponentType[] {typeof(Boost)});
                entityManager.SetComponentData(e, new Boost
                {
                    Time = 10f
                });
                var gun = GameVariables.Player.PlayerWeaponEntities[WeaponType.Shotgun];
                GunComponent gunComponent = entityManager.GetComponentData<GunComponent>(gun);
                gunComponent.HasInfiniteAmmo = true;
                gunComponent.BetweenShotTime *= 0.25f;
                entityManager.SetComponentData(gun, gunComponent);
                E = false;
                C = false;
                S = false;
            }
        }
    }
}
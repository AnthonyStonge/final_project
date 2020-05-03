using Enums;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class InvincibleSystem : SystemBase
{
    private static float InvincibleDashTime;
    private static float InvincibleDeathTime;
    private static float InvincibleSpawnTime;
    private static float InvicibleHitTime;

    protected override void OnCreate()
    {
        //TODO Change these values for real values
        InvincibleDashTime = InvincibleDeathTime = InvincibleSpawnTime = InvicibleHitTime = 1.0f;
    }

    protected override void OnUpdate()
    {
        float delta = Time.DeltaTime;

        Entities.ForEach((ref InvincibleData invincible, ref LifeComponent life) =>
        {
            if (life.Invincibility != InvincibilityType.None)
            {
                var newTimer = 0f;
                switch (life.Invincibility)
                {
                    case InvincibilityType.Dash:
                        newTimer = InvincibleDashTime;
                        break;
                    case InvincibilityType.Death:
                        newTimer = InvincibleDeathTime;
                        break;
                    case InvincibilityType.Hit:
                        newTimer = InvicibleHitTime;
                        break;
                    case InvincibilityType.Spawn:
                        newTimer = InvincibleSpawnTime;
                        break;
                    default:
                        #if UNITY_EDITOR
                        Debug.Log("Should not happend, InvincibilityType non existing");
                        #endif
                        break;
                }

                invincible.Timer = newTimer;
                life.SetInvincibility(InvincibilityType.None);
            }
            else if (life.IsInvincible)
            {
                if (invincible.Timer <= 0)
                {
                    life.StopInvincibility();
                }
                else
                {
                    invincible.Timer -= delta;
                }
            }
        }).ScheduleParallel();
    }
}
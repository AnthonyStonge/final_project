using System.Collections;
using System.Collections.Generic;
using EventStruct;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[UpdateAfter(typeof(ProjectileHitDetectionSystem))]
public class VisualEventSystem : SystemBase
{
    private Texture2D texture;
    private Color[] colorPositions;
    protected override void OnCreate()
    {
        colorPositions = new Color[2048];
        texture = new Texture2D(2048, 1, TextureFormat.RFloat, false);
    }

    protected override void OnUpdate()
    {
        //Take event and play it at desired position
        //Weapons
        // foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        // {
        //     if (info.EventType == WeaponInfo.WeaponEventType.ON_SHOOT)
        //         VisualEffectManager.PlayVisualEffect(info);
        // }

        for (int i = 0; i < EventsHolder.BulletsEvents.Length; i++)
        {
            float3 position = EventsHolder.BulletsEvents[i].HitPosition;
            colorPositions[i] = new Color(position.x, position.y, position.z);
        }
        texture.Apply();
        //Bullets
        // foreach (BulletInfo info in EventsHolder.BulletsEvents)
        // {
        //     VisualEffectManager.PlayVisualEffect(info);
        // }
    }

    protected override void OnDestroy()
    {
        Object.Destroy(texture);
    }
}
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

[DisableAutoCreation]
// [UpdateAfter(typeof(ProjectileHitDetectionSystem))]
public class VisualEventSystem : SystemBase
{
    private Texture2D texture;
    private Color[] colorPositions;

    private int propertyTexture;
    private int propertyCount;

    private VisualEffect visualEffect;
    protected override void OnCreate()
    {
        //Might need to initialize it
        colorPositions = new Color[2048];
        propertyTexture = VisualEffectHolder.PropertyTexture;
        propertyCount = VisualEffectHolder.PropertyCount;
        texture = new Texture2D(2048, 1, TextureFormat.RGBAFloat, false);
        visualEffect = VisualEffectHolder.ProjectileVFXDict[ProjectileType.PistolBullet];
        EventsHolder.BulletsEvents.Clear();
    }

    protected override void OnUpdate()
    {
        var count = EventsHolder.BulletsEvents.Length;

        Debug.Log(count);
        // Takes only 2048 event from the last one
        var min = math.clamp(count - 2048, 0, count);
        
        for (var i = min; i < count; i++)
        {
            //Could cache EventsHolder maybe?
            float3 position = EventsHolder.BulletsEvents[i].HitPosition;
            Color color = colorPositions[i];
            color.r = position.x;
            color.g = position.y;
            color.b = position.z;
            
            //Note https://answers.unity.com/questions/266170/for-different-texture-sizes-which-is-faster-setpix.html
            // Says that setPixels32 is 15x faster than setPixel
            texture.SetPixel(i, 0, new Color(position.x,position.y, position.z));
        }
        texture.Apply();

        visualEffect.SetTexture(propertyTexture, texture);
        visualEffect.SetInt(propertyCount, count);
        visualEffect.Play();
        EventsHolder.BulletsEvents.Clear();

    }

    protected override void OnDestroy()
    {
        Object.Destroy(texture);
    }
}
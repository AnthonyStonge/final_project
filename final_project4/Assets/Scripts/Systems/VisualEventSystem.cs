using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.VFX;

[DisableAutoCreation]
public class VisualEventSystem : SystemBase
{
    private Texture2D texture;
    private Color[] colorPositions;

    private int propertyTexture;
    private int propertyCount;
    private NativeList<BulletInfo> bulletsEvents;
    private VisualEffect visualEffect;
    protected override void OnCreate()
    {
        //Caching BulletsEvents Reference
        bulletsEvents = EventsHolder.BulletsEvents;
        bulletsEvents.Clear();
        
        //Init position array
        colorPositions = new Color[2048];
        
        //Get Property ID
        propertyTexture = VisualEffectHolder.PropertyTexture;
        propertyCount = VisualEffectHolder.PropertyCount;
        
        /*
        Create texture. Will only take 2048 positions,
        texture RGBAFloat is the key here. It allows Colors to have any values.
        */
        texture = new Texture2D(2048, 1, TextureFormat.RGBAFloat, false);
        visualEffect = VisualEffectHolder.ProjectileVFXDict[ProjectileType.PistolBullet];
        
        //Since Texture is a reference type, it can be linked once in the shader
        visualEffect.SetTexture(propertyTexture, texture);
        texture.name = "Bullets";
    }

    protected override void OnUpdate()
    {
        //Get Event Count
        var count = bulletsEvents.Length;
        
        //Tell the Visual Effect How many bullets there is
        visualEffect.SetInt(propertyCount, count);
    
        //Returns if there's no event
        if (count == 0) return;
        
        // Takes only the 2048 event (max) from the last one
        var min = math.clamp(count - 2048, 0, count);
        
        for (var i = min; i < count; i++)
        {
            var position = bulletsEvents[i].HitPosition;
            var color = colorPositions[i];
            color.r = position.x;
            color.g = position.y;
            color.b = position.z;
            colorPositions[i] = color;
        }
        //SetPixels is up to 3x faster then SetPixel
        texture.SetPixels(0, 0, count, 1, colorPositions);
        texture.Apply();
    
        visualEffect.Play();
    }
    
    //To make Trails
    /*protected override void OnUpdate()
    {
        int count = 0;
        Entities.WithoutBurst().ForEach((ref DamageProjectile damageProjectile, ref Translation translation) =>
        {
            var t = translation.Value;
            texture.SetPixel(count, 0, new Color(t.x, t.y, t.z));
            count++;
        }).Run();
        texture.Apply();

        //Tell the Visual Effect How many bullets there is
        visualEffect.SetInt(propertyCount, count);
        //SetPixels is up to 3x faster then SetPixel

        visualEffect.Play();
        
        //TODO Clear events all at the same place
        bulletsEvents.Clear();
    }*/
       
    protected override void OnDestroy()
    {
        Object.Destroy(texture);
    }
}
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
    public struct EffectTexture
    {
        public Texture2D TexturePositions;
        public Texture2D TextureRotations;
        
        public Color[] Positions;
        public Color[] Rotations;
        private int indexAt;
        
        public int Count => indexAt + 1;

        public void Reset()
        {
            indexAt = 0;
        }
        public void Add(float3 position, float3 rotation)
        {
            //To prevent adding element outside of the array
            if(indexAt >= Positions.Length) return;
            
            var color = Positions[indexAt];
            color.r = position.x;
            color.g = position.y;
            color.b = position.z;
            Positions[indexAt] = color;
            
            color = Rotations[indexAt];
            color.r = rotation.x;
            color.g = rotation.y;
            color.b = rotation.z;
            Rotations[indexAt] = color;

            indexAt++;
        }

        public bool Set()
        {
            if (Count <= 0) return false;
            TexturePositions.SetPixels(0, 0, Count, 1, Positions);
            TextureRotations.SetPixels(0, 0, Count, 1, Rotations);
            
            TexturePositions.Apply();
            TextureRotations.Apply();
            return true;
        }
    }
    private Dictionary<int, EffectTexture> effectTextures;
    
    protected override void OnCreate()
    {
        
        effectTextures = new Dictionary<int, EffectTexture>();
        
        foreach (var effect in VisualEffectHolder.Effects)
        {
            effectTextures.Add(effect.Key, new EffectTexture
            {
                Positions = new Color[effect.Value.MaxAmount],
                Rotations = new Color[effect.Value.MaxAmount],
                TexturePositions = new Texture2D(effect.Value.MaxAmount, 1, TextureFormat.RGBAFloat, false),
                TextureRotations = new Texture2D(effect.Value.MaxAmount, 1, TextureFormat.RGBAFloat, false),
            });
            
            //TODO Set name to textures
            effect.Value.VisualEffect.SetTexture(VisualEffectHolder.PropertyTexturePosition, effectTextures[effect.Key].TexturePositions);
            effect.Value.VisualEffect.SetTexture(VisualEffectHolder.PropertyTextureRotation, effectTextures[effect.Key].TextureRotations);
        }
    }

    protected override void OnUpdate()
    {
        foreach (var info in EventsHolder.BulletsEvents)
        {
            effectTextures[VisualEffectHolder.BulletEffects[info.ProjectileType][info.CollisionType]].
                Add(info.HitPosition, math.forward(info.HitRotation) );
        }

        foreach (var effect in effectTextures)
        {
            if (!effect.Value.Set()) continue;
            VisualEffectHolder.Effects[effect.Key].VisualEffect.SetInt(VisualEffectHolder.PropertyCount, effect.Value.Count);
            VisualEffectHolder.Effects[effect.Key].VisualEffect.Play();
        }
    }

    protected override void OnDestroy()
    {
        foreach (var effect in effectTextures.Values)
        {
            Object.Destroy(effect.TexturePositions);
            Object.Destroy(effect.TextureRotations);
        }
    }
}
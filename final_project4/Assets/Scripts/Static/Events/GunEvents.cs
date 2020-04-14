using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Static.Events
{
    public static class GunEvents
    {
        public delegate void ShootPistol(float3 position, quaternion dir);

        private static EntityManager entityManager;
        private static MonoGameVariables gameVariables;

        public static ShootPistol OnShootPistol;

        public static void Initialize()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            gameVariables = MonoGameVariables.instance;

            //OnShootPistol
            OnShootPistol = CreatePistolBullet;
        }

        private static Entity CreateEntity(float3 position, quaternion dir, RenderMesh renderMesh)
        {
            //Create entity
            Entity e = entityManager.CreateEntity(StaticArchetypes.BulletArchetype);

            //Set components
            entityManager.SetComponentData(e, new Translation
            {
                Value = position
            });
            entityManager.SetComponentData(e, new Rotation
            {
                Value = dir
            });
            entityManager.SetSharedComponentData(e, renderMesh);
            
            //
            return e;
        }

        private static void CreatePistolBullet(float3 position, quaternion dir)
        {
            RenderMesh renderMesh = new RenderMesh
            {
                mesh = gameVariables.BulletMesh,
                material = gameVariables.BulletMaterial
            };
            Entity e = CreateEntity(position, dir, renderMesh);
            
            //Set name
            entityManager.SetName(e, "Pistol Bullet");
            //Add personal tag
            entityManager.AddComponent<PistolTag>(e);
        }
    }
}
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
        public delegate void ShootPistol(float3 position, quaternion rotation);

        private static EntityManager entityManager;

        public static ShootPistol OnShootPistol;

        public static void Initialize()
        {
            //TODO REMOVE PLS ITS DISGUSTING LOL
            GameVariables.PistolVars.Bullet.mesh = MonoGameVariables.instance.BulletMesh;
            GameVariables.PistolVars.Bullet.mat = MonoGameVariables.instance.BulletMaterial;
            
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            //OnShootPistol
            OnShootPistol = CreatePistolBullet;
            
        }

        private static Entity CreateEntity(float3 position, quaternion rotation, RenderMesh renderMesh)
        {
            //TODO Instantiate from "Prefab"
            //Create entity
            Entity e = entityManager.CreateEntity(StaticArchetypes.BulletArchetype);

            //Set components
            entityManager.SetComponentData(e, new Translation
            {
                Value = position
            });
            entityManager.SetComponentData(e, new Rotation
            {
                Value = rotation
            });
            entityManager.SetSharedComponentData(e, renderMesh);
            
            //
            return e;
        }

        private static void CreatePistolBullet(float3 position, quaternion dir)
        {
            RenderMesh renderMesh = new RenderMesh
            {
                mesh = GameVariables.PistolVars.Bullet.mesh,
                material = GameVariables.PistolVars.Bullet.mat
            };
            Entity e = CreateEntity(position, dir, renderMesh);
            
            //Set speed
            entityManager.SetComponentData(e, new DamageProjectile()
            {
                Speed = GameVariables.PistolVars.Bullet.Speed
            });
            
            //Set name
            entityManager.SetName(e, "Pistol Bullet");
            
            //Add personal tag
            entityManager.AddComponent<PistolTag>(e);
        }
    }
}
﻿using Enums;
using Holders;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using static GameVariables;

namespace Static.Events
{
    public static class PlayerEvents
    {
        public delegate void PlayerSpawn(float3 position, quaternion rotation, short health);

        public delegate void PlayerDeath(float3 position);

        public delegate void PlayerRespawn(float3 position, quaternion rotation);


        //Variables
        private static EntityManager entityManager;

        //Events
        public static PlayerSpawn OnPlayerSpawn;
        public static PlayerDeath OnPlayerDeath;
        public static PlayerRespawn OnPlayerRespawn;


        public static void Initialize()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            //Player spawn
            OnPlayerSpawn = SpawnPlayer; //Create entity
            //Play spawn sound?
            //Play spawn shader?

            //Player death
            OnPlayerDeath = (float3 position) => { DeathPlayer(); }; //Toggle to inactive
            //Disable inputs

            //Player respawn
            OnPlayerRespawn = RespawnPlayer; //Toggle to active
            //Enable inputs
        }

        private static void SpawnPlayer(float3 position, quaternion rotation, short health)
        {
            //Create player entity
            Entity player = entityManager.CreateEntity(StaticArchetypes.PlayerArchetype);
            entityManager.SetName(player, "Player");

            //Set Values
            entityManager.SetComponentData(player, new Translation
            {
                Value = position
            });
            entityManager.SetComponentData(player, new Rotation
            {
                Value = rotation
            });
            entityManager.SetComponentData(player, new HealthData
            {
                Value = health
            });
            entityManager.SetComponentData(player, new SpeedData
            {
                Value = PlayerVars.Default.DefaultSpeed
            });
            StateActions state = PlayerVars.Default.UseDebugVariables
                ? PlayerVars.Default.StartingState
                : StateActions.IDLE;
            entityManager.SetComponentData(player, new StateData
            {
                Value = state
            });
            entityManager.SetComponentData(player, new DashComponent
            {
                Distance = PlayerVars.Dash.DefaultLength,
                Timer = new TimeTrackerComponent
                {
                    ResetValue = PlayerVars.Dash.DefaultResetTime
                }
            });
            entityManager.SetSharedComponentData(player, PlayerVars.Default.RenderMesh);

            //Set info in GameVariables
            PlayerVars.Entity = player;
            PlayerVars.CurrentPosition = position;
            PlayerVars.CurrentState = state;
            PlayerVars.CurrentHealth = health;
            PlayerVars.IsAlive = PlayerVars.CurrentHealth > 0;
            PlayerVars.CurrentSpeed = PlayerVars.Default.DefaultSpeed;
        }

        private static void DeathPlayer()
        {
            //Toggle obj to inactive (not destroying obj to avoid creation cost)
            entityManager.SetEnabled(GameVariables.PlayerVars.Entity, false);
        }

        private static void RespawnPlayer(float3 position, quaternion rotation)
        {
            Entity player = GameVariables.PlayerVars.Entity;

            //Toggle obj to active
            entityManager.SetEnabled(player, true);

            //Set components values
            entityManager.SetComponentData(player, new Translation
            {
                Value = position
            });
            entityManager.SetComponentData(player, new Rotation
            {
                Value = rotation
            });
            entityManager.SetComponentData(player, new HealthData
            {
                Value = GameVariables.PlayerVars.Default.DefaultHealth
            });
        }
    }
}
using System;
using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public static class GlobalEvents
{
    public static class GameEvents
    {
        public static bool TogglePauseGame = true;

        public static void PauseGame()
        {
            var world = World.DefaultGameObjectInjectionWorld;

            TogglePauseGame = !TogglePauseGame;
#if UNITY_EDITOR
            Debug.Log(TogglePauseGame);
#endif
            DisableGameLogic(TogglePauseGame);

            ShowPauseMenu();
        }

        public static void DisableGameLogic(bool deactivate)
        {
            var world = World.DefaultGameObjectInjectionWorld;
            
            world.GetOrCreateSystem<LateInitializeManager>().Enabled = !deactivate;
            world.GetOrCreateSystem<TransformSimulationManager>().Enabled = !deactivate;
            world.GetOrCreateSystem<LateSimulationManager>().Enabled = !deactivate;
            world.GetOrCreateSystem<PresentationManager>().Enabled = !deactivate;
            world.GetOrCreateSystem<SimulationSystemGroup>().Enabled = !deactivate;
        }

        private static void ShowPauseMenu()
        {
            GameVariables.UI.PausedMenu.SetActive(!TogglePauseGame);
        }

        public static void Destroy<T>() where T : IComponentData
        {
            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

            //Create Query
            EntityQuery query = manager.CreateEntityQuery(typeof(T));
            using (var entities = query.ToEntityArray(Allocator.TempJob))
            {
                //Destroy entities with query
                manager.DestroyEntity(entities);
            }
        }

        public static void StartHellLevel(int difficulty, int deathCount)
        {
            //TODO explain to player he needs to survive X amount of time to respawn
#if UNITY_EDITOR
            Debug.Log("Current Difficulty : " + difficulty + ", Current Death Count : " + deathCount);
#endif

            //TODO start Soundtrack for hell level
            MapEvents.LoadMap(MapType.Level_Hell, true);
            UIManager.ResetPlayerHealth();
        }

        public static void RestartGame()
        {
            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            //Toggle UI YOU LOST
            //TODO

            //Toggle PlayerWeapons
            ChangeWorldDelaySystem.OnChangeWorld += () =>
            {
                SwapWeaponSystem.SwapWeaponBetweenWorld(WeaponType.Pistol, MapType.Level_Hell, MapType.LevelMenu);
                
                //Reset UI
                UIManager.ResetPlayerHealth();
                UIManager.ToggleHellTimers(false);
            };

            //Reset Life
            LifeComponent playerLife = manager.GetComponentData<LifeComponent>(GameVariables.Player.Entity);
            playerLife.Reset();
            manager.SetComponentData(GameVariables.Player.Entity, playerLife);
            //Reset Weapons ammo
            WeaponInitializer.Initialize();
            UIManager.ReloadAllWeapons();
            UIManager.SetWeaponType(WeaponType.Pistol);
            //Reset Death count
            //TODO
            //Load Menu MapType
            MapEvents.LoadMap(MapType.LevelMenu, true);
        }

        public static void QuitApplication()
        {
            Application.Quit();
        }

        public static void OnSwapLevel()
        {
            Destroy<AmmunitionComponent>();
            Destroy<BulletTag>();
            Destroy<EnemyTag>();
        }
    }

    public static class PlayerEvents
    {
        public static void ResetPlayerHp()
        {
            Entity player = GameVariables.Player.Entity;
            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            LifeComponent life = manager.GetComponentData<LifeComponent>(player);
            life.Reset();
            manager.SetComponentData(player, life);
        }

        public static void SetPlayerPosition(float3 position)
        {
            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            manager.SetComponentData(GameVariables.Player.Entity, new Translation
            {
                Value = position
            });
        }

        public static void SetPlayerRotation(quaternion rotation)
        {
            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            manager.SetComponentData(GameVariables.Player.Entity, new Rotation
            {
                Value = rotation
            });
        }

        public static void LockUserInputs()
        {
            //Get input components / Player entity
            EntityManager e = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = GameVariables.Player.Entity;

            //Set disable
            InputComponent inputs = e.GetComponentData<InputComponent>(entity);
            inputs.Enabled = false;
            e.SetComponentData(entity, inputs);
        }

        public static void UnlockUserInputs()
        {
            //Get input components / Player entity
            EntityManager e = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = GameVariables.Player.Entity;

            //Set enable
            InputComponent inputs = e.GetComponentData<InputComponent>(entity);
            inputs.Enabled = true;
            e.SetComponentData(entity, inputs);
        }

        public static void SetInvincibility(InvincibilityType type)
        {
            EntityManager e = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entity = GameVariables.Player.Entity;

            //Get Player LifeComponent
            LifeComponent life = e.GetComponentData<LifeComponent>(entity);
            life.SetInvincibility(type);
            e.SetComponentData(entity, life);
        }

        public static void SetDelayOnPlayerWeapon()
        {
            EntityManager e = World.DefaultGameObjectInjectionWorld.EntityManager;

            //Set delay on Player Weapon (Quick fix for bullets spawning at wrong spot)
            Entity weaponEntity = EventsHolder.LevelEvents.CurrentLevel != MapType.Level_Hell
                ? GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]
                : GameVariables.Player.PlayerHellWeaponEntities[GameVariables.Player.CurrentWeaponHeld];
            GunComponent weapon = e.GetComponentData<GunComponent>(weaponEntity);
            weapon.SwapTimer = 0.02f;
            e.SetComponentData(weaponEntity, weapon);
        }
    }

    public static class CameraEvents
    {
        public static void FadeIn()
        {
            SetFadeInfo(FadeObject.FadeType.FadeIn, 1);
        }

        public static void FadeOut()
        {
            SetFadeInfo(FadeObject.FadeType.FadeOut, 0);
        }

        private static void SetFadeInfo(FadeObject.FadeType type, float startValue)
        {
            //Set fade component info
            GameVariables.UI.FadeObject.FadeValue = startValue;
            GameVariables.UI.FadeObject.Type = type;

            //Turn on fade system
            World.DefaultGameObjectInjectionWorld.GetExistingSystem<FadeSystem>().Enabled = true;
        }

        public static void ShakeCam(float time, float shakeAmplitude, float shakeFrequency)
        {
            //Set fade component info
            GameVariables.ShakeComponent.CamShakeDuration = time;
            GameVariables.ShakeComponent.ShakeAmplitude = shakeAmplitude;
            GameVariables.ShakeComponent.ShakeFrequency = shakeFrequency;

            //Turn on fade system
            World.DefaultGameObjectInjectionWorld.GetExistingSystem<ShakeCamSystem>().Enabled = true;
        }
    }
}
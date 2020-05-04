using System;
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
            //world.GetOrCreateSystem<InitializeManager>().Enabled = TogglePauseGame;
            world.GetOrCreateSystem<LateInitializeManager>().Enabled = TogglePauseGame;
            world.GetOrCreateSystem<TransformSimulationManager>().Enabled = TogglePauseGame;
            world.GetOrCreateSystem<LateSimulationManager>().Enabled = TogglePauseGame;
            world.GetOrCreateSystem<PresentationManager>().Enabled = TogglePauseGame;
            //world.GetOrCreateSystem<BuildPhysicsWorld>().Enabled = TogglePauseGame;

            world.GetOrCreateSystem<SimulationSystemGroup>().Enabled = TogglePauseGame;

            ShowPauseMenu();
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

            //Destroy entities with query
            manager.DestroyEntity(query);
        }

        public static void StartHellLevel(int difficulty, int deathCount)
        {
            #if UNITY_EDITOR
            Debug.Log("Current Difficulty : " + difficulty + ", Current Death Count : " + deathCount);
            #endif

            //TODO start Soundtrack for hell level
            MapEvents.LoadMap(MapType.Level_Hell);
        }

        public static void GameLost()
        {
            //TODO Enable Game Over UI for short amount of time.
            //Then show player in Menu Level.

            //Return player to Menu
            MapEvents.LoadMap(MapType.LevelMenu);

            //Set Player position to Menu CheckPoint

            //TODO Reset GameValue???
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
        public static void OnPlayerDie()
        {
#if UNITY_EDITOR
            Debug.Log("On Player Death");
#endif
            
            /* GameVariables.Player.AmountLife--;
 
             //Look if Player should respawn in current level
             if (GameVariables.Player.AmountLife <= 0)
                 GameEvents.GameLost();
             else
                 RespawnPlayerOnCheckPoint();*/

            //Number of the time the player dies defines the difficulties of "Hell Level"

            //TODO Sound Event
            //TODO VFX 
        }

        private static void SpawnPlayerOnHellLevel()
        {
        }

        private static void RespawnPlayerOnCheckPoint()
        {
            //Reset Life of Player
            ResetPlayerHp();

            //Get last saved spawn position
        }

        private static void SpawnOnCheckPoint(MapType mapType, ushort checkPointId)
        {
        }

        private static void ResetPlayerHp()
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
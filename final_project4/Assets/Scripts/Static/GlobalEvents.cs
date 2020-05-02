using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public static class GlobalEvents
{
    public static class GameEvents
    {
        public static void PauseGame()
        {
        
        }

        public static void UnPauseGame()
        {
        
        }

        public static void DestroyAllEnemies()
        {
            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
            //Create Query
            EntityQuery query = manager.CreateEntityQuery(typeof(EnemyTag));
        
            //Destroy entities with query
            manager.DestroyEntity(query);
        }

        public static void GameLost()
        {
            
        }
    }
    
    public static class PlayerEvents
    {
        public static void OnPlayerDie()
        {
            //Look if Player should respawn in current level
            if(GameVariables.Player.AmountLife <= 0)
                GameEvents.GameLost();
                
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

        public static void LockUserInputs(ref InputComponent inputs)
        {
            inputs.Enabled = false;
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
    
        public static void UnlockUserInputs(ref InputComponent inputs)
        {
            inputs.Enabled = true;
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

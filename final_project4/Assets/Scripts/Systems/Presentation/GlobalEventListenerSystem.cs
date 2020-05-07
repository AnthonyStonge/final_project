using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class GlobalEventListenerSystem : SystemBase
{
    private float hellTimer = 30f;
    private float currentHellTimer = 0f;
    private MapType LastMap;

    protected override void OnUpdate()
    {
        var playerLifeComponent = EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity);
        var levelEvents = EventsHolder.LevelEvents;
        if (levelEvents.CurrentLevel == MapType.Level_Hell)
        {
            if (currentHellTimer <= 0)
            {
                //TODO Hell level Succesful UI
#if UNITY_EDITOR
                Debug.Log("Player Survived loading last level");
#endif
                MapEvents.LoadMap(LastMap, true);
                UIManager.ResetPlayerHealth();
                UIManager.ToggleHellTimers(false);
                currentHellTimer = hellTimer;
            }
            else
            {
                currentHellTimer -= Time.DeltaTime;
                UIManager.SetTimeOnHellTimers(currentHellTimer);
            }
        }
        else if (levelEvents.CurrentLevel == MapType.LevelMenu)
        {
            //Default Menu Logic
        }
        else
        {
            //Default Level Logic
        }
      
        if (playerLifeComponent.Invincibility == InvincibilityType.Hit)
        {
            GlobalEvents.CameraEvents.ShakeCam(.2f, 1, 1.5f);
        }
        
        //Look for player hp
        if (playerLifeComponent.IsDead())
        {
            if (levelEvents.CurrentLevel != MapType.Level_Hell)
            {
                
                playerLifeComponent.Reset();
                EntityManager.SetComponentData(GameVariables.Player.Entity, playerLifeComponent);
                
                LastMap = EventsHolder.LevelEvents.CurrentLevel;
                EventsHolder.LevelEvents.DeathCount++;
                levelEvents.CurrentLevel = MapType.Level_Hell;
               // GlobalEvents.PlayerEvents.OnPlayerDie();
                GlobalEvents.GameEvents.StartHellLevel(EventsHolder.LevelEvents.Difficulty,
                    EventsHolder.LevelEvents.DeathCount);

               // EventsHolder.LevelEvents.CurrentLevel = MapType.Level_Hell;
                EventsHolder.LevelEvents.LevelEvent = LevelInfo.LevelEventType.OnStart;
                UIManager.ResetPlayerHealth();
                UIManager.ToggleHellTimers(true);

                currentHellTimer = hellTimer;
            }
            else
            {
                currentHellTimer = hellTimer;    //??
                GlobalEvents.GameEvents.GameLost();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class GlobalEventListenerSystem : SystemBase
{
    private float hellTimer = 20f;
    private float currentHellTimer = 0f;
    private MapType LastMap;

    protected override void OnUpdate()
    {
        var levelEvents = EventsHolder.LevelEvents;
        if (levelEvents.CurrentLevel == MapType.Level_Hell)
        {
            if (currentHellTimer >= hellTimer)
            {
                //TODO Hell level Succesful
#if UNITY_EDITOR
                Debug.Log("Player Survived loading last level");
#endif
                MapEvents.LoadMap(LastMap);
            }
            else
            {
                currentHellTimer += Time.DeltaTime;
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

        //Look for player hp
        if (EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity).Life.Value <= 0)
        {
            if (levelEvents.CurrentLevel != MapType.Level_Hell)
            {
                EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity).Reset();
                
                LastMap = EventsHolder.LevelEvents.CurrentLevel;
                EventsHolder.LevelEvents.DeathCount++;

                GlobalEvents.PlayerEvents.OnPlayerDie();
                GlobalEvents.GameEvents.StartHellLevel(EventsHolder.LevelEvents.Difficulty,
                    EventsHolder.LevelEvents.DeathCount);

               // EventsHolder.LevelEvents.CurrentLevel = MapType.Level_Hell;
                EventsHolder.LevelEvents.LevelEvent = LevelInfo.LevelEventType.OnStart;
            }
            else
            {
                GlobalEvents.GameEvents.GameLost();
            }
        }
    }
}
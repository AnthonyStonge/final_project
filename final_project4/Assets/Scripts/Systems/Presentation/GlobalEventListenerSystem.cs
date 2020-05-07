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
    private WeaponType WeaponTypeToGoBackTo;

    protected override void OnUpdate()
    {
        var playerLifeComponent = EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity);
        var levelEvents = EventsHolder.LevelEvents;
        if (levelEvents.CurrentLevel == MapType.Level_Hell)
        {
            if (currentHellTimer <= 0)
            {
                OnExitHellLevel();
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
                OnLoadHellLevel(ref playerLifeComponent, ref levelEvents);
            }
            else
            {
                currentHellTimer = hellTimer;    //??
                GlobalEvents.GameEvents.GameLost();
            }
        }
    }

    private void OnLoadHellLevel(ref LifeComponent playerLife, ref LevelInfo levelInfo)
    {
        //Reset Timer
        currentHellTimer = hellTimer;
        
        //Increase Death
        EventsHolder.LevelEvents.DeathCount++;
        
        //Toggle PlayerWeapons
        WeaponTypeToGoBackTo = GameVariables.Player.CurrentWeaponHeld;
        SwapWeaponSystem.SwapWeaponBetweenWorld(WeaponType.HellShotgun, EventsHolder.LevelEvents.CurrentLevel, MapType.Level_Hell);

        //Reset Player Life
        playerLife.Reset();
        EntityManager.SetComponentData(GameVariables.Player.Entity, playerLife);
        
        //Set new MapType
        LastMap = EventsHolder.LevelEvents.CurrentLevel;
        levelInfo.CurrentLevel = MapType.Level_Hell;
        EventsHolder.LevelEvents.CurrentLevel = MapType.Level_Hell;
        MapEvents.LoadMap(MapType.Level_Hell, true);

        //Set UI info
        UIManager.ResetPlayerHealth();
        UIManager.ToggleHellTimers(true);
        UIManager.SetWeaponType(WeaponType.HellShotgun);
        
        //??
        EventsHolder.LevelEvents.LevelEvent = LevelInfo.LevelEventType.OnStart;
    }

    private void OnExitHellLevel()
    {
        //Set UI info
        UIManager.ResetPlayerHealth();
        UIManager.ToggleHellTimers(false);

        //Toggle PlayerWeapons
        SwapWeaponSystem.SwapWeaponBetweenWorld(WeaponTypeToGoBackTo, MapType.Level_Hell, LastMap);
        
        //Set new MapType
        EventsHolder.LevelEvents.CurrentLevel = LastMap;
        MapEvents.LoadMap(LastMap, true);
        
        //Set more UI info
        UIManager.SetWeaponType(WeaponTypeToGoBackTo);
    }
}
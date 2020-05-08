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
    private static MapType LastMap;
    private static WeaponType WeaponTypeToGoBackTo;

    protected override void OnUpdate()
    {
        //Get Player Health
        LifeComponent playerLife = EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity);
        
        //Look if Player Died
        if (playerLife.IsDead())
        {
            //Get Current MapType
            MapType currentMapType = EventsHolder.LevelEvents.CurrentLevel;
            
            SoundEventSystem.PlayerDieSound();

            if (currentMapType == MapType.Level_Hell)
            {
                #if UNITY_EDITOR
                Debug.Log("Player died in Hell World... Returning to main menu.");
                #endif
                    
                //Stop HelloWorldSystem
                World.GetExistingSystem<HellWorldSystem>().Enabled = false;
                
                //Restart Game from beginning
                GlobalEvents.GameEvents.RestartGame();
                
                return;
            }
            
            //Save current Level info
            EventsHolder.LevelEvents.DeathCount++;
            
            //Load Hell Level
            OnLoadHellLevel(ref playerLife);
        }
        
        if (playerLife.Invincibility == InvincibilityType.Hit)
        {
            GlobalEvents.CameraEvents.ShakeCam(.2f, 1, 1.5f);
        }
    }

    private void OnLoadHellLevel(ref LifeComponent playerLife)
    {
        //Start HellWorldSystem
        World.GetExistingSystem<HellWorldSystem>().Enabled = true;
        
        //Toggle PlayerWeapons
        WeaponTypeToGoBackTo = GameVariables.Player.CurrentWeaponHeld;
        SwapWeaponSystem.SwapWeaponBetweenWorld(WeaponType.HellShotgun, EventsHolder.LevelEvents.CurrentLevel, MapType.Level_Hell);

        //Reset Player Life
        playerLife.Reset();
        EntityManager.SetComponentData(GameVariables.Player.Entity, playerLife);
        
        //Set new MapType
        LastMap = EventsHolder.LevelEvents.CurrentLevel;
        EventsHolder.LevelEvents.CurrentLevel = MapType.Level_Hell;
        MapEvents.LoadMap(MapType.Level_Hell, true);

        //Set UI info
        UIManager.ResetPlayerHealth();
        UIManager.ToggleHellTimers(true);
        UIManager.SetWeaponType(WeaponType.HellShotgun);
        
        //??
        EventsHolder.LevelEvents.LevelEvent = LevelInfo.LevelEventType.OnStart;
    }

    public static void OnExitHellLevel()
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
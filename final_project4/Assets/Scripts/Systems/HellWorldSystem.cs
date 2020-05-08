using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HellWorldSystem : SystemBase
{
    public static float HellTimer;
    private static float ResetHellTimer = 5;

    protected override void OnCreate()
    {
        Enabled = false;
    }

    protected override void OnStartRunning()
    {
        Debug.Log("Starting Hell World...");
        //Reset Timer
        HellTimer = ResetHellTimer;
    }

    protected override void OnUpdate()
    {
        //Decrease Timer
        HellTimer -= Time.DeltaTime;
        
        //Set UI timers
        UIManager.SetTimeOnHellTimers(HellTimer);
        
        //Look if end timer reached
        if (HellTimer > 0)
            return;

        Debug.Log("Player survived Hell World... Returning to previous map");
        OnHellWorldEnd();
        
        Enabled = false;
    }

    private static void OnHellWorldEnd()
    {
        //Set UI timers
        UIManager.SetTimeOnHellTimers(0);
        //TODO DISPLAY END HELL LEVEL UI
        
        //Stop Spawner
        //TODO

        //Kill all enemies
        //TODO
        
        //Instantiate Teleport + vfx?
        //TODO

        //End level / Go back to previous world
        GlobalEventListenerSystem.OnExitHellLevel();
    }
}

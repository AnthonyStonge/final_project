using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class HellWorldSystem : SystemBase
{
    public static float HellTimer;
    private static float ResetHellTimer = 30;

    private static float BeforeLevelStartTimer;
    private static float ResetBeforeLevelStart = 5;

    private static ushort PreviousDisplayedNumber;

    private static HellWorldStateType Type;
    enum HellWorldStateType
    {
        OnPreLevel,
        OnLevel,
        OnEndLevel
    }

    protected override void OnCreate()
    {
        Enabled = false;
    }

    protected override void OnStartRunning()
    {
        //Reset Timer
        HellTimer = ResetHellTimer;
        BeforeLevelStartTimer = ResetBeforeLevelStart;
        PreviousDisplayedNumber = (ushort) (BeforeLevelStartTimer + 1);
        
        Type = HellWorldStateType.OnPreLevel;
        MonoGameVariables.Instance.Hell_ExplainText.gameObject.SetActive(true);
        MonoGameVariables.Instance.Hell_FirstTimer.gameObject.SetActive(true);

        World.GetExistingSystem<TemporaryEnemySpawnerSystem>().Enabled = false;
    }

    protected override void OnUpdate()
    {
        if(Type == HellWorldStateType.OnPreLevel)
        {
            OnPreLevel();
            return;
        }

        //Decrease Timer
        HellTimer -= Time.DeltaTime;

        //Set UI timers
        UIManager.SetTimeOnHellTimers(HellTimer);

        //Look if end timer reached
        if (HellTimer > 0)
            return;
#if UNITY_EDITOR
        Debug.Log("Player survived Hell World... Returning to previous map");
#endif
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

    //Used to display First timer
    private void OnPreLevel()
    {
        BeforeLevelStartTimer -= Time.DeltaTime;

        if (BeforeLevelStartTimer <= 0)
        {
            Type = HellWorldStateType.OnLevel;
            //Toggle off first timer
            MonoGameVariables.Instance.Hell_ExplainText.gameObject.SetActive(false);
            MonoGameVariables.Instance.Hell_FirstTimer.gameObject.SetActive(false);
            
            World.GetExistingSystem<TemporaryEnemySpawnerSystem>().Enabled = true;
            return;
        }

        //Look if should create number
        if (BeforeLevelStartTimer <= PreviousDisplayedNumber - 1)
        {
            PreviousDisplayedNumber--;

            //Reset text scale
            MonoGameVariables.Instance.Hell_FirstTimer.transform.localScale = new Vector3(1, 1, 1);
            MonoGameVariables.Instance.Hell_FirstTimer.text = PreviousDisplayedNumber.ToString();
            MonoGameVariables.Instance.Hell_FirstTimer.StartCoroutine(DecreaseTextSize(MonoGameVariables.Instance.Hell_FirstTimer));
        }
    }

    private IEnumerator DecreaseTextSize(TextMeshPro text)
    {
        float timer = 1;
        float speed = 2;

        while (timer > 0)
        {
            timer -= Time.DeltaTime;

            float size = 1 * timer * speed;
            text.transform.localScale = new Vector3(size, size, 1);

            yield return null;
        }
    }
}
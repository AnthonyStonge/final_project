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

    private static float AfterLevelTimer;
    private static float ResetAfterLevel = 3;

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
        MonoGameVariables.Instance.Hell_WaitingTimer.gameObject.SetActive(true);

        World.GetExistingSystem<TemporaryEnemySpawnerSystem>().Enabled = false;
    }

    protected override void OnUpdate()
    {
        if (!GlobalEvents.GameEvents.TogglePauseGame)
            return;
        
        if(Type == HellWorldStateType.OnPreLevel)
        {
            OnPreLevel();
            return;
        }

        if (Type == HellWorldStateType.OnLevel)
        {
            //Decrease Timer
            HellTimer -= Time.DeltaTime;
            
            //Set UI timers
            UIManager.SetTimeOnHellTimers(HellTimer);
            
            //Look if end timer reached
            if (HellTimer > 0)
                return;

            Type = HellWorldStateType.OnEndLevel;

            //Deactivate spawners
            World.GetExistingSystem<TemporaryEnemySpawnerSystem>().Enabled = false;
            
            //Destroy all enemies
            GlobalEvents.GameEvents.Destroy<EnemyTag>();
            
            //Make sure Timers are set o 0.00
            UIManager.SetTimeOnHellTimers(0);
            
            //Toggle Win Text
            MonoGameVariables.Instance.Hell_WinText.gameObject.SetActive(true);
            MonoGameVariables.Instance.Hell_WaitingTimer.gameObject.SetActive(true);
            
            //Init Delay
            AfterLevelTimer = ResetAfterLevel;
            PreviousDisplayedNumber = (ushort)(AfterLevelTimer + 1);
        }

        if (Type == HellWorldStateType.OnEndLevel)
        {
            OnEndLevel();
        }
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
            MonoGameVariables.Instance.Hell_WaitingTimer.gameObject.SetActive(false);
            
            World.GetExistingSystem<TemporaryEnemySpawnerSystem>().Enabled = true;
            return;
        }

        //Look if should create number
        if (BeforeLevelStartTimer <= PreviousDisplayedNumber - 1)
        {
            PreviousDisplayedNumber--;

            //Reset text scale
            MonoGameVariables.Instance.Hell_WaitingTimer.transform.localScale = new Vector3(1, 1, 1);
            MonoGameVariables.Instance.Hell_WaitingTimer.text = PreviousDisplayedNumber.ToString();
            MonoGameVariables.Instance.Hell_WaitingTimer.StartCoroutine(DecreaseTextSize(MonoGameVariables.Instance.Hell_WaitingTimer));
        }
    }

    private void OnEndLevel()
    {
        AfterLevelTimer -= Time.DeltaTime;

        if (AfterLevelTimer <= 0)
        {
            //Toggle off UI
            MonoGameVariables.Instance.Hell_WinText.gameObject.SetActive(false);
            MonoGameVariables.Instance.Hell_WaitingTimer.gameObject.SetActive(false);
            
            //Return to previous map
            GlobalEventListenerSystem.OnExitHellLevel();
            
            //Disable self
            Enabled = false;
            
            return;
        }
        
        //Look if should create number
        if (AfterLevelTimer <= PreviousDisplayedNumber - 1)
        {
            PreviousDisplayedNumber--;

            //Reset text scale
            MonoGameVariables.Instance.Hell_WaitingTimer.transform.localScale = new Vector3(1, 1, 1);
            MonoGameVariables.Instance.Hell_WaitingTimer.text = PreviousDisplayedNumber.ToString();
            MonoGameVariables.Instance.Hell_WaitingTimer.StartCoroutine(DecreaseTextSize(MonoGameVariables.Instance.Hell_WaitingTimer));
        }
    }

    private IEnumerator DecreaseTextSize(TextMeshPro text)
    {
        float timer = 1;
        float speed = 2;

        while (timer > 0)
        {
            if (!GlobalEvents.GameEvents.TogglePauseGame)
                yield return null;
            
            timer -= Time.DeltaTime;

            float size = 1 * timer * speed;
            text.transform.localScale = new Vector3(size, size, 1);

            yield return null;
        }
    }
}
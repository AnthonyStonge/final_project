using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class WinningSystem : SystemBase
{
    private float Timer;
    private float ResetTimer = 7;

    private float TextTimer;
    private float ResetTextTimer;
    private float Speed;

    protected override void OnCreate()
    {
        Enabled = false;
    }

    protected override void OnStartRunning()
    {
        Timer = ResetTimer;

        //Toggle UI on
        MonoGameVariables.Instance.WinningText.gameObject.SetActive(true);
        
        GlobalEvents.GameEvents.Destroy<EnemyTag>();
        GlobalEvents.GameEvents.Destroy<BulletTag>();
    }

    protected override void OnStopRunning()
    {
        //Toggle UI off
        MonoGameVariables.Instance.WinningText.gameObject.SetActive(false);
        
        GlobalEvents.GameEvents.RestartGame();
    }

    protected override void OnUpdate()
    {
        Timer -= Time.DeltaTime;

        if (Timer > 0)
        {
            /*//Decrease Text size
            float size = TextTimer * Speed;
            MonoGameVariables.Instance.WinningText.transform.localScale = new Vector3(size, size, 1);

            TextTimer -= Time.DeltaTime * (1 / ResetTimer);*/
        }
        else
        {
            this.Enabled = false;
        }
    }
}
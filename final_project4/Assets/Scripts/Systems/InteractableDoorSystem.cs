using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class InteractableDoorSystem : SystemBase
{
    protected override void OnCreate()
    {
        Enabled = false;
    }

    protected override void OnStartRunning()
    {
        //Toggle UI on
#if UNITY_EDITOR
        Debug.Log("Walk in Interactable Door...");
#endif
    }

    protected override void OnStopRunning()
    {
        //Toggle UI off
#if UNITY_EDITOR
        Debug.Log("Walk out Interactable Door...");
#endif

        GameVariables.Interactables.PreviousInteractableSelected = null;
    }

    protected override void OnUpdate()
    {
        //Listen for input
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        OnAction();
    }

    private static void OnAction()
    {
        //Open Door
#if UNITY_EDITOR
        Debug.Log("Opened Door...");
#endif
    }
}
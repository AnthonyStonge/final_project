using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public class CameraFollowSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Debug.Log("On CameraFollowSystem Update");

        //Update MainCamera to player position
        if (MonoGameVariables.instance.MainCamera == null) return;

        MonoGameVariables.instance.MainCamera.transform.position = 
            GameVariables.PlayerVars.CurrentPosition + GameVariables.CameraVars.PlayerOffset;
    }
}
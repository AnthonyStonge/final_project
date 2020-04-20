using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
//TODO No need to do a foreach withoutburst when we have access to the player entity and thats all we want to update
[DisableAutoCreation]
public class UpdatePlayerInfoSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithoutBurst().WithAll<PlayerTag>().ForEach((in Translation trans/*, in HealthData healthData, in SpeedData speedData, in StateData stateData*/) =>
        {
            //TODO Temporary fix to enable camera tracking, need to add missing component on player
            // GameVariables.PlayerVars.CurrentSpeed = speedData.Value;
            // GameVariables.PlayerVars.CurrentHealth = healthData.Value;
            // GameVariables.PlayerVars.CurrentState = stateData.Value;
            GameVariables.PlayerVars.CurrentPosition = trans.Value;
            // GameVariables.PlayerVars.Transform.position = trans.Value;
        }).Run();
    }
}

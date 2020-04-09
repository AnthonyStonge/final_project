using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
//TODO No need to do a foreach withoutburst when we have access to the player entity and thats all we want to update
[DisableAutoCreation]
public class UpdatePlayerInfoSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithoutBurst().WithAll<PlayerTag>().ForEach((in Translation trans, in HealthData healthData, in SpeedData speedData, in StateData stateData) =>
        {
            GameVariables.PlayerVars.Speed = speedData.Value;
            GameVariables.PlayerVars.Health = healthData.Value;
            GameVariables.PlayerVars.CurrentState = stateData.Value;
            GameVariables.PlayerVars.CurrentPosition = trans.Value;
            GameVariables.PlayerVars.Transform.position = trans.Value;
        }).Run();
    }
}

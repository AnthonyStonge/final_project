using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class UpdatePlayerStateSystem : SystemBase
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        var stateData = GetComponent<StateData>(GameVariables.Player.Entity);
        
        
        if (stateData.Value == StateActions.DYING)
        {
            GameStateComponent gameStateComponent = EntityManager.GetComponentData<GameStateComponent>(GameLogicSystem.GameLogicEntity);
            
            //TODO Remove this code
            gameStateComponent.CurrentGameState = GameState.MENU;
            gameStateComponent.DesiredGameState = GameState.GAME;
            
            EntityManager.SetComponentData(GameLogicSystem.GameLogicEntity, gameStateComponent);
            
        }
    }
}

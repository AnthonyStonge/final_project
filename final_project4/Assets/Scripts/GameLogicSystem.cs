using Unity.Entities;

public class GameLogicSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithoutBurst().ForEach((ref GameStateComponent gameStateComponent) =>
        {
            switch (gameStateComponent.DesiredGameState)
            {
                case GameState.GAME:
                    if (gameStateComponent.CurrentGameState == GameState.GAME)
                    {
                        //TODO normal behaviour
                    }
                    else
                    {
                        //Initialize All Game loop entities & stuff 
                    }
                    break;
                case GameState.MENU:
                    if (gameStateComponent.CurrentGameState == GameState.MENU)
                    {
                        //TODO normal behaviour
                    }
                    else
                    {
                        //Initialize All Menu loop entites & stuff
                    }
                    break;
                case GameState.INTRO:
                    if (gameStateComponent.CurrentGameState == GameState.INTRO)
                    {
                        //TODO normal behaviour
                    }
                    else
                    {
                        //Initialize All Game loop entities & stuff 
                    }
                    break;
                case GameState.SCENECHANGE: 
                    if (gameStateComponent.CurrentGameState == GameState.SCENECHANGE)
                    {
                        //TODO normal behaviour
                    }
                    else
                    {
                        //Initialize All Game loop entities & stuff 
                    }
                    break;
            }
        }).Run();
    }
}

using Unity.Entities;

public struct GameStateComponent : IComponentData
{
    public GameState CurrentGameState { get; private set; }
    public GameState DesiredGameState;

    public void ConfirmChangeOfState()
    {
        CurrentGameState = DesiredGameState;
    }
}

public enum GameState
{
    INTRO,
    MENU,
    SCENECHANGE,
    GAME
}

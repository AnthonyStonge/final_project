using Unity.Entities;

public struct GameStateComponent : IComponentData
{
    public GameState CurrentGameState { get; private set; }
    public GameState DesiredGameState;

    public bool IsInTransition;

    public bool IsChangeOfStateRequested()
    {
        return DesiredGameState != CurrentGameState;
    }
    
    public void ConfirmChangeOfState()
    {
        CurrentGameState = DesiredGameState;
        IsInTransition = false;
    }

    public GameStateComponent(GameState gs)
    {
        CurrentGameState = gs;
        DesiredGameState = gs;
        IsInTransition = false;
    }
}

public enum GameState
{
    INTRO,
    MENU,
    GAME
}

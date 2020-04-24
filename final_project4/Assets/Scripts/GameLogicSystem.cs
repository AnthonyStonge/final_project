using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class GameLogicSystem : SystemBase
{
    private static Dictionary<GameState, IStateLogic> LogicClassDict;

    private Entity gameLogicEntity;
    private EntityManager entityManager;

    private bool IsFadingOut = false;
    private bool IsFadingIn = false;

    protected override void OnCreate()
    {
        LogicClassDict = new Dictionary<GameState, IStateLogic>();

        LogicClassDict.Add(GameState.GAME, new GameLogic());
        LogicClassDict.Add(GameState.INTRO, new IntroLogic());
        LogicClassDict.Add(GameState.MENU, new MenuLogic());

        entityManager = GameVariables.EntityManager;

        gameLogicEntity = entityManager.CreateEntity();

        entityManager.AddComponentData(gameLogicEntity, new GameStateComponent());
    }

    protected override void OnStartRunning()
    {
        foreach (var i in LogicClassDict)
        {
            if (i.Key != GameVariables.StartingState)
            {
                i.Value.Disable();
            }
        }

        LogicClassDict[GameVariables.StartingState].Initialize();
        LogicClassDict[GameVariables.StartingState].Enable();
    }

    protected override void OnUpdate()
    {
        

        //var gameEntity = EntityManager.GetComponentData<GameStateComponent>();
        Entities.WithoutBurst().ForEach((ref GameStateComponent gameStateComponent) =>
        {
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                gameStateComponent.DesiredGameState = GameState.INTRO;
            }

            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                gameStateComponent.DesiredGameState = GameState.MENU;
            }

            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                gameStateComponent.DesiredGameState = GameState.GAME;
            }
            
            //Normal Logic
            if (!gameStateComponent.IsChangeOfStateRequested())
                return;

            //Transition logic.
            if (gameStateComponent.IsInTransition)
            {
                if (IsFadingOut)
                {
                    //Fading out
                    if (FadeOut())
                    {
                        //Fade out over, Ready to init & disable last state
                        //Screen SHOULD hide the world to the player
                        DestroyLastState(gameStateComponent.CurrentGameState);
                        InitializeNextState(gameStateComponent.DesiredGameState);
                        IsFadingOut = false;
                        IsFadingIn = true;
                    }
                }
                else if (IsFadingIn)
                {
                    //Fading In
                    if (FadeIn())
                    {
                        //Fade In Over, Ready to change state
                        EnableNextState(gameStateComponent.DesiredGameState);
                        gameStateComponent.ConfirmChangeOfState();
                        IsFadingIn = false;
                    }
                }

                return;
            }

            //Initialization trigger once per Statechange
            if (gameStateComponent.IsChangeOfStateRequested())
            {
                gameStateComponent.IsInTransition = true;
                IsFadingOut = true;
                DisableLastState(gameStateComponent.CurrentGameState);
            }
        }).Run();
    }

    private static bool FadeIn()
    {
        //TODO logic
        Debug.Log("FadeIn");
        return true;
    }

    private static bool FadeOut()
    {
        //TODO logic
        Debug.Log("FadeOut");
        return true;
    }

    private static void EnableNextState(GameState state)
    {
        LogicClassDict[state].Enable();
    }

    private static void DisableLastState(GameState state)
    {
        LogicClassDict[state].Disable();
    }

    private static void DestroyLastState(GameState state)
    {
        LogicClassDict[state].Destroy();
    }

    private static void InitializeNextState(GameState state)
    {
        LogicClassDict[state].Initialize();
    }
}
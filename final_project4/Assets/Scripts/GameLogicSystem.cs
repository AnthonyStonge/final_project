using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class GameLogicSystem : SystemBase
{
    private static Dictionary<GameState, IStateLogic> LogicClassDict;

    public static Entity GameLogicEntity;
    private EntityManager entityManager;

    private bool IsFadingOut = false;
    private bool IsFadingIn = false;

    private static bool FadingOver;
    protected override void OnCreate()
    {
        LogicClassDict = new Dictionary<GameState, IStateLogic>();

        LogicClassDict.Add(GameState.GAME, new GameLogic());
        LogicClassDict.Add(GameState.INTRO, new IntroLogic());
        LogicClassDict.Add(GameState.MENU, new MenuLogic());

        entityManager = GameVariables.EntityManager;

        GameLogicEntity = entityManager.CreateEntity();

        entityManager.AddComponentData(GameLogicEntity, new GameStateComponent());

        FadeSystem.OnFadeEnd += () => FadingOver = true;

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
        Entities.WithStructuralChanges().WithoutBurst().ForEach((ref GameStateComponent gameStateComponent) =>
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
                    if (FadingOver)
                    {
                        FadingOver = false;
                        //Fade out over, Ready to init & disable last state
                        //Screen SHOULD hide the world to the player
                        DestroyLastState(gameStateComponent.CurrentGameState);
                        InitializeNextState(gameStateComponent.DesiredGameState);
                        IsFadingOut = false;
                        IsFadingIn = true;
                        GlobalEvents.FadeIn();

                    }
                }
                else if (IsFadingIn)
                {
                    //Fading In
                    if (FadingOver)
                    {
                        FadingOver = false;
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
                GlobalEvents.FadeOut();
                gameStateComponent.IsInTransition = true;
                IsFadingOut = true;
                DisableLastState(gameStateComponent.CurrentGameState);
            }
        }).Run();
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
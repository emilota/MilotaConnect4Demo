// Created and programmed by Eric Milota, 2021

using UnityEngine;

namespace MilotaConnect4Demo
{
    public class StartNewGameState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.START_NEW_GAME;

        public override void OnStateEnter(App app) 
        {
            app.SceneManager.GotoSceneASync(SceneEnum.GAME_SCENE, () =>
            {
                // init board
                app.Board.Init(
                    app,
                    app.GameSceneMB.BoardNumColumns,
                    app.GameSceneMB.BoardNumRows,
                    app.GameSceneMB.BoardNumCheckersInARowToWin);
                // sanity check
                if (!app.Board.IsValidBoard)
                {
                    Debug.LogError(
                        "Error, params used to build game board aren't valid! " +
                        "Please recheck GameSceneMonoBehaviour.cs values.");
                }
                app.Board.StartGame();
                app.Board.CreateGameObjects();

                app.GameSceneMB.ShowBigMessage(Localize.START_NEW_GAME_LETS_PLAY);
            });
        }

        public override void OnStateLeave(App app) 
        {
            app.GameSceneMB.HideBigMessage();
        }

        public override void OnStateUpdate(App app)
        {
            if (app.GameSceneMB != null)
            {
                if (app.StateManager.TimeInCurrentState >= app.GameSceneMB.StartNewGameMessageTimeInMS)
                {
                    app.Board.StartGame();
                    app.StateManager.GotoState(State.PLAYER_SELECT);
                }
            }
        }

        public override void OnStateFixedUpdate(App app) 
        { 
        }
    }
}

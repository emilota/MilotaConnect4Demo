// Created and programmed by Eric Milota, 2021

using System.Collections.Generic;

namespace MilotaConnect4Demo
{
    public class ResetBoardState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.RESET_BOARD;

        public override void OnStateEnter(App app) 
        {
            app.GameSceneMB.HideBigMessage();
            app.GameSceneMB.HideFooterMessage();
            app.Board.ConvertAllStaticCheckersIntoMovingCheckers();
        }

        public override void OnStateLeave(App app) 
        {
        }

        public override void OnStateUpdate(App app) 
        {
            if (app.Board.CheckerManager.NumActiveCheckers <= 0)
            {
                app.StateManager.GotoState(State.START_NEW_GAME);
            }
        }

        public override void OnStateFixedUpdate(App app)
        {
        }
    }
}

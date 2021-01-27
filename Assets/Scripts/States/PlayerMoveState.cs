// Created and programmed by Eric Milota, 2021

using System.Collections.Generic;

namespace MilotaConnect4Demo
{
    public class PlayerMoveState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.PLAYER_MOVE;

        public override void OnStateEnter(App app) 
        {
            // checker move should already be showing when we get here
        }

        public override void OnStateLeave(App app) 
        {
            app.Board.RemoveAllMovingCheckers();
        }

        public override void OnStateUpdate(App app) 
        {
            if (app.Board.CheckerManager.NumActiveCheckers == 0)
            {
                // done moving!  Time to see if somebody won

                string tempString = app.Board.GetDebugBoardInfo();

                WhichPlayer whichPlayerWinner = WhichPlayer.NONE;
                List<BoardCoord> winnerBoardCoordList = null;

                if (app.Board.CheckForWin(ref whichPlayerWinner, ref winnerBoardCoordList))
                {
                    for(int index = 0; index < winnerBoardCoordList.Count; index++)
                    {
                        app.Board.SetBoardEntryInfo(
                            winnerBoardCoordList[ index ].Col,
                            winnerBoardCoordList[ index ].Row,
                            whichPlayerWinner,
                            true, // blink!
                            true);
                    }
                    app.Board.SetWhichPlayerWinner(whichPlayerWinner);
                    app.StateManager.GotoState(State.GAME_OVER);
                }
                else if (app.Board.CheckForFullBoard())
                {
                    app.Board.SetWhichPlayerWinner(WhichPlayer.NONE); // no winner
                    app.StateManager.GotoState(State.GAME_OVER);
                }
                else
                {
                    // go to next player's turn
                    app.Board.AdvanceToNextPlayersTurn();
                    app.StateManager.GotoState(State.PLAYER_SELECT);
                }
            }
        }

        public override void OnStateFixedUpdate(App app)
        {
        }
    }
}

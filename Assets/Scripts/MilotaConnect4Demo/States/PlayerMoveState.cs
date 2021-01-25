// Created and programmed by Eric Milota, 2021

using System.Collections.Generic;

namespace MilotaConnect4Demo
{
    public class PlayerMoveState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.PLAYER_MOVE;

        public override void OnStateEnter(Controller controller) 
        {
            // checker move should already be showing when we get here
        }

        public override void OnStateLeave(Controller controller) 
        {
            controller.Board.RemoveAllMovingCheckers();
        }

        public override void OnStateClickRestartOrQuitButton(Controller controller)
        {
            controller.StateManager.GotoState(State.TITLE_SCREEN);
        }

        public override void OnStateClickFullscreenButton(Controller controller)
        {
        }

        public override void OnStateUpdate(Controller controller) 
        {
            if (controller.Board.CheckerManager.NumActiveCheckers == 0)
            {
                // done moving!  Time to see if somebody won

                string tempString = controller.Board.GetDebugBoardInfo();

                WhichPlayer whichPlayerWinner = WhichPlayer.NONE;
                List<BoardCoord> winnerBoardCoordList = null;

                if (controller.Board.CheckForWin(ref whichPlayerWinner, ref winnerBoardCoordList))
                {
                    for(int index = 0; index < winnerBoardCoordList.Count; index++)
                    {
                        controller.Board.SetBoardEntryInfo(
                            winnerBoardCoordList[ index ].Col,
                            winnerBoardCoordList[ index ].Row,
                            whichPlayerWinner,
                            true, // blink!
                            true);
                    }
                    controller.Board.SetWhichPlayerWinner(whichPlayerWinner);
                    controller.StateManager.GotoState(State.GAME_OVER);
                }
                else if (controller.Board.CheckForFullBoard())
                {
                    controller.Board.SetWhichPlayerWinner(WhichPlayer.NONE); // no winner
                    controller.StateManager.GotoState(State.GAME_OVER);
                }
                else
                {
                    // go to next player's turn
                    controller.Board.AdvanceToNextPlayersTurn();
                    controller.StateManager.GotoState(State.PLAYER_SELECT);
                }
            }
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
        }
    }
}

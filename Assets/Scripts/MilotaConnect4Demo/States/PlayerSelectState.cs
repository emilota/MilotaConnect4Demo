// Created and programmed by Eric Milota, 2021

using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class PlayerSelectState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.PLAYER_SELECT;

        public override void OnStateEnter(Controller controller) 
        {
            switch (controller.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        controller.Board.HideCheckerSelect(); // hidden for now....will show if we hit box collider with cursor
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public override void OnStateLeave(Controller controller) 
        {
            switch (controller.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        // it's me!  Hide the checker select
                        controller.Board.HideCheckerSelect();
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public override void OnStateClickRestartOrQuitButton(Controller controller)
        {
            controller.StateManager.GotoState(State.TITLE_SCREEN);
        }

        public override void OnStateClickFullscreenButton(Controller controller)
        {
            switch (controller.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        // it's my turn

                        // show/hide 3D cursor
                        RaycastHit raycastHit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out raycastHit))
                        {
                            int col = Const.INVALID_COL_VALUE;
                            int row = Const.INVALID_ROW_VALUE;
                            if (controller.Board.FindColumnFromPlaneCoordX(
                                raycastHit.point.x,
                                ref col,
                                ref row))
                            {
                                controller.Board.ShowCheckerSelect(
                                    col,
                                    row);
                            }
                            else
                            {
                                controller.Board.HideCheckerSelect();
                            }
                        }
                        else
                        {
                            controller.Board.HideCheckerSelect();
                        }

                        if (controller.Board.IsCheckerSelectShowing)
                        {
                            // drop checker....maybe
                            if (controller.Board.TryDropChecker(
                                    controller.Board.WhichPlayerCurrent,
                                    controller.Board.CheckerSelectCol,
                                    controller.Board.CheckerSelectRow))
                            {
                                // player 1 human dropped a checker
                                controller.StateManager.GotoState(State.PLAYER_MOVE);
                            }
                        }
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        // it's AI's turn
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public override void OnStateUpdate(Controller controller) 
        {
            switch (controller.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        // it's my turn... update checker position

                        // show/hide 3D cursor
                        RaycastHit raycastHit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out raycastHit))
                        {
                            int col = Const.INVALID_COL_VALUE;
                            int row = Const.INVALID_ROW_VALUE;
                            if (controller.Board.FindColumnFromPlaneCoordX(
                                raycastHit.point.x,
                                ref col,
                                ref row))
                            {
                                controller.Board.ShowCheckerSelect(
                                    col,
                                    row);
                            }
                            else
                            {
                                controller.Board.HideCheckerSelect();
                            }
                        }
                        else
                        {
                            controller.Board.HideCheckerSelect();
                        }
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        // it's AI's turn
                        if (controller.Board.AI.OnTryToMakeAMove(controller))
                        {
                            // we've moved!
                            controller.StateManager.GotoState(State.PLAYER_MOVE);
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
            switch (controller.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        // it's my turn
                        controller.Board.UpdateCheckerSelectShowHide();
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        // it's AI's turn
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}

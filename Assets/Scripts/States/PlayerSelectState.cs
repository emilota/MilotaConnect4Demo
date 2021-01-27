// Created and programmed by Eric Milota, 2021

using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class PlayerSelectState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.PLAYER_SELECT;

        public override void OnStateEnter(App app) 
        {
            switch (app.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        app.Board.HideCheckerSelect(); // hidden for now....will show if we hit box collider with cursor
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

        public override void OnStateLeave(App app) 
        {
            switch (app.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        // it's me!  Hide the checker select
                        app.Board.HideCheckerSelect();
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

        public override void OnStateUpdate(App app) 
        {
            switch (app.Board.WhichPlayerCurrent)
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
                            if (app.Board.FindColumnFromPlaneCoordX(
                                raycastHit.point.x,
                                ref col,
                                ref row))
                            {
                                app.Board.ShowCheckerSelect(
                                    col,
                                    row);
                            }
                            else
                            {
                                app.Board.HideCheckerSelect();
                            }
                        }
                        else
                        {
                            app.Board.HideCheckerSelect();
                        }
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        // it's AI's turn
                        if (app.Board.AI.OnTryToMakeAMove(app))
                        {
                            // we've moved!
                            app.StateManager.GotoState(State.PLAYER_MOVE);
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public override void OnStateFixedUpdate(App app)
        {
            switch (app.Board.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        // it's my turn
                        app.Board.UpdateCheckerSelectShowHide();
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

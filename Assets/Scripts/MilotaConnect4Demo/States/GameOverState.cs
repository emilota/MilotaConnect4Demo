using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class GameOverState : BaseState
    {
        public enum DisplayMode
        {
            NONE,
            GAME_OVER,
            YOU_WIN_OR_YOU_LOSE
        }

        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.GAME_OVER;

        private DisplayMode mDisplayMode = DisplayMode.NONE;

        private void UpdateBigMessage(Controller controller)
        {
            switch (mDisplayMode)
            {
                case DisplayMode.NONE:
                    {
                        controller.UI.HideBigMessage();
                        break;
                    }
                case DisplayMode.GAME_OVER:
                    {
                        controller.UI.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_GAME_OVER);
                        break;
                    }
                case DisplayMode.YOU_WIN_OR_YOU_LOSE:
                    {
                        switch (controller.Board.WhichPlayerWinner)
                        {
                            case WhichPlayer.NONE:
                                {
                                    controller.UI.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_ITS_A_DRAW);
                                    break;
                                }
                            case WhichPlayer.PLAYER_1_HUMAN:
                                {
                                    controller.UI.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_YOU_WIN);
                                    break;
                                }
                            case WhichPlayer.PLAYER_2_AI:
                                {
                                    controller.UI.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_YOU_LOSE);
                                    break;
                                }
                            default:
                                {
                                    controller.UI.HideBigMessage();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        controller.UI.HideBigMessage();
                        break;
                    }
            }
        }

        public override void OnStateEnter(Controller controller) 
        {
            mDisplayMode = DisplayMode.GAME_OVER;
            UpdateBigMessage(controller);
            controller.UI.ShowFooterMessage(Localize.GAME_OVER_FOOTER_MESSAGE);
        }

        public override void OnStateLeave(Controller controller) 
        {
            controller.UI.HideFooterMessage();
        }

        public void AdvanceDisplayMode()
        {
            switch (mDisplayMode)
            {
                case DisplayMode.NONE:
                    {
                        break;
                    }
                case DisplayMode.GAME_OVER:
                    {
                        mDisplayMode = DisplayMode.YOU_WIN_OR_YOU_LOSE;
                        break;
                    }
                case DisplayMode.YOU_WIN_OR_YOU_LOSE:
                    {
                        mDisplayMode = DisplayMode.GAME_OVER;
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
            if (controller.StateManager.TimeInCurrentState >= controller.UI.GameOverMessageBlinkInMS)
            {
                AdvanceDisplayMode();
                controller.StateManager.ResetStateTime();
                UpdateBigMessage(controller);
            }

            if ((Input.GetMouseButtonDown(0)) && 
                (!EventSystem.current.IsPointerOverGameObject())) // ignore if over button
            {
                controller.StateManager.GotoState(State.RESET_BOARD);
            }
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
            controller.Board.UpdateCheckerShowHide();
        }
    }
}

// Created and programmed by Eric Milota, 2021

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
        private bool mFooterMessageToggle = true;

        private void UpdateBigMessage(App app)
        {
            switch (mDisplayMode)
            {
                case DisplayMode.NONE:
                    {
                        app.GameSceneMB.HideBigMessage();
                        break;
                    }
                case DisplayMode.GAME_OVER:
                    {
                        app.GameSceneMB.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_GAME_OVER);
                        break;
                    }
                case DisplayMode.YOU_WIN_OR_YOU_LOSE:
                    {
                        switch (app.Board.WhichPlayerWinner)
                        {
                            case WhichPlayer.NONE:
                                {
                                    app.GameSceneMB.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_ITS_A_DRAW);
                                    break;
                                }
                            case WhichPlayer.PLAYER_1_HUMAN:
                                {
                                    app.GameSceneMB.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_YOU_WIN);
                                    break;
                                }
                            case WhichPlayer.PLAYER_2_AI:
                                {
                                    app.GameSceneMB.ShowBigMessage(Localize.GAME_OVER_BIG_MESSAGE_YOU_LOSE);
                                    break;
                                }
                            default:
                                {
                                    app.GameSceneMB.HideBigMessage();
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        app.GameSceneMB.HideBigMessage();
                        break;
                    }
            }
        }

        private void UpdateFooterMessage(App app)
        {
            if (app.StateManager.TimeInCurrentState > app.GameSceneMB.FooterBlinkRateInMS)
            {
                mFooterMessageToggle = !mFooterMessageToggle;
                app.StateManager.ResetStateTime();
            }
            if (mFooterMessageToggle)
            {
                app.GameSceneMB.ShowFooterMessage(Localize.GAME_OVER_FOOTER_MESSAGE);
            }
            else
            {
                app.GameSceneMB.HideFooterMessage();
            }
        }

        public override void OnStateEnter(App app) 
        {
            mDisplayMode = DisplayMode.GAME_OVER;
            mFooterMessageToggle = true;
            UpdateBigMessage(app);
            UpdateFooterMessage(app);
        }

        public override void OnStateLeave(App app) 
        {
            app.GameSceneMB.HideFooterMessage();
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

        public override void OnStateUpdate(App app) 
        {
            if (app.StateManager.TimeInCurrentState >= app.GameSceneMB.GameOverMessageBlinkInMS)
            {
                AdvanceDisplayMode();
                app.StateManager.ResetStateTime();
                UpdateBigMessage(app);
            }
        }

        public override void OnStateFixedUpdate(App app)
        {
            UpdateFooterMessage(app);
            app.Board.UpdateCheckerShowHide();
        }
    }
}

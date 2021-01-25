// Created and programmed by Eric Milota, 2021

using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class TitleScreenState : BaseState
    {
        public override State State => State.TITLE_SCREEN;

        private bool mFooterMessageToggle = true;
        private void UpdateFooterMessage(Controller controller)
        {
            if (controller.StateManager.TimeInCurrentState > controller.UI.FooterBlinkRateInMS)
            {
                mFooterMessageToggle = !mFooterMessageToggle;
                controller.StateManager.ResetStateTime();
            }
            if (mFooterMessageToggle)
            {
                controller.UI.ShowFooterMessage(Localize.TITLE_SCREEN_FOOTER_MESSAGE);
            }
            else
            {
                controller.UI.HideFooterMessage();
            }
        }

        public override void OnStateEnter(Controller controller)
        {
            controller.SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.QUIT);
            controller.UI.HideBoard();
            controller.UI.HideBigMessage();

            controller.UI.ShowTitle();

            mFooterMessageToggle = true;
            UpdateFooterMessage(controller);
        }

        public override void OnStateLeave(Controller controller)
        {
            controller.UI.HideTitle();
            controller.UI.HideFooterMessage();
        }

        public override void OnStateClickRestartOrQuitButton(Controller controller)
        {
            controller.StateManager.GotoState(State.THANKS_FOR_PLAYING);
        }

        public override void OnStateClickFullscreenButton(Controller controller) 
        {
            controller.StateManager.GotoState(State.START_NEW_GAME);
        }

        public override void OnStateUpdate(Controller controller)
        {
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
            UpdateFooterMessage(controller);
        }
    }
}

// Created and programmed by Eric Milota, 2021

using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class ThanksForPlayingState : BaseState
    {
        public override State State => State.THANKS_FOR_PLAYING;

        public override void OnStateEnter(Controller controller)
        {
            controller.SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.REALLY_QUIT);
            controller.UI.HideBoard();
            controller.UI.HideBigMessage();

            controller.UI.ShowBigMessage(Localize.THANKS_FOR_PLAYING_BIG_MESSAGE);
            controller.UI.ShowFooterMessage(Localize.THANKS_FOR_PLAYING_FOOTER_MESSAGE);
        }

        public override void OnStateLeave(Controller controller)
        {
            controller.UI.HideTitle();
            controller.UI.HideFooterMessage();
        }

        public override void OnStateClickRestartOrQuitButton(Controller controller)
        {
            controller.StateManager.GotoState(State.WAIT_TO_QUIT_SCREEN);
        }

        public override void OnStateClickFullscreenButton(Controller controller)
        {
            controller.StateManager.GotoState(State.WAIT_TO_QUIT_SCREEN);
        }

        public override void OnStateUpdate(Controller controller)
        {
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
        }
    }
}

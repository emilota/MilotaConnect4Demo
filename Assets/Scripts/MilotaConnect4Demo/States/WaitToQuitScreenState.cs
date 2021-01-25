// Created and programmed by Eric Milota, 2021

using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class WaitToQuitScreenState : BaseState
    {
        public override State State => State.WAIT_TO_QUIT_SCREEN;

        public override void OnStateEnter(Controller controller)
        {
            controller.SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.NONE);
            controller.UI.HideBoard();
            controller.UI.HideTitle();
            controller.UI.HideBigMessage();
            controller.UI.HideFooterMessage();
        }

        public override void OnStateLeave(Controller controller)
        {
        }

        public override void OnStateClickRestartOrQuitButton(Controller controller)
        {
        }

        public override void OnStateClickFullscreenButton(Controller controller)
        {
        }

        public override void OnStateUpdate(Controller controller)
        {
            if (controller.StateManager.TimeInCurrentState >= (100)) // TODO: remove magic hard coded number...1/10th of a sec
            {
                controller.StateManager.GotoState(State.TITLE_SCREEN);
            }
            else
            {
                controller.QuitProgram(); // this might not do anything, so we've got a fallback
            }
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
        }
    }
}

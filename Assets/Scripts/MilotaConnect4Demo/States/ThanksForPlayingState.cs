using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class ThanksForPlayingState : BaseState
    {
        public override State State => State.THANKS_FOR_PLAYING;

        public override void OnStateEnter(Controller controller)
        {
            controller.SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.NONE);
            controller.UI.HideBoard();
            controller.UI.HideTitle();
            controller.UI.HideBigMessage();
            controller.UI.HideFooterMessage();

            controller.UI.ShowBigMessage(Localize.THANKS_FOR_PLAYING_BIG_MESSAGE);
            controller.UI.ShowFooterMessage(Localize.THANKS_FOR_PLAYING_FOOTER_MESSAGE);
        }

        public override void OnStateLeave(Controller controller)
        {
            controller.UI.HideBigMessage();
            controller.UI.HideFooterMessage();
        }

        public override void OnStateUpdate(Controller controller)
        {
            if ((Input.GetMouseButtonDown(0)) && 
                (!EventSystem.current.IsPointerOverGameObject())) // ignore if over button
            {
                controller.StateManager.GotoState(State.NONE); // exit!
            }
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
        }
    }
}

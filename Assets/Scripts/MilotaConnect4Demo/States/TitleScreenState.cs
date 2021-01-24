using UnityEngine;
using UnityEngine.EventSystems;

namespace MilotaConnect4Demo
{
    public class TitleScreenState : BaseState
    {
        public override State State => State.TITLE_SCREEN;

        public override void OnStateEnter(Controller controller)
        {
            controller.SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.QUIT);
            controller.UI.HideBoard();
            controller.UI.HideBigMessage();

            controller.UI.ShowTitle();
            controller.UI.ShowFooterMessage(Localize.TITLE_SCREEN_FOOTER_MESSAGE);
        }

        public override void OnStateLeave(Controller controller)
        {
            controller.SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.NONE);

            controller.UI.HideTitle();
            controller.UI.HideFooterMessage();
        }

        public override void OnStateUpdate(Controller controller)
        {
            if ((Input.GetMouseButtonDown(0)) && 
                (!EventSystem.current.IsPointerOverGameObject())) // ignore if over button
            {
                controller.StateManager.GotoState(State.START_NEW_GAME);
            }
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
        }
    }
}

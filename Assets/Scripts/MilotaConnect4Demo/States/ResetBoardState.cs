using System.Collections.Generic;

namespace MilotaConnect4Demo
{
    public class ResetBoardState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.RESET_BOARD;

        public override void OnStateEnter(Controller controller) 
        {
            controller.UI.HideBigMessage();
            controller.UI.HideFooterMessage();
            controller.Board.ConvertAllStaticCheckersIntoMovingCheckers();
        }

        public override void OnStateLeave(Controller controller) 
        {
        }

        public override void OnStateUpdate(Controller controller) 
        {
            if (controller.Board.CheckerManager.NumActiveCheckers <= 0)
            {
                controller.StateManager.GotoState(State.START_NEW_GAME);
            }
        }

        public override void OnStateFixedUpdate(Controller controller)
        {
        }
    }
}

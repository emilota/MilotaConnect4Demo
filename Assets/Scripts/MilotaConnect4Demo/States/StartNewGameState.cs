namespace MilotaConnect4Demo
{
    public class StartNewGameState : BaseState
    {
        public override MilotaConnect4Demo.State State => MilotaConnect4Demo.State.START_NEW_GAME;

        public override void OnStateEnter(Controller controller) 
        {
            controller.SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.RESTART);

            controller.Board.Empty();
            controller.Board.CreateGameObjects();
            controller.UI.ShowBoard();

            controller.UI.ShowBigMessage(Localize.START_NEW_GAME_LETS_PLAY);
        }

        public override void OnStateLeave(Controller controller) 
        {
            controller.UI.HideBigMessage();
        }

        public override void OnStateUpdate(Controller controller)
        {
            if (controller.StateManager.TimeInCurrentState >= controller.UI.StartNewGameMessageTimeInMS)
            {
                controller.Board.StartGame();
                controller.StateManager.GotoState(State.PLAYER_SELECT);
            }
        }

        public override void OnStateFixedUpdate(Controller controller) 
        { 
        }
    }
}

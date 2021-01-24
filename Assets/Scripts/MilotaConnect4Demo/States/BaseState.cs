namespace MilotaConnect4Demo
{
    public class BaseState
    {        
        public virtual MilotaConnect4Demo.State State => MilotaConnect4Demo.State.NONE;

        public virtual void OnStateEnter(Controller controller) { }
        public virtual void OnStateLeave(Controller controller) { }
        public virtual void OnStateUpdate(Controller controller) { }
        public virtual void OnStateFixedUpdate(Controller controller) { }
    }
}

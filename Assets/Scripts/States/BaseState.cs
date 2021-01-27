// Created and programmed by Eric Milota, 2021

namespace MilotaConnect4Demo
{
    public class BaseState
    {        
        public virtual MilotaConnect4Demo.State State => MilotaConnect4Demo.State.NONE;

        public virtual void OnStateEnter(App app) { }
        public virtual void OnStateLeave(App app) { }
        public virtual void OnStateUpdate(App app) { }
        public virtual void OnStateFixedUpdate(App app) { }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class StateManager
    {
        private Controller mController = null;
        private State mState = State.NONE;
        private Int64 mStateTimestamp = 0;
        private Dictionary<State,BaseState> mStateDict = new Dictionary<State,BaseState>();
        private BaseState mBaseState = null;

        public Controller Controller => mController;
        public State State => mState;
        public int TimeInCurrentState => (mStateTimestamp == 0 ? 0 : (int) (Util.GetMS() - mStateTimestamp)); // in MS
        public void ResetStateTime() { mStateTimestamp = Util.GetMS(); }

        public StateManager(Controller controller = null) { Init(controller); }

        public void Init(Controller controller = null)
        {
            Uninit();   // here so we can call init multiple times and it will correctly shutdown before reinitializing

            mController = controller;
            mState = State.NONE;
            mStateTimestamp = 0;
            mStateDict.Clear();
            if (mController != null)
            {
                // populate state dictionary for fast State -> BaseState derived lookups
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeof(BaseState));
                Type[] typeArray = assembly.GetTypes(); 
                for(int index = 0; index < typeArray.Length; index++)
                {
                    Type type = typeArray[index];
                    if (typeof(BaseState).IsAssignableFrom(type)) // Let's only add those derived from BaseState (or BaseState itself)
                    {
                        BaseState baseState = Activator.CreateInstance(type) as BaseState;
                        mStateDict.Add(baseState.State, baseState);
                    }
                } 
            }
        }

        public void Uninit()
        {
            mController = null;
            mState = State.NONE;
            mStateTimestamp = 0;
            mStateDict.Clear();
        }

        public BaseState FindStateObject(State state) // can return null
        {
            if (state == State.NONE)
                return null;
            return mStateDict[ state ];
        }

        private void OnStateEnter()
        {
            mBaseState?.OnStateEnter(mController);
        }

        private void OnStateLeave()
        {
            mBaseState?.OnStateLeave(mController);
        }

        public void OnStateUpdate()
        {
            mBaseState?.OnStateUpdate(mController);
        }

        public void OnStateFixedUpdate()
        {
            mBaseState?.OnStateFixedUpdate(mController);
        }

        public void GotoState(State stateNew)
        {
            State stateOld = mState;
            if ((stateNew == stateOld) && (mBaseState != null))
                return; // already in this state

            // leave current state
            OnStateLeave();

            // change states
            mBaseState = FindStateObject(stateNew);
            mState = stateNew;
            mStateTimestamp = Util.GetMS();

            if (stateNew == State.NONE)
            {
                mController?.QuitProgram();
            }
            else
            {
                // enter new state
                OnStateEnter();
            }
        }
    }
}

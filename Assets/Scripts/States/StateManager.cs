// Created and programmed by Eric Milota, 2021

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class StateManager
    {
        private App mApp = null;
        private State mState = State.NONE;
        private Int64 mStateTimestamp = 0;
        private Dictionary<State,BaseState> mStateDict = new Dictionary<State,BaseState>();
        private BaseState mBaseState = null;

        public App App => mApp;
        public State State => mState;
        public int TimeInCurrentState => (mStateTimestamp == 0 ? 0 : (int) (Util.GetMS() - mStateTimestamp)); // in MS
        public void ResetStateTime() { mStateTimestamp = Util.GetMS(); }

        public StateManager(App app = null) { Init(app); }

        public void Init(App app = null)
        {
            Uninit();   // here so we can call init multiple times and it will correctly shutdown before reinitializing

            mApp = app;
            mState = State.NONE;
            mStateTimestamp = 0;
            mStateDict.Clear();
            if (mApp != null)
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
            mApp = null;
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
            mBaseState?.OnStateEnter(mApp);
        }

        private void OnStateLeave()
        {
            mBaseState?.OnStateLeave(mApp);
        }

        public void OnStateUpdate()
        {
            if ((mApp != null) && (!mApp.SceneManager.IsSceneLoading))
            {
                mBaseState?.OnStateUpdate(mApp);
            }
        }

        public void OnStateFixedUpdate()
        {
            if ((mApp != null) && (!mApp.SceneManager.IsSceneLoading))
            {
                mBaseState?.OnStateFixedUpdate(mApp);
            }
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

            // enter new state
            OnStateEnter();
        }
    }
}

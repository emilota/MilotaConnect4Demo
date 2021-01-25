// Created and programmed by Eric Milota, 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class Controller : MonoBehaviour
    {
        private UI mUI = null;
        private StateManager mStateManager = new StateManager();
        private Board mBoard = new Board();
        private RestartOrQuitButtonMode mRestartOrQuitButtonMode = RestartOrQuitButtonMode.NONE;

        public UI UI => mUI;
        public StateManager StateManager => mStateManager;
        public Board Board => mBoard;

        public void SetRestartOrQuitButtonMode(RestartOrQuitButtonMode restartOrQuitButtonMode)
        {
            mRestartOrQuitButtonMode = restartOrQuitButtonMode;
            switch (mRestartOrQuitButtonMode)
            {
                case RestartOrQuitButtonMode.NONE:
                    {
                        TMPro.TextMeshProUGUI text = this.UI.GORestartOrQuitButtonText.GetComponent<TMPro.TextMeshProUGUI>();
                        text.text = Localize.RESTART_OR_QUIT_BUTTON_NONE;
                        break;
                    }
                case RestartOrQuitButtonMode.RESTART:
                    {
                        TMPro.TextMeshProUGUI text = this.UI.GORestartOrQuitButtonText.GetComponent<TMPro.TextMeshProUGUI>();
                        text.text = Localize.RESTART_OR_QUIT_BUTTON_RESTART;
                        break;
                    }
                case RestartOrQuitButtonMode.QUIT:
                    {
                        TMPro.TextMeshProUGUI text = this.UI.GORestartOrQuitButtonText.GetComponent<TMPro.TextMeshProUGUI>();
                        text.text = Localize.RESTART_OR_QUIT_BUTTON_QUIT;
                        break;
                    }
                case RestartOrQuitButtonMode.REALLY_QUIT:
                    {
                        TMPro.TextMeshProUGUI text = this.UI.GORestartOrQuitButtonText.GetComponent<TMPro.TextMeshProUGUI>();
                        text.text = Localize.RESTART_OR_QUIT_BUTTON_REALLY_QUIT;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void Awake()
        {
            mUI = FindObjectOfType<UI>();
            mStateManager.Init(this);
            mBoard.Init(
                this,
                mUI.BoardNumColumns,
                mUI.BoardNumRows,
                mUI.BoardNumCheckersInARowToWin);

            // sanity check
            if (!mBoard.IsValidBoard)
            {
                Debug.LogError("Error, params used to build game board aren't valid!  Please recheck UI.cs values.");
            }
        }
        
        public void OnClickRestartOrQuitButton()
        {
            mStateManager.OnStateClickRestartOrQuitButton();
        }

        public void OnClickFullscreenButton()
        {
            mStateManager.OnStateClickFullscreenButton();
        }

        public void Start()
        {
            StartProgram();
        }

        public void StartProgram()
        {
            mStateManager.GotoState(State.TITLE_SCREEN);
        }

        public void QuitProgram()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else // #if UNITY_EDITOR
            if (Application.platform == RuntimePlatform.Android)
            {
                // Android is strange.  You can't actually quit an application.  
                // You must instead just move your activity to the background.

                // find our activity and move it to be background
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call<bool>("moveTaskToBack", true);
            }
            else
            {
                Application.Quit();
            }
#endif // #else // #if UNITY_EDITOR
        }

        public void Update()
        {
            // update current state
            mStateManager.OnStateUpdate();
        }

        public void FixedUpdate()
        {
            // update current state
            mStateManager.OnStateFixedUpdate();

            // update game board
            mBoard.OnBoardFixedUpdate();
        }
    }
}

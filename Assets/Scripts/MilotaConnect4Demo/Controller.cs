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
            switch(mRestartOrQuitButtonMode)
            {
                case RestartOrQuitButtonMode.NONE:
                    {
                        mUI.GORestartOrQuitButton.SetActive(false);
                        break;
                    }
                case RestartOrQuitButtonMode.RESTART:
                    {
                        TMPro.TextMeshProUGUI text = this.UI.GORestartOrQuitButtonText.GetComponent<TMPro.TextMeshProUGUI>();
                        text.text = Localize.RESTART_OR_QUIT_BUTTON_RESTART;
                        mUI.GORestartOrQuitButton.SetActive(true);
                        break;
                    }
                case RestartOrQuitButtonMode.QUIT:
                    {
                        TMPro.TextMeshProUGUI text = this.UI.GORestartOrQuitButtonText.GetComponent<TMPro.TextMeshProUGUI>();
                        text.text = Localize.RESTART_OR_QUIT_BUTTON_QUIT;
                        mUI.GORestartOrQuitButton.SetActive(true);
                        break;
                    }
                default:
                    {
                        mUI.GORestartOrQuitButton.SetActive(false);
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
            switch(mRestartOrQuitButtonMode)
            {
                case RestartOrQuitButtonMode.NONE:
                    {
                        break;
                    }
                case RestartOrQuitButtonMode.RESTART:
                    {
                        this.StateManager.GotoState(State.TITLE_SCREEN);
                        break;
                    }
                case RestartOrQuitButtonMode.QUIT:
                    {
                        this.StateManager.GotoState(State.THANKS_FOR_PLAYING);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void Start()
        {
            SetRestartOrQuitButtonMode(RestartOrQuitButtonMode.NONE);
            mStateManager.GotoState(State.TITLE_SCREEN);
        }

        public void QuitProgram()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else // #if UNITY_EDITOR
            UnityEngine.Application.Quit();		
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

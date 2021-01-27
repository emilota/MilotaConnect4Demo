// Created and programmed by Eric Milota, 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class App
    {
        private static App gInstance = null;
        public static App Instance => gInstance;

        public static App InitIfNeeded()
        {
            if (gInstance == null)
            {
                gInstance = new App();
            }

            return gInstance;
        }

        //-------------------------------------

        private StateManager mStateManager = new StateManager();
        private SceneManager mSceneManager = new SceneManager();
        private Board mBoard = new Board();

        private SplashSceneMB mSplashSceneMB = null;
        private GameSceneMB mGameSceneMB = null;
        private ExitSceneMB mExitSceneMB = null;

        //-------------------------------------

        public StateManager StateManager => mStateManager;
        public SceneManager SceneManager => mSceneManager;
        public Board Board => mBoard;

        public SplashSceneMB SplashSceneMB => mSplashSceneMB;
        public void SetSplashSceneMB(SplashSceneMB splashSceneMB) { mSplashSceneMB = splashSceneMB; }
        public GameSceneMB GameSceneMB => mGameSceneMB;
        public void SetGameSceneMB(GameSceneMB gameSceneMB) { mGameSceneMB = gameSceneMB; }
        public ExitSceneMB ExitSceneMB => mExitSceneMB;
        public void SetExitSceneMB(ExitSceneMB exitSceneMB) { mExitSceneMB = exitSceneMB; }

        //-------------------------------------

        public App() { Init(); }

        public void Init()
        {
            Uninit();

            mStateManager.Init(this);
            mSceneManager.Init();
            mBoard.Init(this);

            SetSplashSceneMB(null);
            SetGameSceneMB(null);
            SetExitSceneMB(null);
        }

        public void Uninit()
        {
            mSceneManager.GotoSceneASync(SceneEnum.NONE, () =>
            {
                mStateManager.Uninit();
                mBoard.Uninit();
                mSceneManager.Uninit();

                SetSplashSceneMB(null);
                SetGameSceneMB(null);
                SetExitSceneMB(null);
            });
        }

        public void OnAppClick(Click click)
        {
            switch(click)
            {
                case Click.NONE:
                    {
                        break;
                    }
                case Click.START_GAME_BUTTON:
                    {
                        mStateManager.GotoState(State.START_NEW_GAME);
                        break;
                    }
                case Click.LEAVE_APP_BUTTON:
                    {
                        mStateManager.GotoState(State.THANKS_FOR_PLAYING);
                        break;
                    }
                case Click.PLAYER_DROP_CHECKER_BUTTON:
                    {
                        OnSomePlayerDropCheckerButtonPressed();
                        break;
                    }
                case Click.PLAYER_RESET_BOARD_BUTTON:
                    {
                        mStateManager.GotoState(State.RESET_BOARD);
                        break;
                    }
                case Click.RESET_GAME_BUTTON:
                    {
                        mStateManager.GotoState(State.TITLE_SCREEN);
                        break;
                    }
                case Click.PLAY_AGAIN_BUTTON:
                    {
                        mStateManager.GotoState(State.RESET_BOARD);
                        break;
                    }
                case Click.QUIT_APPLICATION_BUTTON:
                    {
                        QuitProgram();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public void OnSomePlayerDropCheckerButtonPressed()
        {
            if (mStateManager.State != State.PLAYER_SELECT)
                return;
            switch (mBoard.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        // it's my turn

                        // show/hide 3D cursor
                        RaycastHit raycastHit;
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (Physics.Raycast(ray, out raycastHit))
                        {
                            int col = Const.INVALID_COL_VALUE;
                            int row = Const.INVALID_ROW_VALUE;
                            if (mBoard.FindColumnFromPlaneCoordX(
                                raycastHit.point.x,
                                ref col,
                                ref row))
                            {
                                mBoard.ShowCheckerSelect(
                                    col,
                                    row);
                            }
                            else
                            {
                                mBoard.HideCheckerSelect();
                            }
                        }
                        else
                        {
                            mBoard.HideCheckerSelect();
                        }

                        if (mBoard.IsCheckerSelectShowing)
                        {
                            // drop checker....maybe
                            if (mBoard.TryDropChecker(
                                    mBoard.WhichPlayerCurrent,
                                    mBoard.CheckerSelectCol,
                                    mBoard.CheckerSelectRow))
                            {
                                // player 1 human dropped a checker
                                mStateManager.GotoState(State.PLAYER_MOVE);
                            }
                        }
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        // it's AI's turn
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
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

                mStateManager.GotoState(State.TITLE_SCREEN);
    
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

        public void OnAppUpdate()
        {
            // update current state
            mStateManager.OnStateUpdate();
        }

        public void OnAppFixedUpdate()
        {
            // update current state
            mStateManager.OnStateFixedUpdate();

            // update game board
            mBoard.OnBoardFixedUpdate();
        }
    }
}

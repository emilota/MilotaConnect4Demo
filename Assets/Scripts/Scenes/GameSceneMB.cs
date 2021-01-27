// Created and programmed by Eric Milota, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class GameSceneMB : MonoBehaviour
    {
        private App mApp = null;

        //---------------------------------------

        public int BoardNumColumns = Const.BOARD_NUM_COLUMNS;
        public int BoardNumRows = Const.BOARD_NUM_ROWS;
        public int BoardNumCheckersInARowToWin = Const.BOARD_NUM_CHECKERS_IN_A_ROW_TO_WIN;

        public bool DebugHumanPlayerAlwaysStartsFirst = Const.DEBUG_HUMAN_PLAYER_ALWAYS_STARTS_FIRST;
        public AIPlayStyle DebugAIPlayStyle = Const.DEBUG_AI_PLAY_STYLE;

        public int StartNewGameMessageTimeInMS = Const.START_NEW_GAME_MESSAGE_TIME_IN_MS;
        public int FooterBlinkRateInMS = Const.FOOTER_BLINK_RATE_IN_MS;
        public int SelectCheckerBlinkMS = Const.SELECT_CHECKER_BLINK_MS;
        public float CheckerDropScale = Const.CHECKER_DROP_SCALE;
        public int GameOverMessageBlinkInMS = Const.GAME_OVER_MESSAGE_BLINK_MS;
        public int WinningCheckersBlinkInMS = Const.WINNING_CHECKERS_BLINK_IN_MS;

        // game objects already in scene
        public GameObject GOBoardRoot;
        public GameObject GOAnchorUL;
        public GameObject GOAnchorUR;
        public GameObject GOAnchorLL;
        public GameObject GOResetGameMessageText;
        public GameObject GOBigMessage;
        public GameObject GOFooterMessage;

        // references to prefabs
        public GameObject PrefabQuestionMark;
        public GameObject PrefabPlayer1Checker;
        public GameObject PrefabPlayer2Checker;

        //----------------------------------------------------------------------

        public void Awake()
        {
            mApp = App.InitIfNeeded();
            mApp.SetGameSceneMB(this);

            Util.SetGameObjectTextMeshProText(this.GOResetGameMessageText, Localize.GAME_RESET_GAME_BUTTON_TEXT);

            HideAnchorsAndMessages();

            mApp.SceneManager.FinishedWithSceneAwake(SceneEnum.SPLASH_SCENE);
        }

        ~GameSceneMB()
        {
            HideAnchorsAndMessages();

            mApp.SetGameSceneMB(null);

            mApp = null;
        }

        public void HideAnchorsAndMessages()
        {
            HideAnchors();
            HideBigMessage();
            HideFooterMessage();
        }

        public void HideAnchors()
        {
            this.GOAnchorUL.SetActive(false); // used internally
            this.GOAnchorUR.SetActive(false); // used internally
            this.GOAnchorLL.SetActive(false); // used internally
        }

        public void ShowAnchors()
        {
            this.GOAnchorUL.SetActive(true); // used internally
            this.GOAnchorUR.SetActive(true); // used internally
            this.GOAnchorLL.SetActive(true); // used internally
        }

        public void ShowBigMessage(string message)
        {
            Util.SetGameObjectTextMeshProText(this.GOBigMessage, message);

            this.GOBigMessage.SetActive(true);
        }

        public void HideBigMessage()
        {
            this.GOBigMessage.SetActive(false);
        }

        public void ShowFooterMessage(string message)
        {
            Util.SetGameObjectTextMeshProText(this.GOFooterMessage, message);

            this.GOFooterMessage.SetActive(true);
        }

        public void HideFooterMessage()
        {
            this.GOFooterMessage.SetActive(false);
        }

        public void OnClickResetGameButton()
        {
            mApp.OnAppClick(Click.RESET_GAME_BUTTON);
        }

        public void OnClickFullscreenButton()
        {
            if (mApp.StateManager.State == State.GAME_OVER) 
                mApp.OnAppClick(Click.PLAYER_RESET_BOARD_BUTTON);
            else
                mApp.OnAppClick(Click.PLAYER_DROP_CHECKER_BUTTON);
        }

        public void Update()
        {
            mApp.OnAppUpdate();
        }

        public void FixedUpdate()
        {
            mApp.OnAppFixedUpdate();
        }
    }
}


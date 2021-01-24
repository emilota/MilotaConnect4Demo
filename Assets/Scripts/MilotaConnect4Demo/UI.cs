using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class UI : MonoBehaviour
    {
        public int BoardNumColumns = Const.BOARD_NUM_COLUMNS;
        public int BoardNumRows = Const.BOARD_NUM_ROWS;
        public int BoardNumCheckersInARowToWin = Const.BOARD_NUM_CHECKERS_IN_A_ROW_TO_WIN;

        public bool DebugHumanPlayerAlwaysStartsFirst = Const.DEBUG_HUMAN_PLAYER_ALWAYS_STARTS_FIRST;
        public AIPlayStyle DebugAIPlayStyle = Const.DEBUG_AI_PLAY_STYLE;

        public int StartNewGameMessageTimeInMS = Const.START_NEW_GAME_MESSAGE_TIME_IN_MS;
        public int SelectCheckerBlinkMS = Const.SELECT_CHECKER_BLINK_MS;
        public float CheckerDropScale = Const.CHECKER_DROP_SCALE;
        public int GameOverMessageBlinkInMS = Const.GAME_OVER_MESSAGE_BLINK_MS;
        public int WinningCheckersBlinkInMS = Const.WINNING_CHECKERS_BLINK_IN_MS;

        // game objects already in scene
        public GameObject GORestartOrQuitButton;
        public GameObject GORestartOrQuitButtonText;
        public GameObject GOTitle;
        public GameObject GOBoard;
        public GameObject GOBoardModel;
        public GameObject GOAnchorUL;
        public GameObject GOAnchorUR;
        public GameObject GOAnchorLL; 
        public GameObject GOBigMessage;
        public GameObject GOFooterMessage;

        // references to prefabs
        public GameObject PrefabQuestionMark;
        public GameObject PrefabPlayer1Checker;
        public GameObject PrefabPlayer2Checker;

        public void Awake()
        {
            HideTitle();
            HideBoard();
            HideAnchors();
            HideBigMessage();
            HideFooterMessage();
        }

        public void ShowTitle()
        {
            this.GOTitle.SetActive(true);
        }

        public void HideTitle()
        {
            this.GOTitle.SetActive(false);
        }

        public void ShowBoard()
        {
            this.GOBoard.SetActive(true);
        }

        public void HideBoard()
        {
            this.GOBoard.SetActive(false);
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
            message = message ?? "";

            TextMeshPro text = this.GOBigMessage.GetComponent<TextMeshPro>();
            text.text = message;

            this.GOBigMessage.SetActive(true);
        }

        public void HideBigMessage()
        {
            this.GOBigMessage.SetActive(false);
        }

        public void ShowFooterMessage(string message)
        {
            message = message ?? "";

            TextMeshPro text = this.GOFooterMessage.GetComponent<TextMeshPro>();
            text.text = message;

            this.GOFooterMessage.SetActive(true);
        }

        public void HideFooterMessage()
        {
            this.GOFooterMessage.SetActive(false);
        }
    }
}


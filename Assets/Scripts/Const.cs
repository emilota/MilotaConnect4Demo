// Created and programmed by Eric Milota, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class Const
    {
        public const ScreenOrientationManagerMode APP_SCREEN_ORIENTATION_MANAGER_MODE = ScreenOrientationManagerMode.ANY_LANDSCAPE_ORIENTATION_OK;
        public const int APP_SCREEN_ORIENTATION_MANAGER_MS_BETWEEN_PUMPS = 100; // 1/10th of a second
        public const int APP_SCREEN_ORIENTATION_MANAGER_MS_TO_RESTORE = 500;    // 1/2 of a second

        public const int INVALID_COL_VALUE = -1;
        public const int INVALID_ROW_VALUE = -1;

        // UI seed values
        public const int BOARD_NUM_COLUMNS = 7;
        public const int BOARD_NUM_ROWS = 6;
        public const int BOARD_NUM_CHECKERS_IN_A_ROW_TO_WIN = 4;

        public const bool DEBUG_HUMAN_PLAYER_ALWAYS_STARTS_FIRST = false; // DEBUGGING TOOL:  Do humans always start first?
        public const AIPlayStyle DEBUG_AI_PLAY_STYLE = AIPlayStyle.SOME_SMARTS; // DEBUGGING TOOL: Make AI play a specific way

        public const int START_NEW_GAME_MESSAGE_TIME_IN_MS = 1*1000; // 1 second

        public const int FOOTER_BLINK_RATE_IN_MS = 500; // 1/2 sec

        public const int SELECT_CHECKER_BLINK_MS = 300; // 300ms
        public const float CHECKER_DROP_SCALE = 0.2f; // it's a % of the delta between rows

        public const int GAME_OVER_MESSAGE_BLINK_MS = 2*1000; // 2 seconds
        public const int WINNING_CHECKERS_BLINK_IN_MS = 200; // 1/5 of a second
    }
}


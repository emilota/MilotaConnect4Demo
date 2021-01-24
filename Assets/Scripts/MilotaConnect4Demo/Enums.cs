using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public enum State
    {
        NONE,
        TITLE_SCREEN,
        START_NEW_GAME,
        PLAYER_SELECT,
        PLAYER_MOVE,
        GAME_OVER,
        RESET_BOARD,
        THANKS_FOR_PLAYING
    }

    public enum RestartOrQuitButtonMode
    {
        NONE,
        RESTART,
        QUIT
    }

    public enum WhichPlayer : int
    {
        NONE = 0,

        PLAYER_1_HUMAN = 1,
        PLAYER_2_AI = 2,

        PLAYER_MIN = PLAYER_1_HUMAN,
        PLAYER_MAX = PLAYER_2_AI
    }

    public enum AIPlayStyle
    {
        NONE = 0,
        ALL_LEFT,
        ALL_RIGHT,
        LEFT_THEN_RIGHT,
        RANDOM,
        SOME_SMARTS
    }
}

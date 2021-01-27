// Created and programmed by Eric Milota, 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public enum SceneEnum
    {
        NONE,

        SPLASH_SCENE,
        GAME_SCENE,
        EXIT_SCENE
    }

    public enum State
    {
        NONE,

        TITLE_SCREEN,   // goto splash scene

        START_NEW_GAME, // goto game scene
        PLAYER_SELECT,
        PLAYER_MOVE,
        GAME_OVER,
        RESET_BOARD,

        THANKS_FOR_PLAYING  // goto exit scene
    }

    public enum Click
    {
        NONE,

        START_GAME_BUTTON,
        LEAVE_APP_BUTTON,
        PLAYER_DROP_CHECKER_BUTTON,
        PLAYER_RESET_BOARD_BUTTON,
        RESET_GAME_BUTTON,
        PLAY_AGAIN_BUTTON,
        QUIT_APPLICATION_BUTTON
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

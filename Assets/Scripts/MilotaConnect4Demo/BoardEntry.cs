// Created and programmed by Eric Milota, 2021

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class BoardEntry // used to represent a spot on our game board
    {
        public Board Board = null;
        public WhichPlayer WhichPlayerChecker = WhichPlayer.NONE;
        public bool Blink = false;
        public GameObject GOPlayer1Checker = null;
        public GameObject GOPlayer2Checker = null;

        public BoardEntry(Board board, WhichPlayer whichPlayerChecker, bool blink)
        {
            this.Board = board;
            this.WhichPlayerChecker = whichPlayerChecker;
            this.Blink = blink;
            this.ResetGORefs();
        }

        public void ResetGORefs()
        {
            this.GOPlayer1Checker = null;
            this.GOPlayer2Checker = null;
        }

        public void UpdateCheckerShowHide()
        {
            bool showPlayer1Checker = false;
            bool showPlayer2Checker = false;
            switch (this.WhichPlayerChecker)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        showPlayer1Checker = true;
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        showPlayer2Checker = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            if (this.Blink)
            {
                if (Util.Blink(this.Board.Controller.UI.WinningCheckersBlinkInMS))
                {
                    showPlayer1Checker = false;
                    showPlayer2Checker = false;
                }
            }

            if (this.GOPlayer1Checker != null)
                this.GOPlayer1Checker.SetActive(showPlayer1Checker);
            if (this.GOPlayer2Checker != null)
                this.GOPlayer2Checker.SetActive(showPlayer2Checker);
        }
    }
}

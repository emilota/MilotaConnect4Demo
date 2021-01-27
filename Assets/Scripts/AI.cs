// Created and programmed by Eric Milota, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class AI
    {
        private Board mBoard = null;
        private int mLeftTheRightCounter = 0;

        public AI(Board board = null) { Init(board); }

        public void Init(Board board = null)
        {
            Uninit();

            mBoard = board;
            mLeftTheRightCounter = 0;
        }

        public void Uninit()
        {
            mBoard = null;
            mLeftTheRightCounter = 0;
        }

        public void OnGameStart(App app)
        {
            // give AI a place to reset stuff at the beginning of a new game
            mLeftTheRightCounter = 0;
        }

        public void OnTurnStart(App app)
        {
            // a turn has started...it could be us, it could be a human
        }

        public bool TryMove(App app, int col)
        {
            int row = Const.INVALID_COL_VALUE;
            if (app.Board.CanDropOnCol(col, ref row))
            {
                // drop here!
                if (app.Board.TryDropChecker(
                        app.Board.WhichPlayerCurrent,
                        col,
                        row))
                {
                    return true;    // made a move!
                }
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_None(App app)
        {
            // while this isn't in the game rules, the AI is abicating their turn.
            // So they are saying they did a move (when they really didn't)
            return true;    // made a move!
        }

        public bool OnTryToMakeAMove_AllLeft(App app)
        {
            for(int col = 0; col < app.Board.NumCols; col++)
            {
                if (TryMove(app, col))
                    return true;    // moved!
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_AllRight(App app)
        {
            for (int col = app.Board.NumCols - 1; col >= 0; col--)
            {
                if (TryMove(app, col))
                    return true;    // moved!
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_LeftThenRight(App app)
        {
            bool didAMove = false;
            if (mLeftTheRightCounter == 0)
                didAMove = OnTryToMakeAMove_AllLeft(app);
            else
                didAMove = OnTryToMakeAMove_AllRight(app);
            if (didAMove)
            {
                mLeftTheRightCounter++;
                if (mLeftTheRightCounter > 1)
                    mLeftTheRightCounter = 0;
                return true; // moved!
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_Random(App app)
        {
            int col = Util.GetRandomRange(0, app.Board.NumCols - 1);
            if (TryMove(app, col))
                return true;    // moved!
            return false;   // didn't make a move
        }
        
        public enum AIMoveStatus
        {
            NONE = 0,
            NO_MOVE,
            WE_CAN_WIN,
            WE_CAN_BLOCK,
            NOTHING_SPECIAL
        }

        public AIMoveStatus ComputeAIMoveStatusForCol(
            App app,
            int col,
            ref int rowRef,
            WhichPlayer whichPlayerMe,
            WhichPlayer whichPlayerOther)
        {
            rowRef = Const.INVALID_COL_VALUE;
            int row = Const.INVALID_COL_VALUE;
            if (!app.Board.CanDropOnCol(col, ref row))
            {
                // can't move here
                return AIMoveStatus.NO_MOVE;
            }
            rowRef = row;

            List<BoardCoord> boardCoordList = null;

            // see if we can win from here
            WhichPlayer whichPlayerWinner = WhichPlayer.NONE;

            app.Board.SetBoardEntryInfo(col, row, whichPlayerMe, false, false); // let's put ourself there
            bool thereIsAWin =  app.Board.CheckForWin(
                ref whichPlayerWinner,
                ref boardCoordList);
            app.Board.SetBoardEntryInfo(col, row, WhichPlayer.NONE, false, false); // restore

            if ((thereIsAWin) && (whichPlayerWinner == whichPlayerMe))
            {
                // we can win if we move here
                return AIMoveStatus.WE_CAN_WIN;
            }

            // see if other player went here if they'd win or not
            app.Board.SetBoardEntryInfo(col, row, whichPlayerOther, false, false); // let's put ourself there
            thereIsAWin =  app.Board.CheckForWin(
                ref whichPlayerWinner,
                ref boardCoordList);
            app.Board.SetBoardEntryInfo(col, row, WhichPlayer.NONE, false, false); // restore

            // see if we can lose from here
            if ((thereIsAWin) && (whichPlayerWinner == whichPlayerOther))
            {
                // other player would win if they went there, let's mark this a block space
                return AIMoveStatus.WE_CAN_BLOCK;
            }

            return AIMoveStatus.NOTHING_SPECIAL;
        }

        public bool OnTryToMakeAMove_SomeSmarts(App app)
        {
            // TODO: This could totally be optimized.  Just slapping it together
            // to test out some things

            WhichPlayer whichPlayerMe = app.Board.WhichPlayerCurrent;
            WhichPlayer whichPlayerOther =
                (whichPlayerMe == WhichPlayer.PLAYER_2_AI
                    ? WhichPlayer.PLAYER_1_HUMAN
                    : WhichPlayer.PLAYER_2_AI);

            // TODO:  Keep these lists around instead of creating garbage
            List<int> winList = new List<int>();
            List<int> blockList = new List<int>();
            List<int> span4List = new List<int>();
            List<int> span3List = new List<int>();
            List<int> span2List = new List<int>();
            List<int> span0And1List = new List<int>();
            List<List<int>> listList = new List<List<int>>(); // our list of our lists, prioritized
            listList.Add(winList);
            listList.Add(blockList);
            listList.Add(span4List);
            listList.Add(span3List);
            listList.Add(span2List);
            listList.Add(span0And1List);

            int row = Const.INVALID_ROW_VALUE;
            for (int col = 0; col < app.Board.NumCols; col++)
            {
                AIMoveStatus  aiMoveStatus = ComputeAIMoveStatusForCol(
                    app, 
                    col,
                    ref row,
                    app.Board.WhichPlayerCurrent,
                    whichPlayerOther);
                switch(aiMoveStatus)
                {
                    case AIMoveStatus.NONE:
                        {
                            break;
                        }
                    case AIMoveStatus.NO_MOVE:
                        {
                            break;
                        }
                    case AIMoveStatus.WE_CAN_WIN:
                        {
                            winList.Add(col);
                            break;
                        }
                    case AIMoveStatus.WE_CAN_BLOCK:
                        {
                            blockList.Add(col);
                            break;
                        }
                    case AIMoveStatus.NOTHING_SPECIAL:
                        {
                            app.Board.SetBoardEntryInfo(col, row, whichPlayerMe, false, false); // let's put ourself there
                            int greatestSpan = app.Board.ComputeGreatestSpanFromCoord(col, row);
                            app.Board.SetBoardEntryInfo(col, row, WhichPlayer.NONE, false, false); // restore
                            if (greatestSpan > 3)
                                span4List.Add(col);
                            else if (greatestSpan > 2)
                                span3List.Add(col);
                            else if (greatestSpan > 1)
                                span2List.Add(col);
                            else
                                span0And1List.Add(col); // treat 2-grouping and lone placement same
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            for(int index = 0; index < listList.Count; index++)
            {
                List<int> list = listList[index];

                string debugListString = Util.GetIntListString(list);

                if (list.Count > 0)
                {
                    list.ShuffleList();
                    for (int index2 = 0; index2 < list.Count; index2++)
                    {
                        if (TryMove(app, list[ index2 ]))
                            return true;    // moved!
                    }
                }
            }

            // we shouldn't get here, but if we do, let's just pick something random
            return OnTryToMakeAMove_Random(app); 
        }

        public bool OnTryToMakeAMove(App app)
        {
            // this is where it's the AI's turn and they
            // need to figure out where to move

            switch (app.GameSceneMB.DebugAIPlayStyle)
            {
                case AIPlayStyle.NONE:
                    return OnTryToMakeAMove_None(app);
                case AIPlayStyle.ALL_LEFT:
                    return OnTryToMakeAMove_AllLeft(app);
                case AIPlayStyle.ALL_RIGHT:
                    return OnTryToMakeAMove_AllRight(app);
                case AIPlayStyle.LEFT_THEN_RIGHT:
                    return OnTryToMakeAMove_LeftThenRight(app);
                case AIPlayStyle.RANDOM:
                    return OnTryToMakeAMove_Random(app);
                case AIPlayStyle.SOME_SMARTS:
                    return OnTryToMakeAMove_SomeSmarts(app);
                default:
                    break;
            }
            return OnTryToMakeAMove_Random(app);
        }
    }
}


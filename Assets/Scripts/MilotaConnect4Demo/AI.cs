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

        public void OnGameStart(Controller controller)
        {
            // give AI a place to reset stuff at the beginning of a new game
            mLeftTheRightCounter = 0;
        }

        public void OnTurnStart(Controller controller)
        {
            // a turn has started...it could be us, it could be a human
        }

        public bool TryMove(Controller controller, int col)
        {
            int row = Const.INVALID_COL_VALUE;
            if (controller.Board.CanDropOnCol(col, ref row))
            {
                // drop here!
                if (controller.Board.TryDropChecker(
                        controller.Board.WhichPlayerCurrent,
                        col,
                        row))
                {
                    return true;    // made a move!
                }
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_None(Controller controller)
        {
            // while this isn't in the game rules, the AI is abicating their turn.
            // So they are saying they did a move (when they really didn't)
            return true;    // made a move!
        }

        public bool OnTryToMakeAMove_AllLeft(Controller controller)
        {
            for(int col = 0; col < controller.Board.NumCols; col++)
            {
                if (TryMove(controller, col))
                    return true;    // moved!
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_AllRight(Controller controller)
        {
            for (int col = controller.Board.NumCols - 1; col >= 0; col--)
            {
                if (TryMove(controller, col))
                    return true;    // moved!
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_LeftThenRight(Controller controller)
        {
            bool didAMove = false;
            if (mLeftTheRightCounter == 0)
                didAMove = OnTryToMakeAMove_AllLeft(controller);
            else
                didAMove = OnTryToMakeAMove_AllRight(controller);
            if (didAMove)
            {
                mLeftTheRightCounter++;
                if (mLeftTheRightCounter > 1)
                    mLeftTheRightCounter = 0;
                return true; // moved!
            }
            return false;   // didn't make a move
        }

        public bool OnTryToMakeAMove_Random(Controller controller)
        {
            int col = Util.GetRandomRange(0, controller.Board.NumCols - 1);
            if (TryMove(controller, col))
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
            Controller controller,
            int col,
            ref int rowRef,
            WhichPlayer whichPlayerMe,
            WhichPlayer whichPlayerOther)
        {
            rowRef = Const.INVALID_COL_VALUE;
            int row = Const.INVALID_COL_VALUE;
            if (!controller.Board.CanDropOnCol(col, ref row))
            {
                // can't move here
                return AIMoveStatus.NO_MOVE;
            }
            rowRef = row;

            List<BoardCoord> boardCoordList = null;

            // see if we can win from here
            WhichPlayer whichPlayerWinner = WhichPlayer.NONE;

            controller.Board.SetBoardEntryInfo(col, row, whichPlayerMe, false, false); // let's put ourself there
            bool thereIsAWin =  controller.Board.CheckForWin(
                ref whichPlayerWinner,
                ref boardCoordList);
            controller.Board.SetBoardEntryInfo(col, row, WhichPlayer.NONE, false, false); // restore

            if ((thereIsAWin) && (whichPlayerWinner == whichPlayerMe))
            {
                // we can win if we move here
                return AIMoveStatus.WE_CAN_WIN;
            }

            // see if other player went here if they'd win or not
            controller.Board.SetBoardEntryInfo(col, row, whichPlayerOther, false, false); // let's put ourself there
            thereIsAWin =  controller.Board.CheckForWin(
                ref whichPlayerWinner,
                ref boardCoordList);
            controller.Board.SetBoardEntryInfo(col, row, WhichPlayer.NONE, false, false); // restore

            // see if we can lose from here
            if ((thereIsAWin) && (whichPlayerWinner == whichPlayerOther))
            {
                // other player would win if they went there, let's mark this a block space
                return AIMoveStatus.WE_CAN_BLOCK;
            }

            return AIMoveStatus.NOTHING_SPECIAL;
        }

        public bool OnTryToMakeAMove_SomeSmarts(Controller controller)
        {
            // TODO: This could totally be optimized.  Just slapping it together
            // to test out some things

            WhichPlayer whichPlayerMe = controller.Board.WhichPlayerCurrent;
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
            for (int col = 0; col < controller.Board.NumCols; col++)
            {
                AIMoveStatus  aiMoveStatus = ComputeAIMoveStatusForCol(
                    controller, 
                    col,
                    ref row,
                    controller.Board.WhichPlayerCurrent,
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
                            controller.Board.SetBoardEntryInfo(col, row, whichPlayerMe, false, false); // let's put ourself there
                            int greatestSpan = controller.Board.ComputeGreatestSpanFromCoord(col, row);
                            controller.Board.SetBoardEntryInfo(col, row, WhichPlayer.NONE, false, false); // restore
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
                        if (TryMove(controller, list[ index2 ]))
                            return true;    // moved!
                    }
                }
            }

            // we shouldn't get here, but if we do, let's just pick something random
            return OnTryToMakeAMove_Random(controller); 
        }

        public bool OnTryToMakeAMove(Controller controller)
        {
            // this is where it's the AI's turn and they
            // need to figure out where to move

            switch (controller.UI.DebugAIPlayStyle)
            {
                case AIPlayStyle.NONE:
                    return OnTryToMakeAMove_None(controller);
                case AIPlayStyle.ALL_LEFT:
                    return OnTryToMakeAMove_AllLeft(controller);
                case AIPlayStyle.ALL_RIGHT:
                    return OnTryToMakeAMove_AllRight(controller);
                case AIPlayStyle.LEFT_THEN_RIGHT:
                    return OnTryToMakeAMove_LeftThenRight(controller);
                case AIPlayStyle.RANDOM:
                    return OnTryToMakeAMove_Random(controller);
                case AIPlayStyle.SOME_SMARTS:
                    return OnTryToMakeAMove_SomeSmarts(controller);
                default:
                    break;
            }
            return OnTryToMakeAMove_Random(controller);
        }
    }
}


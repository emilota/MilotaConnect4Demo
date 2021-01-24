using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class Board // manage our whole game board
    {
        public static BoardDelta[] gDirectionPermutationArray = new BoardDelta[]
        {
            new BoardDelta(0, -1),  // up
            new BoardDelta(1, -1),  // up-right
            new BoardDelta(1, 0),   // right
            new BoardDelta(1, 1)    // down-right
        };

        // this is used to remember what the last game was, so we can
        // correctly choose who starts the new game
        public static WhichPlayer gLastPlayerThatStarted = WhichPlayer.NONE;

        //------------------------------

        public const Controller DEFAULT_CONTROLLER = null;
        public const int DEFAULT_NUM_COLS = 0;
        public const int DEFAULT_NUM_ROWS = 0;
        public const int DEFAULT_NUM_CHECKERS_IN_A_ROW_TO_WIN = 0;

        //------------------------------

        private Controller mController = DEFAULT_CONTROLLER;
        private int mNumCols = DEFAULT_NUM_COLS;
        private int mNumRows = DEFAULT_NUM_ROWS;
        private int mNumCheckersInARowToWin = DEFAULT_NUM_CHECKERS_IN_A_ROW_TO_WIN;

        private UIMetrics mUIMetrics = new UIMetrics();
        private BoardEntry[,] mBoardEntryMatrix = null;
        private CheckerManager mCheckerManager = new CheckerManager();
        private CheckerSelectInfo mCheckerSelectInfo = new CheckerSelectInfo();
        private AI mAI = new AI();
        private List<GameObject> mGOList = new List<GameObject> ();
        private WhichPlayer mWhichPlayerCurrent = WhichPlayer.NONE;
        private WhichPlayer mWhichPlayerWinner = WhichPlayer.NONE;

        //------------------------------

        public Controller Controller => mController;
        public int NumCols => mNumCols;
        public int NumRows => mNumRows;
        public int NumCheckersInARowToWin => mNumCheckersInARowToWin;

        public UIMetrics UIMetrics => mUIMetrics;
        public CheckerManager CheckerManager => mCheckerManager;
        public CheckerSelectInfo CheckerSelectInfo => mCheckerSelectInfo;
        public AI AI => mAI;

        public WhichPlayer WhichPlayerCurrent => mWhichPlayerCurrent;
        public void SetWhichPlayerCurrent(WhichPlayer whichPlayerCurrent) { mWhichPlayerCurrent = whichPlayerCurrent; }
        public void SetWhichPlayerCurrentInitial()
        {
            if (this.Controller.UI.DebugHumanPlayerAlwaysStartsFirst)
            {
                // debugging tool:  Force us to always be first
                SetWhichPlayerCurrent(WhichPlayer.PLAYER_1_HUMAN);
            }
            else
            {
                // normal mode:  pick random if first time, else ping/ping between human and ai starting first
                switch (gLastPlayerThatStarted)
                {
                    case WhichPlayer.NONE:
                    default:
                        {
                            // First time playing!  Do a coin flip to see who starts right now, then we'll alternate for subsequent matches
                            SetWhichPlayerCurrent(Util.GetRandomRange(0, 100) < 50 ? WhichPlayer.PLAYER_1_HUMAN : WhichPlayer.PLAYER_2_AI);
                            break;
                        }
                    case WhichPlayer.PLAYER_1_HUMAN:
                        {
                            // last game player 1 started, let's start this one with player 2
                            SetWhichPlayerCurrent(WhichPlayer.PLAYER_2_AI);
                            break;
                        }
                    case WhichPlayer.PLAYER_2_AI:
                        {
                            // last game player 2 started, let's start this one with player 1
                            SetWhichPlayerCurrent(WhichPlayer.PLAYER_1_HUMAN);
                            break;
                        }
                }
            }

            gLastPlayerThatStarted = this.WhichPlayerCurrent; // remember this choice for next time
        }

        public WhichPlayer WhichPlayerWinner => mWhichPlayerWinner;
        public void SetWhichPlayerWinner(WhichPlayer whichPlayerWinner) { mWhichPlayerWinner = whichPlayerWinner; }

        //------------------------------

        private void PrivateFillAllInternalValues(
            Controller controller = DEFAULT_CONTROLLER,
            int numCols = DEFAULT_NUM_COLS,
            int numRows = DEFAULT_NUM_ROWS,
            int numCheckersInARowToWin = DEFAULT_NUM_CHECKERS_IN_A_ROW_TO_WIN)
        {
            mController = controller;
            mNumCols = numCols;
            mNumRows = numRows;
            mNumCheckersInARowToWin = numCheckersInARowToWin;

            mUIMetrics.ResetValues();
            mBoardEntryMatrix = null;
            mCheckerManager.Init(this);
            mCheckerSelectInfo.ResetValues();
            mAI.Init(this);

            mGOList.Clear();

            mWhichPlayerCurrent = WhichPlayer.NONE;
            mWhichPlayerWinner = WhichPlayer.NONE;
        }

        public Board() { Init(); }

        public Board(
            Controller controller = DEFAULT_CONTROLLER,
            int numCols = DEFAULT_NUM_COLS,
            int numRows = DEFAULT_NUM_ROWS,
            int numCheckersInARowToWin = DEFAULT_NUM_CHECKERS_IN_A_ROW_TO_WIN)
        {
            Init(
                controller,
                numCols,
                numRows,
                numCheckersInARowToWin);
        }

        public void Init(
            Controller controller = DEFAULT_CONTROLLER,
            int numCols = DEFAULT_NUM_COLS,
            int numRows = DEFAULT_NUM_ROWS,
            int numCheckersInARowToWin = DEFAULT_NUM_CHECKERS_IN_A_ROW_TO_WIN)
        {
            Uninit();   // here so we can call init multiple times and it will correctly shutdown before reinitializing

            PrivateFillAllInternalValues(
                controller,
                numCols,
                numRows,
                numCheckersInARowToWin);

            if (IsValidBoard)
                Setup();
        }

        public void Uninit()
        {
            Teardown();

            PrivateFillAllInternalValues();
        }

        public bool IsValidBoard
        {
            get
            {
                if (mController == null)
                    return false;
                if (mNumCols < 2)
                    return false;
                if (mNumRows < 2)
                    return false;
                if (mNumCheckersInARowToWin < 1)
                    return false;
                return true;
            }
        }

        public GameObject InstantiatePrefab(
            string name,
            GameObject goParent,
            GameObject prefab,
            Vector3 coord,
            bool active,
            bool addToList)
        {
            GameObject go = GameObject.Instantiate(
                prefab,
                coord,
                Quaternion.identity);
            go.name = (name ?? "UnnamedGameObject");
            if (goParent != null)
                go.transform.SetParent(goParent.transform);
            go.SetActive(active);
            if (addToList)
                mGOList.Add(go);
            return go;
        }

        public bool Setup()
        {
            Teardown();

            if (!IsValidBoard)
            {
                Debug.LogError("Setup() : Trying to set up game board with invalid settings.  Please recheck.");
                return false;
            }

            // before we can create our game objects, we need to figure out some stuff regarding where they will all go
            mUIMetrics.CalculateValues(this);

            // build our grid
            mBoardEntryMatrix = new BoardEntry[ mNumCols, mNumRows ];
            for (int row = 0; row < mNumRows; row++)
            {
                for (int col = 0; col < mNumCols; col++)
                {
                    mBoardEntryMatrix[ col, row ] = new BoardEntry(this, WhichPlayer.NONE, false);
                }
            }

            mCheckerManager.Init(this);
            mCheckerSelectInfo.ResetValues();
            mAI.Init(this);

            // reset it to default "empty" game board
            Empty();

            return true;
        }

        public void Teardown()
        {
            DestroyGameObjects();

            mUIMetrics.ResetValues();
            mBoardEntryMatrix = null;

            mCheckerManager.Empty();
            mCheckerSelectInfo.ResetValues();
        }

        public bool CreateGameObjects()
        {
            DestroyGameObjects();

            if (!IsValidBoard)
                return false; // not a valid board

            GameObject goRoot = mController.UI.GOBoard;
            GameObject goBoardModel = mController.UI.GOBoardModel;

            GameObject prefabPlayer1Checker = mController.UI.PrefabPlayer1Checker;
            GameObject prefabPlayer2Checker = mController.UI.PrefabPlayer2Checker;

            Vector3 coord = new Vector3(0.0f, 0.0f, 0.0f);    // origin for now

            mCheckerSelectInfo.GOPlayer1Start = InstantiatePrefab(
                "GOPlayer1Start",
                goRoot,
                prefabPlayer1Checker,
                coord,
                false, // inactive
                true);  // add to list

            mCheckerSelectInfo.GOPlayer1End = InstantiatePrefab(
                "GOPlayer1End",
                goRoot,
                prefabPlayer1Checker,
                coord,
                false, // inactive
                true);  // add to list

            if (mBoardEntryMatrix != null)
            {
                for (int row = 0; row < mNumRows; row++)
                {
                    for (int col = 0; col < mNumCols; col++)
                    {
                        BoardEntry boardEntry = mBoardEntryMatrix[ col, row ];

                        coord = mUIMetrics.CalculatePosition(col, row);

                        boardEntry.GOPlayer1Checker = InstantiatePrefab(
                            "Player1Checker(" + col + "," + row + ")",
                            goRoot,
                            prefabPlayer1Checker,
                            coord,
                            false,// inactive
                            true); // yes, add to list

                        boardEntry.GOPlayer2Checker = InstantiatePrefab(
                            "Player2Checker(" + col + "," + row + ")",
                            goRoot,
                            prefabPlayer2Checker,
                            coord,
                            false,// inactive
                            true); // yes, add to list
                    }
                }
            }
            UpdateCheckerShowHide();

            return true;
        }

        public void DestroyGameObjects()
        {
            mCheckerManager.Empty();

            while (mGOList.Count > 0)
            {
                GameObject gameObject = mGOList[mGOList.Count - 1];
                GameObject.Destroy(gameObject);
                mGOList.RemoveAt(mGOList.Count - 1);
            }

            mCheckerSelectInfo.ResetGORefs();

            if (mBoardEntryMatrix != null)
            {
                for (int row = 0; row < mNumRows; row++)
                {
                    for (int col = 0; col < mNumCols; col++)
                    {
                        BoardEntry boardEntry = mBoardEntryMatrix[col,row];
                        boardEntry.ResetGORefs();
                    }
                }
            }
        }

        public void UpdateCheckerShowHide()
        {
            if (mBoardEntryMatrix != null)
            {
                for (int row = 0; row < mNumRows; row++)
                {
                    for (int col = 0; col < mNumCols; col++)
                    {
                        BoardEntry boardEntry = mBoardEntryMatrix[ col, row ];
                        boardEntry.UpdateCheckerShowHide();
                    }
                }
            }
        }

        public bool IsNextRowEmpty(int col, int row)
        {
            row++;
            if (row >= mNumRows)
                return false;
            if (row < 0)
                return false;
            if (col < 0)
                return false;
            if (col >= mNumCols)
                return false;
            if (mBoardEntryMatrix[ col, row ].WhichPlayerChecker != WhichPlayer.NONE)
                return false;
            return true;
        }

        public bool CanDropOnCol(int col, ref int row)
        {
            row = -1;
            BoardEntry boardEntry = FindBoardEntry(col, 0);
            if (boardEntry == null)
                return false;
            if (boardEntry.WhichPlayerChecker != WhichPlayer.NONE)
                return false;

            // yes, we can drop!  Let's figure out what row we'll land on
            row = 0;
            while (IsNextRowEmpty(col, row))
            {
                row++;
            }

            return true;
        }

        public BoardEntry FindBoardEntry(int col, int row)
        {
            // these are on seperate lines so we can put breakpoints on each one if needed
            if (mBoardEntryMatrix == null)
                return null;
            if (col < 0)
                return null;
            if (col >= mNumCols)
                return null;
            if (row < 0)
                return null;
            if (row >= mNumRows)
                return null;
            return mBoardEntryMatrix[ col, row ];
        }

        public bool FindColumnFromPlaneCoordX(float worldX, ref int col, ref int row)
        {
            col = -1;
            row = -1;

            worldX += (mUIMetrics.BoardWidth * 0.5f);
            if (worldX < 0.0f)
                return false;
            else if (worldX > mUIMetrics.BoardWidth)
                return false;
            col = (int) (worldX / mUIMetrics.CellWidth);
            if (col < 0)
                return false;
            if (col >= mNumCols)
                return false;
            int tempRow = 0;
            if (!CanDropOnCol(col, ref tempRow))
                return false;
            row = tempRow;
            return true;
        }

        public void FillBoard(WhichPlayer whichPlayerChecker)
        {
            if (mBoardEntryMatrix != null)
            {
                for (int row = 0; row < mNumRows; row++)
                {
                    for (int col = 0; col < mNumCols; col++)
                    {
                        mBoardEntryMatrix[ col, row ].WhichPlayerChecker = whichPlayerChecker;
                    }
                }
            }

            UpdateCheckerShowHide();
        }

        public void Empty()
        {
            FillBoard(WhichPlayer.NONE);
        }

        public void ShowCheckerSelect(int col, int row)
        {
            mCheckerSelectInfo.Showing = true;
            mCheckerSelectInfo.Col = col;
            mCheckerSelectInfo.Row = row;
            mCheckerSelectInfo.GOPlayer1Start.transform.position = mUIMetrics.CalculatePosition(col, -1);
            mCheckerSelectInfo.GOPlayer1End.transform.position = mUIMetrics.CalculatePosition(col, row);
            UpdateCheckerSelectShowHide();
        }

        public void HideCheckerSelect()
        {
            mCheckerSelectInfo.Showing = false;
            mCheckerSelectInfo.Col = Const.INVALID_COL_VALUE;
            mCheckerSelectInfo.Row = Const.INVALID_ROW_VALUE;
            UpdateCheckerSelectShowHide();
        }

        public void UpdateCheckerSelectShowHide()
        {
            if (mCheckerSelectInfo.Showing)
            {
                mCheckerSelectInfo.GOPlayer1Start.SetActive(true);
                mCheckerSelectInfo.GOPlayer1End.SetActive(Util.Blink(this.Controller.UI.SelectCheckerBlinkMS));
            }
            else
            {
                mCheckerSelectInfo.GOPlayer1Start.SetActive(false);
                mCheckerSelectInfo.GOPlayer1End.SetActive(false);
            }
        }

        public bool IsCheckerSelectShowing => mCheckerSelectInfo.Showing;
        public int CheckerSelectCol => mCheckerSelectInfo.Col;
        public int CheckerSelectRow => mCheckerSelectInfo.Row;

        public bool TryDropChecker(WhichPlayer whichPlayer, int col, int row)
        {
            // sanity checks
            int row2 = -1;
            if (!CanDropOnCol(col, ref row2))
                return false;
            if (row != row2)
                return false;

            // show this guy!
            SpawnCheckerMove(whichPlayer, col, row);
            HideCheckerSelect();

            return true;
        }

        public bool SpawnCheckerMove(WhichPlayer whichPlayer, int col, int row)
        {
            // sanity check
            switch (whichPlayer)
            {
                case WhichPlayer.NONE:
                    break;
                case WhichPlayer.PLAYER_1_HUMAN:
                    break;
                case WhichPlayer.PLAYER_2_AI:
                    break;
                default:
                    return false;
            }
            int startCol = col;
            int startRow = -1;
            int targetCol = col;
            int targetRow = row;
            Checker checker = mCheckerManager.SpawnChecker(
                "PlayerMove " + Convert.ToString(whichPlayer),
                whichPlayer,
                startCol, startRow,
                targetCol, targetRow,
                (checker2) =>
                {
                    // hit target!
                    checker2.CheckerManager.Board.SetBoardEntryInfo(
                        checker2.TargetCol,
                        checker2.TargetRow,
                        checker2.WhichPlayerChecker,
                        false, // no blink
                        true); // update UI
                });
            return true;
        }

        public bool SetBoardEntryInfo(
            int col,
            int row,
            WhichPlayer whichPlayerChecker,
            bool blink,
            bool updateUI)
        {
            BoardEntry boardEntry = FindBoardEntry(col, row);
            if (boardEntry == null)
                return false;
            boardEntry.WhichPlayerChecker = whichPlayerChecker;
            boardEntry.Blink = blink;
            if (updateUI)
            {
                boardEntry.UpdateCheckerShowHide();
            }
            return true;
        }

        public void RemoveAllMovingCheckers()
        {
            mCheckerManager.Empty();
        }

        private bool PrivateCheckForWinFrom(
            int col,
            int row,
            int deltaCol,
            int deltaRow,
            int numCheckersInARowToWin,
            WhichPlayer whichPlayerCheckerToMatch,
            ref List<BoardCoord> boardCoordListRef)
        {
            col += deltaCol;
            row += deltaRow;
            BoardEntry boardEntry = FindBoardEntry(col, row);
            if (boardEntry == null)
                return false; // off grid
            WhichPlayer whichPlayerChecker = boardEntry.WhichPlayerChecker;
            if (whichPlayerChecker != whichPlayerCheckerToMatch)
                return false;
            numCheckersInARowToWin--;
            if (numCheckersInARowToWin > 0)
            {
                List<BoardCoord> boardCoordListTemp = null;

                bool returnValue = PrivateCheckForWinFrom(
                    col,
                    row,
                    deltaCol,
                    deltaRow,
                    numCheckersInARowToWin,
                    whichPlayerCheckerToMatch,
                    ref boardCoordListTemp);
                if (returnValue == true)
                {
                    boardCoordListRef = boardCoordListTemp;
                    boardCoordListRef.Add(new BoardCoord(col, row));
                }
                return returnValue;
            }

            // let's record which tiles created this win
            boardCoordListRef = new List<BoardCoord>();
            boardCoordListRef.Add(new BoardCoord(col, row));

            // match!
            return true;
        }

        public bool PrivateCheckForWinFrom(
            int col,
            int row,
            int numCheckersInARowToWin,
            WhichPlayer whichPlayerCheckerToMatch,
            ref List<BoardCoord> boardCoordListRef)
        {
            for (int index = 0; index < gDirectionPermutationArray.Length; index++)
            {
                BoardDelta boardDelta = gDirectionPermutationArray[index];

                List<BoardCoord> boardCoordList = null;
                if (PrivateCheckForWinFrom(
                    col,
                    row,
                    gDirectionPermutationArray[ index ].DeltaX,
                    gDirectionPermutationArray[ index ].DeltaY,
                    mNumCheckersInARowToWin - 1,
                    whichPlayerCheckerToMatch,
                    ref boardCoordList))
                {
                    boardCoordListRef = boardCoordList;
                    return true; // we've got a win!!!
                }
            }

            return false;   // nope
        }

        public bool CheckForWin(ref WhichPlayer whichPlayerWinnerRef, ref List<BoardCoord> boardCoordListRef)
        {
            whichPlayerWinnerRef = WhichPlayer.NONE; // for now
            boardCoordListRef = null; // for now

            if (mBoardEntryMatrix == null)
                return false;

            for (int row = 0; row < mNumRows; row++)
            {
                for (int col = 0; col < mNumCols; col++)
                {
                    WhichPlayer whichPlayerCheckerToMatch = mBoardEntryMatrix[col,row].WhichPlayerChecker;
                    if (whichPlayerCheckerToMatch != WhichPlayer.NONE)
                    {
                        List<BoardCoord> boardCoordList = null;
                        if (PrivateCheckForWinFrom(
                            col,
                            row,
                            mNumCheckersInARowToWin - 1,
                            whichPlayerCheckerToMatch,
                            ref boardCoordList))
                        {
                            whichPlayerWinnerRef = whichPlayerCheckerToMatch;
                            boardCoordList.Add(new BoardCoord(col, row)); // add this piece
                            boardCoordListRef = boardCoordList;
                            return true; // we've got a win!!!
                        }
                    }
                }
            }

            return false;
        }

        private int CalculateSpan(int col, int row, WhichPlayer whichPlayer, BoardDelta boardDelta, int iter)
        {
            if (iter < 0)
                return 0;
            col += boardDelta.DeltaX;
            row += boardDelta.DeltaY;
            BoardEntry boardEntry = FindBoardEntry(col, row);
            if (boardEntry == null)
                return 0;
            if (boardEntry.WhichPlayerChecker != whichPlayer)
                return 0;
            // we match!
            int span = CalculateSpan(col, row, whichPlayer, boardDelta, iter - 1);
            span++;
            return span;
        }

        public int ComputeGreatestSpanFromCoord(int col, int row)
        {
            BoardEntry boardEntry = FindBoardEntry(col, row);
            if (boardEntry == null)
                return 0;

            WhichPlayer whichPlayer = boardEntry.WhichPlayerChecker;
            int maxSpan = 0;
            for(int index = 0; index < gDirectionPermutationArray.Length; index++)
            {
                BoardDelta boardDelta = gDirectionPermutationArray[index];
                BoardDelta boardDeltaRev = boardDelta;
                boardDeltaRev.Negate();

                int span = 
                    CalculateSpan(col,row,whichPlayer,boardDelta,mNumCheckersInARowToWin-1) + 
                    CalculateSpan(col,row,whichPlayer,boardDeltaRev,mNumCheckersInARowToWin-1);
                if (maxSpan < span)
                    maxSpan = span;
            }
            return maxSpan;
        }

        public bool CheckForFullBoard()
        {
            // we're full if our top row is full

            if (mBoardEntryMatrix == null)
                return false;

            for (int col = 0; col < mNumCols; col++)
            {
                WhichPlayer whichPlayer = mBoardEntryMatrix[col,0].WhichPlayerChecker;
                if (whichPlayer == WhichPlayer.NONE)
                    return false;
                // this spot has a player checker on it
            }

            return true;    // full!
        }

        public void ConvertAllStaticCheckersIntoMovingCheckers()
        {
            if (mBoardEntryMatrix == null)
                return;

            for (int row = 0; row < mNumRows; row++)
            {
                for (int col = 0; col < mNumCols; col++)
                {
                    BoardEntry boardEntry = mBoardEntryMatrix[ col, row ];
                    if ((boardEntry.WhichPlayerChecker == WhichPlayer.PLAYER_1_HUMAN) ||
                        (boardEntry.WhichPlayerChecker == WhichPlayer.PLAYER_2_AI))
                    {
                        Checker checker = mCheckerManager.SpawnChecker(
                            "FallingChecker(" + col + "," + row + ")",
                            boardEntry.WhichPlayerChecker,
                            col,
                            row,
                            col,
                            mNumRows,
                            null); // don't need to do anything when we hit the target
                    }

                    SetBoardEntryInfo(
                        col, 
                        row, 
                        WhichPlayer.NONE, 
                        false, 
                        true);
                }
            }

        }

        public string GetDebugBoardInfo()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (mBoardEntryMatrix == null)
            {
                stringBuilder.Append("null");
            }
            else
            {
                for (int row = 0; row < mNumRows; row++)
                {
                    if (row > 0)
                        stringBuilder.Append(",");
                    stringBuilder.Append("[");
                    for (int col = 0; col < mNumCols; col++)
                    {
                        if (col > 0)
                            stringBuilder.Append(",");
                        WhichPlayer whichPlayerChecker = mBoardEntryMatrix[col,row].WhichPlayerChecker;
                        switch(whichPlayerChecker)
                        {
                            case WhichPlayer.NONE:
                                {
                                    stringBuilder.Append(" ");
                                    break;
                                }
                            case WhichPlayer.PLAYER_1_HUMAN:
                                {
                                    stringBuilder.Append("1");
                                    break;
                                }
                            case WhichPlayer.PLAYER_2_AI:
                                {
                                    stringBuilder.Append("2");
                                    break;
                                }
                            default:
                                {
                                    stringBuilder.Append("?");
                                    break;
                                }
                        }
                    }
                    stringBuilder.Append("]");
                }
            }
            return stringBuilder.ToString();
        }

        public void AdvanceToNextPlayersTurn()
        {
            switch (this.WhichPlayerCurrent)
            {
                case WhichPlayer.NONE:
                    {
                        SetWhichPlayerCurrent(WhichPlayer.NONE);
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        SetWhichPlayerCurrent(WhichPlayer.PLAYER_2_AI);
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        SetWhichPlayerCurrent(WhichPlayer.PLAYER_1_HUMAN);
                        break;
                    }
                default:
                    {
                        SetWhichPlayerCurrent(WhichPlayer.NONE);
                        break;
                    }
            }
        }

        public void StartGame()
        {
            SetWhichPlayerCurrentInitial();
            SetWhichPlayerWinner(WhichPlayer.NONE);
            Empty();

            this.AI.OnGameStart(this.Controller);
        }

        public void OnBoardFixedUpdate()
        {
            mCheckerManager.OnCheckerManagerFixedUpdate();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class Checker
    {
        public const string DEFAULT_NAME = "nonamechecker";

        public CheckerManager CheckerManager = null;
        public string Name = DEFAULT_NAME;
        public WhichPlayer WhichPlayerChecker = WhichPlayer.NONE;
        public int StartCol = Const.INVALID_COL_VALUE;
        public int StartRow = Const.INVALID_ROW_VALUE;
        public int TargetCol = Const.INVALID_COL_VALUE;
        public int TargetRow = Const.INVALID_ROW_VALUE;
        public Action<Checker> OnHitTarget = null;
        public Vector3 PosCurrent = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 PosTarget = new Vector3(0.0f, 0.0f, 0.0f);
        public GameObject GOChecker = null;
        public bool AllDone = false;

        public Checker(
            CheckerManager checkerManager,
            string name,
            WhichPlayer whichPlayerChecker,
            int startCol,
            int startRow,
            int targetCol,
            int targetRow,
            Action<Checker> onHitTarget)
        {
            Init(checkerManager,
                name,
                whichPlayerChecker,
                startCol,
                startRow,
                targetCol,
                targetRow,
                onHitTarget);
        }

        public void Init(
            CheckerManager checkerManager,
            string name,
            WhichPlayer whichPlayerChecker,
            int startCol,
            int startRow,
            int targetCol,
            int targetRow,
            Action<Checker> onHitTarget)
        {
            Uninit();

            this.CheckerManager = checkerManager;
            this.Name = (name ?? DEFAULT_NAME);
            this.WhichPlayerChecker = whichPlayerChecker;
            this.StartCol = startCol;
            this.StartRow = startRow;
            this.TargetCol = targetCol;
            this.TargetRow = targetRow;
            this.OnHitTarget = onHitTarget;
            this.PosCurrent = this.CheckerManager.Board.UIMetrics.CalculatePosition(this.StartCol, this.StartRow);
            this.PosTarget = this.CheckerManager.Board.UIMetrics.CalculatePosition(this.TargetCol, this.TargetRow);

            GameObject goRoot = this.CheckerManager.Board.Controller.UI.GOBoard;
            GameObject prefab = this.CheckerManager.Board.Controller.UI.PrefabQuestionMark;
            switch (this.WhichPlayerChecker)
            {
                case WhichPlayer.NONE:
                    {
                        break;
                    }
                case WhichPlayer.PLAYER_1_HUMAN:
                    {
                        prefab = this.CheckerManager.Board.Controller.UI.PrefabPlayer1Checker;
                        break;
                    }
                case WhichPlayer.PLAYER_2_AI:
                    {
                        prefab = this.CheckerManager.Board.Controller.UI.PrefabPlayer2Checker;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            this.GOChecker = this.CheckerManager.Board.InstantiatePrefab(
                this.Name,
                goRoot,
                prefab,
                this.PosCurrent,
                true, // active!
                false); // don't add to list

            this.AllDone = false;

            if (this.CheckerManager != null)
            {
                this.CheckerManager.AddChecker(this);
            }
        }

        public void Uninit()
        {
            if (this.CheckerManager != null)
            {
                this.CheckerManager.RemoveChecker(this);
            }

            if (this.GOChecker != null)
            {
                GameObject.DestroyImmediate(this.GOChecker, false);
                this.GOChecker = null;
            }
        }

        public void OnCheckerFixedUpdate()
        {
            if ((this.CheckerManager != null) && (!this.AllDone))
            {
                float distance = Vector3.Distance(
                        this.PosCurrent,
                        this.PosTarget);
                Board board = this.CheckerManager.Board;
                float delta = (
                        board.UIMetrics.DY.magnitude *
                        board.Controller.UI.CheckerDropScale);
                if (distance <= delta)
                {
                    // hit target!
                    this.PosCurrent = this.PosTarget;
                    this.AllDone = true;

                    if (this.GOChecker != null)
                        this.GOChecker.transform.position = this.PosCurrent;

                    this.OnHitTarget?.Invoke(this);
                }
                else
                {
                    // not at target yet...let's keep moving
                    this.PosCurrent += (board.UIMetrics.DY * board.Controller.UI.CheckerDropScale);
                    if (this.GOChecker != null)
                        this.GOChecker.transform.position = this.PosCurrent;
                }
            }
        }
    }
}


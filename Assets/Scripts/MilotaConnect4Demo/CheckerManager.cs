// Created and programmed by Eric Milota, 2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilotaConnect4Demo
{
    public class CheckerManager
    {
        private Board mBoard = null;
        private List<Checker> mCheckerList = new List<Checker>();

        public Board Board => mBoard;

        public CheckerManager(Board board = null) { Init(board); }

        public void Init(Board board = null)
        {
            mBoard = board;
            Empty();
        }

        public void Uninit()
        {
            Empty();
            mBoard = null;
        }

        public void Empty()
        {
            while (mCheckerList.Count > 0)
            {
                Checker checker = mCheckerList[mCheckerList.Count - 1];
                checker.Uninit();
            }
        }

        public int NumActiveCheckers => mCheckerList.Count;

        public void AddChecker(Checker checker)
        {
            if (checker == null)
                return;
            if (!mCheckerList.Contains(checker))
                mCheckerList.Add(checker);
        }

        public void RemoveChecker(Checker checker)
        {
            if (checker == null)
                return;
            if (mCheckerList.Contains(checker))
                mCheckerList.Remove(checker);
        }

        public Checker SpawnChecker(
            string name,
            WhichPlayer whichPlayerChecker,
            int startCol,
            int startRow,
            int targetCol,
            int targetRow,
            Action<Checker> onHitTarget)
        {
            Checker checker = new Checker(
                    this,
                    name,
                    whichPlayerChecker,
                    startCol,
                    startRow,
                    targetCol,
                    targetRow,
                    onHitTarget);
            return checker;
        }

        public void OnCheckerManagerFixedUpdate()
        {
            int index = 0;
            for (; ; )
            {
                if (index >= mCheckerList.Count)
                    break;

                Checker checker = mCheckerList[index];
                checker.OnCheckerFixedUpdate();
                if (checker.AllDone)
                {
                    checker.Uninit();
                }
                else
                {
                    index++;
                }
            }
        }
    }
}


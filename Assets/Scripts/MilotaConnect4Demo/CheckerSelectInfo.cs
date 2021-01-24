using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class CheckerSelectInfo
    {
        public bool Showing = false;
        public int Col = Const.INVALID_COL_VALUE;
        public int Row = Const.INVALID_ROW_VALUE;
        public GameObject GOPlayer1Start = null;
        public GameObject GOPlayer1End = null;

        public CheckerSelectInfo() { ResetValues(); }

        public void ResetValues()
        {
            this.Showing = false;
            this.Col = Const.INVALID_COL_VALUE;
            this.Row = Const.INVALID_ROW_VALUE;
            ResetGORefs();
        }

        public void ResetGORefs()
        {
            this.GOPlayer1Start = null;
            this.GOPlayer1End = null;
        }
    }
}


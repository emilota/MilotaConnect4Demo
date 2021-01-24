using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public class UIMetrics // used to keep board UI metric info
    {
        public Vector3 UL = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 DX = new Vector3(0.0f, 0.0f, 0.0f);
        public Vector3 DY = new Vector3(0.0f, 0.0f, 0.0f);
        public float CellWidth = 0.0f;
        public float CellHeight = 0.0f;
        public float BoardWidth = 0.0f;
        public float BoardHeight = 0.0f;

        public UIMetrics() { ResetValues(); }

        public void ResetValues()
        {
            this.UL = new Vector3(0.0f, 0.0f, 0.0f);
            this.DX = new Vector3(0.0f, 0.0f, 0.0f);
            this.DY = new Vector3(0.0f, 0.0f, 0.0f);
            this.CellWidth = 0.0f;
            this.CellHeight = 0.0f;
            this.BoardWidth = 0.0f;
            this.BoardHeight = 0.0f;
        }

        public void CalculateValues(Board board)
        {
            GameObject GO_AnchorUL = board.Controller.UI.GOAnchorUL;
            GameObject GO_AnchorUR = board.Controller.UI.GOAnchorUR;
            GameObject GO_AnchorLL = board.Controller.UI.GOAnchorLL;

            ResetValues();

            this.UL = GO_AnchorUL.transform.position;
            this.DX = (GO_AnchorUR.transform.position - this.UL) / (board.NumCols - 1); // measuring span between first and last item, so must subtract 1
            this.DY = (GO_AnchorLL.transform.position - this.UL) / (board.NumRows - 1); // measuring span between first and last item, so must subtract 1
            this.CellWidth = Vector3.Distance(this.UL, this.UL + this.DX);
            this.CellHeight = Vector3.Distance(this.UL, this.UL + this.DY);
            this.BoardWidth = Vector3.Distance(this.UL, this.UL + (this.DX * board.NumCols));
            this.BoardHeight = Vector3.Distance(this.UL, this.UL + (this.DY * board.NumCols));
        }

        public Vector3 CalculatePosition(int col, int row)
        {
            return UL + (this.DX * col) + (this.DY * row);
        }
    }
}


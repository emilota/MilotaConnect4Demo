using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public struct BoardCoord // used when reporting back which checkers were the "4" when winning or losing
    {
        public int Col;
        public int Row;

        public BoardCoord(int col, int row)
        {
            this.Col = col;
            this.Row = row;
        }
    }
}


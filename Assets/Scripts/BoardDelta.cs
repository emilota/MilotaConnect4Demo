// Created and programmed by Eric Milota, 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MilotaConnect4Demo
{
    public struct BoardDelta // used to determine which direction we are looking for connects
    {
        public int DeltaX;
        public int DeltaY;

        public BoardDelta(int deltaX, int DeltaY)
        {
            this.DeltaX = deltaX;
            this.DeltaY = DeltaY;
        }

        public void Negate()
        {
            this.DeltaX = -this.DeltaX;
            this.DeltaY = -this.DeltaY;
        }
    }
}


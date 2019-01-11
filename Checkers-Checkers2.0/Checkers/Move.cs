using System;
using System.Collections.Generic;

namespace Checkers
{

    public class Move
    {

        public readonly int XStart, YStart, XEnd, YEnd;

        public Move(int xStart, int yStart, int xEnd, int yEnd)
        {
            XStart = xStart;
            XEnd = xEnd;
            YStart = yStart;
            YEnd = yEnd;
        }

        public bool IsBasicMove
        {
            get
            {
                bool isXSingle = Math.Abs(XEnd - XStart) == 1;
                bool isYSingle = Math.Abs(YEnd - YStart) == 1;
                return isXSingle && isYSingle;
            }
        }
   
     



    }
}
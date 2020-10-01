using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TjuvOchPolis
{
    class Police : Person
    {
        public Police(int vx, int vy, int x, int y)
        {
            VelX = vx;
            VelY = vy;
            X = x;
            Y = y;
        }
    }
}

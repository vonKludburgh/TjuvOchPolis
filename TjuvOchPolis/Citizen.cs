using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TjuvOchPolis;

namespace TjuvOchPolis
{
    class Citizen : Person
    {
        public Citizen(int vx, int vy, int x, int y)
        {
            VelX = vx;
            VelY = vy;
            X = x;
            Y = y;

            Belongings.Watch = 1;
            Belongings.Wallet = 1;
            Belongings.Phone = 1;
            Belongings.Keys = 1;
        }
    }
}

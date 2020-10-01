using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TjuvOchPolis
{
    class Person
    {
        public int VelX { get; set; }
        public int VelY { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Prison { get; set; }
        public int PrisonTime { get; set; }
        public Inventory Belongings = new Inventory();
        public Inventory StolenGoods = new Inventory();
        public Inventory Confiscated = new Inventory();
    }

}

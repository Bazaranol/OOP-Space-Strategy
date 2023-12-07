using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class Cell
    {
        public Point Coords { get; }
        public int Size { get; }
        public List<MapObject> ContainingObjects { set; get; }
        public Cell(Point coords, int size)
        {
            Coords = coords;
            Size = size;
            ContainingObjects = new List<MapObject>();
        }
    }
}

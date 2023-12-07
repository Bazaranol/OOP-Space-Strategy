using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class SpaceStation : StaticObject
    {
        public List<Module> ConnectedModules = new List<Module>();

        public SpaceStation(Cell containigCell) : base(containigCell)
        {
            ContainingCell.ContainingObjects.Remove(this);
            Radius = 1;
        }

        protected override void _spawn(Point coords)
        {
            //тут все просто
            ContainingCell.ContainingObjects.Add(this);
            _connectModules(coords.Y, coords.X);
        }

        private void _connectModules(int i, int j)
        {
            //стыкуется каждый модуль с шансом 75%
            if (MapControll.Random.Next(1, 101) < 76 && j > 0)
            {
                ConnectedModules.Add(new OilModule(MapControll.Cells[i, j - 1], this));
            }

            if (MapControll.Random.Next(1, 101) < 76 && i > 0)
            {
                ConnectedModules.Add(new UraniumModule(MapControll.Cells[i - 1, j], this));
            }

            if (MapControll.Random.Next(1, 101) < 76 && j < MapControll.Size - 1)
            {
                ConnectedModules.Add(new EnginesModule(MapControll.Cells[i, j + 1], this));
            }

            if (MapControll.Random.Next(1, 101) < 76 && i < MapControll.Size - 1)
            {
                ConnectedModules.Add(new RobotsModule(MapControll.Cells[i + 1, j], this));
            }
        }//done

        public override Bitmap getImage()
        {
            return sprites.spaceStation;
        }
    }
}

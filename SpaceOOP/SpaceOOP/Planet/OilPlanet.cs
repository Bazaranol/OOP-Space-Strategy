using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class OilPlanet : Planet
    {
        public OilPlanet(Cell containingCell) : base(containingCell) { }

        protected override void _spawnResource(Cell cell, int probality)
        {

            if (MapControll.Random.Next(1, 1000) < probality)
            {
                var oil = new Oil(cell, this);
                MapControll.ChangedCells.Add(cell);
            }
        }

        public override Bitmap getImage()
        {
            return sprites.oilPlanet;
        }
    }
}

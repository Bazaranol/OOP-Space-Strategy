using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    internal class ResourcefulPlanet: Planet
    {
        public ResourcefulPlanet(Cell containingCell) : base(containingCell) { }

        protected override void _spawnResource(Cell cell, int probability)
        {

            if (MapControll.Random.Next(1, 1000) < probability)
            {
                if (MapControll.Random.Next(1, 3) == 1)
                {
                    var oil = new Oil(cell, this);
                }
                else
                {
                    var uranium = new Uranium(cell, this);
                }

                MapControll.ChangedCells.Add(cell);
            }
        }

        public override Bitmap getImage()
        {
            return sprites.resourcefulPlanet;
        }
    }
}

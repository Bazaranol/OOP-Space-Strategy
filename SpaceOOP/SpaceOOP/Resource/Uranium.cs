using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class Uranium: Resource
    {
        public Uranium(Cell containigCell, Planet containingPlanet) : base(containigCell, containingPlanet)
        {
            ContainingFuel = MapControll.Random.Next(40, 121);
        }

        public override Bitmap getImage()
        {
            return sprites.uranium;
        }
    }
}

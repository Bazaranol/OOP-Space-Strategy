using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class Oil : Resource
    {
        public Oil(Cell containigCell, Planet containingPlanet) : base(containigCell, containingPlanet)
        {
            ContainingFuel = MapControll.Random.Next(20, 71);
        }

        public override Bitmap getImage()
        {
            return sprites.oil;
        }
    }
}

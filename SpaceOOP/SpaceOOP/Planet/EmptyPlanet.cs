using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    internal class EmptyPlanet: Planet
    {
        private SolidBrush _bgBrush = new SolidBrush(Color.Black);
        public EmptyPlanet(Cell containingCell) : base(containingCell) { }

        protected override void _spawnResource(Cell cell, int probalitity) { }

        public override Bitmap getImage()
        {
            return sprites.emptyPlanet;
        }
    }
}

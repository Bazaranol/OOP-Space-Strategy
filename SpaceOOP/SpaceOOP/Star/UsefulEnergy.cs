using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class UsefulEnergy: StarEnergy
    {
        public UsefulEnergy(Cell containigCell) : base(containigCell) { }

        public override Bitmap getImage()
        {
            return sprites.usefulEnergy; 
        }
    }
}

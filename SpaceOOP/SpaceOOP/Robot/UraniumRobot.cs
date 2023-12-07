using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class UraniumRobot: Robot
    {
        public UraniumRobot(Cell containingCell, SpaceShip ownerShip) : base(containingCell, ownerShip)
        {
            StorageCapacity = 180;
        }

        public override Bitmap getImage()
        {
            return sprites.uraniumRobot;
        }
    }
}

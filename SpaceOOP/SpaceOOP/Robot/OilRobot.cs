
using System.Drawing;


namespace SpaceOOP
{
    public class OilRobot: Robot
    {
        public OilRobot(Cell containingCell, SpaceShip ownerShip) : base(containingCell, ownerShip)
        {
            StorageCapacity = 100;
        }

        public override Bitmap getImage()
        {
            return sprites.oilRobot;
        }
    }
}

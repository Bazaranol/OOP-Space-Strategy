using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class UraniumRobotItem : RobotItem
    {
        public UraniumRobotItem(MapObject containingObject) : base(containingObject)
        {
            Cost = 400;
        }
    }
}

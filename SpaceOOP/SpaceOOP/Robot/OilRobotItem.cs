using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class OilRobotItem: RobotItem
    {
        public OilRobotItem(MapObject containingObject) : base(containingObject)
        {
            Cost = 300;
        }
    }
}

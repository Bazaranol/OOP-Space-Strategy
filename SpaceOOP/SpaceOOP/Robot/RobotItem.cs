using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class RobotItem: Item
    {
        public RobotItem(MapObject containingObject) : base(containingObject) { }
    }
}

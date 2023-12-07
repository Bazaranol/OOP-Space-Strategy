using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class Item
    {
        public double Cost { get; set; }
        public MapObject ContainingObject { get; protected set; }

        public Item(MapObject containingObject)
        {
            ContainingObject = containingObject;
        }
    }
}

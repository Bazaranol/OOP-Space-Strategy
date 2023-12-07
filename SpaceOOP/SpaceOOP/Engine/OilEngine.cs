using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class OilEngine : Engine
    {
        public OilEngine(MapObject containingObject) : base(containingObject)
        {
            
            MaxCapacity = Condition / 2 + 50;
            if (ContainingObject is RegularShip)
            {
                Capacity = MaxCapacity;
                Cost = ((Capacity / 50) + (Condition / 50) + 1) * 50;
            }
            else
            {
                Capacity = 0;
                Cost = Condition * 2.2;
            }
        }

        protected override void _countAdditionalParametres()
        {
            MaxCapacity = Condition / 2 + 50;
            Cost = ((Capacity / 50) + (Condition / 50) + 1) * 50;
        }
    }
}

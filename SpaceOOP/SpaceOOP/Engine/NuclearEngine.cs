using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class NuclearEngine: Engine
    {
        public NuclearEngine(MapObject containingObject) : base(containingObject)
        {
            MaxCapacity = Condition + 50;
            if (ContainingObject is RegularShip || ContainingObject is NuclearShip)
            {
                Capacity = MaxCapacity;
                Cost = ((Capacity / 50) + (Condition / 50) + 1) * 60;
            }
            else
            {
                Capacity = 0;
                Cost = Condition * 3;
            }
        }

        protected override void _countAdditionalParametres()
        {
            MaxCapacity = Condition + 50;
            Cost = ((Capacity / 50) + (Condition / 50) + 1) * 70;
        }
    }
}

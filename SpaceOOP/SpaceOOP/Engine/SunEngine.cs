using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class SunEngine : Engine
    {
        public SunEngine(MapObject containingObject) : base(containingObject)
        {
           
            MaxCapacity = Condition / 5 + 30;
            if (ContainingObject is RegularShip || ContainingObject is SunShip)
            {
                Capacity = MaxCapacity;
                Cost = ((Capacity / 50) + (Condition / 50) + 1) * 40;
            }
            else
            {
                Capacity = 0;
                Cost = Condition * 2;
            }
        }

        protected override void _countAdditionalParametres()
        {
            MaxCapacity = Condition / 5 + 30;
            Cost = ((Capacity / 50) + (Condition / 50) + 1) * 40;
          
            foreach (var contObj in ContainingObject.ContainingCell.ContainingObjects)
            {
                if (contObj is UsefulEnergy && MaxCapacity - Capacity > 1)
                {
                    Capacity += 5;
                    break;
                }
            }
        }
    }
}

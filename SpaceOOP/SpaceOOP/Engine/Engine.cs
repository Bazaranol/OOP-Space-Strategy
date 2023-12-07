using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class Engine: Item
    {
        public double Condition { get; set; }
        public double Capacity { get; set; }
        public double MaxCapacity { get; set; }

        public Engine(MapObject containingObject) : base(containingObject)
        {
            Condition = MapControll.Random.Next(80, 101);
        }

        public void LiveOneStep(bool isMoving)
        {
            if (isMoving)
            {
                Condition -= 0.1;
                Capacity -= 0.3;
            }
            _countAdditionalParametres();
        }

        protected abstract void _countAdditionalParametres();
    }
}

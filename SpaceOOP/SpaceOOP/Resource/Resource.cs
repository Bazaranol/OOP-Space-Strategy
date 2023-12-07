using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class Resource : StaticObject
    {
        public int ContainingFuel { get; set; }
        private Planet _containingPlanet;

        public Resource(Cell containigCell, Planet containingPlanet) : base(containigCell)
        {
            _containingPlanet = containingPlanet;
            _containingPlanet.ContainingResources.Add(this);
        }

        public bool CheckIsAlive()
        {
            
            if (ContainingFuel < 1)
            {
                _containingPlanet.ContainingResources.Remove(this);
                ContainingCell.ContainingObjects.Remove(this);

                MapControll.ChangedCells.Add(ContainingCell);

                return false;
            }

            return true;
        }
    }
}

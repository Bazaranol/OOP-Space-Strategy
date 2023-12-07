using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class Module: StaticObject
    {
        private SpaceStation _connectedStation;

        public Module(Cell containigCell, SpaceStation connectedStation) : base(containigCell)
        {
            _connectedStation = connectedStation;
        }

        protected virtual void _generateContainingResources() { }
    }
}

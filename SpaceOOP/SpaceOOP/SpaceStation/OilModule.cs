using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class OilModule: Module
    {
        public OilModule(Cell containigCell, SpaceStation connectedStation) : base(containigCell, connectedStation) { }

        public override Bitmap getImage()
        {
            return sprites.oilModule;
        }
    }
}

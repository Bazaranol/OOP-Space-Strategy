using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class EnginesModule : Module
    {
        public List<Engine> Engines { get; protected set; }
        public EnginesModule(Cell containigCell, SpaceStation connectedStation) : base(containigCell, connectedStation)
        {
            Engines = new List<Engine>();
            _generateContainingResources();
        }

        protected override void _generateContainingResources()
        {
            //генерирует возможные движки, лежащие тут (такая реализация нужна, если я захочу еще какие-то движки, а также чтобы у движков было рандомное кач-во и 0 топлива)
            Engines.Add(new OilEngine(this));
            Engines.Add(new NuclearEngine(this));
            Engines.Add(new SunEngine(this));
        }

        public override Bitmap getImage()
        {
            return sprites.enginesModule;
        }
    }
}

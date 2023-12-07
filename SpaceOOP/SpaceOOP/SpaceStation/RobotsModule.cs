using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class RobotsModule: Module
    {
        public List<RobotItem> Robots { get; set; }
        public RobotsModule(Cell containigCell, SpaceStation connectedStation) : base(containigCell, connectedStation)
        {
            Robots = new List<RobotItem>();
            _generateContainingResources();
        }

        protected override void _generateContainingResources()//done
        {
            //генерирует возможных роботов, лежащих тут (такая реализация нужна, если я захочу еще какие-то модули)
            Robots.Add(new OilRobotItem(this));
            Robots.Add(new UraniumRobotItem(this));
        }

        public override Bitmap getImage()
        {
            return sprites.robotsModule;
        }

    }
}

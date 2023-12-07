using System.Collections.Generic;
using System.Drawing;


namespace SpaceOOP
{
    public abstract class Planet : StaticObject
    {
        public List<Resource> ContainingResources { get; set; }
        public Planet(Cell containingCell) : base(containingCell)
        {
            Radius = MapControll.Random.Next(3, 6);
            ContainingCell.ContainingObjects.Remove(this);
            ContainingResources = new List<Resource>();
        }

        protected override void _spawn(Point coords)
        {
            ContainingCell.ContainingObjects.Add(this);

            for (int i = coords.Y - Radius; i < coords.Y + Radius; i++)
            {
                for (int j = coords.X - Radius; j < coords.X + Radius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, this);
                        _spawnResource(MapControll.Cells[i, j], 101);
                    }
                }
            }
        }

        public override void LiveOneStep()
        {
            Point coords = ContainingCell.Coords;
     
            for (int i = coords.Y - Radius; i < coords.Y + Radius; i++)
            {
                for (int j = coords.X - Radius; j < coords.X + Radius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _spawnResource(MapControll.Cells[i, j], 2);
                    }
                }
            }
        }

        protected abstract void _spawnResource(Cell cell, int probability);
    }
}

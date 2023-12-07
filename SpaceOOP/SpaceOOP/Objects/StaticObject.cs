using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class StaticObject : MapObject
    {
        public int Radius { get; protected set; }
        public bool isSpawned { get; protected set; }

        public StaticObject(Cell containingCell) : base(containingCell) { }
 
        protected void _generateSomething(int i, int j, StaticObject spawnObject)
        {
            MapControll.Cells[i, j].ContainingObjects.Clear();
            MapControll.Cells[i, j].ContainingObjects.Add(spawnObject);
        }

        public void CheckSpawn()
        {
            Point coords = ContainingCell.Coords;

            bool canSpawn = true;
         
            for (int i = coords.Y - Radius * 5; i < coords.Y + Radius * 5; i++)
            {
                for (int j = coords.X - Radius * 5; j < coords.X + Radius * 5; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        foreach (var contObj in MapControll.Cells[i, j].ContainingObjects)
                        {
                            if ((contObj is EmptyPlanet || contObj is OilPlanet ||
                                contObj is ResourcefulPlanet || contObj is Star ||
                                contObj is DeathEnergy)
                                && contObj != this)
                            {
                                canSpawn = false;
                                break;
                            }
                        }
                    }
                    if (!canSpawn) break;
                }
                if (!canSpawn) break;
            }
        
            if (canSpawn)
            {

                _spawn(coords);

                isSpawned = true;
            }
            else isSpawned = false;
        }

        protected virtual void _spawn(Point coords) { }

        public override void LiveOneStep() { }
    }
}

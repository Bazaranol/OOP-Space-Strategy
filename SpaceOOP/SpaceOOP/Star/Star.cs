
using System.Drawing;

namespace SpaceOOP
{
    public class Star : StaticObject
    {
        private int _deathRadius;
        private int _energyRadius;

        public Star(Cell containigCell) : base(containigCell)
        {
            Radius = MapControll.Random.Next(8, 13);
            ContainingCell.ContainingObjects.Remove(this);

            if (Radius < 10) _deathRadius = 2;
            else if (Radius < 12) _deathRadius = 3;
            else _deathRadius = 4;
            _energyRadius = _deathRadius * 2;
        }

        protected override void _spawn(Point coords)
        {
            ContainingCell.ContainingObjects.Add(this);
            //заполняет окружение собой и спавнит что-то
            for (int i = coords.Y - Radius; i < coords.Y + Radius; i++)
            {
                for (int j = coords.X - Radius; j < coords.X + Radius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, this);
                    }
                }
            }
            //названия говорят за себя
            _generateDeathEnergy(coords.Y, coords.X);
            _generateEnergy(coords.Y, coords.X);
        }

        private void _generateDeathEnergy(int i1, int j1)
        {
            //распространяет энергию на одной из сторон звезды, остальные циклы для остальных сторон
            for (int i = i1 - Radius - _deathRadius; i < i1 - Radius; i++)
            {
                for (int j = j1 - Radius - _deathRadius; j < j1 + Radius + _deathRadius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new DeathEnergy(MapControll.Cells[i, j]));
                    }
                }
            }

            for (int i = i1 - Radius; i < i1 + Radius + _deathRadius; i++)
            {
                for (int j = j1 - Radius - _deathRadius; j < j1 - Radius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new DeathEnergy(MapControll.Cells[i, j]));
                    }
                }
            }

            for (int i = i1 + Radius; i < i1 + Radius + _deathRadius; i++)
            {
                for (int j = j1 - Radius; j < j1 + Radius + _deathRadius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new DeathEnergy(MapControll.Cells[i, j]));
                    }
                }
            }

            for (int i = i1 - Radius; i < i1 + Radius; i++)
            {
                for (int j = j1 + Radius; j < j1 + Radius + _deathRadius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new DeathEnergy(MapControll.Cells[i, j]));
                    }
                }
            }
        }

        private void _generateEnergy(int i1, int j1)
        {
            //распространяет энергию на одной из сторон смертельной энергии, остальные циклы для остальных сторон
            for (int i = i1 - Radius - _deathRadius - _energyRadius; i < i1 - Radius - _deathRadius; i++)
            {
                for (int j = j1 - Radius - _deathRadius - _energyRadius; j < j1 + Radius + _deathRadius + _energyRadius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new UsefulEnergy(MapControll.Cells[i, j]));
                    }
                }
            }

            for (int i = i1 - Radius - _deathRadius; i < i1 + Radius + _deathRadius + _energyRadius; i++)
            {
                for (int j = j1 - Radius - _deathRadius - _energyRadius; j < j1 - Radius - _deathRadius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new UsefulEnergy(MapControll.Cells[i, j]));
                    }
                }
            }

            for (int i = i1 + Radius + _deathRadius; i < i1 + Radius + _deathRadius + _energyRadius; i++)
            {
                for (int j = j1 - Radius - _deathRadius; j < j1 + Radius + _deathRadius + _energyRadius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new UsefulEnergy(MapControll.Cells[i, j]));
                    }
                }
            }

            for (int i = i1 - Radius - _deathRadius; i < i1 + Radius + _deathRadius; i++)
            {
                for (int j = j1 + Radius + _deathRadius; j < j1 + Radius + _deathRadius + _energyRadius; j++)
                {
                    if (i < MapControll.Size && j < MapControll.Size && i >= 0 && j >= 0)
                    {
                        _generateSomething(i, j, new UsefulEnergy(MapControll.Cells[i, j]));
                    }
                }
            }
        }

        public override Bitmap getImage()
        {
            return sprites.star;
        }

    }
}

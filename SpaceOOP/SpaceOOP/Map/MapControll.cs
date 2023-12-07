
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceOOP;

namespace SpaceOOP
{
    public class MapControll
    {
        public const int Size = 300;

        public static List<Cell> ChangedCells = new List<Cell>();

        public static Cell[,] Cells = new Cell[Size, Size];
        public static List<Planet> Planets = new List<Planet>();
        public static List<Star> Stars = new List<Star>();
        public static List<SpaceStation> SpaceStations = new List<SpaceStation>();

        private List<SpaceShip> _ships = new List<SpaceShip>();

        private Render _render;
        private PictureBox _pictureBox;
        public static Random Random = new Random();

        public MapControll(Render render, PictureBox pictureBox)
        {
            _render = render;
            _pictureBox = pictureBox;
        }

        public void InitializeCells()
        {
            //инициализируются клетки
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Cells[i, j] = new Cell(new Point(j, i), _pictureBox.Width / Size);
                    //спавнятся планеты и звезды
                    if (Random.Next(1, 100000) < 10)
                    {
                        var star = new Star(Cells[i, j]);
                        Stars.Add(star);
                    }

                    if (Random.Next(1, 100000) < 70)
                    {
                        int planetType = Random.Next(1, 4);

                        if (planetType == 1)
                        {
                            var planet = new EmptyPlanet(Cells[i, j]);
                            Planets.Add(planet);
                        }
                        else if (planetType == 2)
                        {
                            var planet = new OilPlanet(Cells[i, j]);
                            Planets.Add(planet);
                        }
                        else if (planetType == 3)
                        {
                            var planet = new ResourcefulPlanet(Cells[i, j]);
                            Planets.Add(planet);
                        }
                    }
                }
            }
            //если звезда не смогла заспавниться (потому что место занято) - удалить планету 
            for (int i = 0; i < Stars.Count(); i++)
            {
                Stars[i].CheckSpawn();
                if (!Stars[i].isSpawned)
                {
                    Stars.Remove(Stars[i]);
                    i--;
                }
            }
            //то же самое с планетами
            for (int i = 0; i < Planets.Count(); i++)
            {
                Planets[i].CheckSpawn();
                if (!Planets[i].isSpawned)
                {
                    Planets.Remove(Planets[i]);
                    i--;
                }
            }
            //спавнятся корабли и станции
            _spawnEntities();
            //рисуются клетки на карте
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _render.DrawCell(Cells[i, j]);
                }
            }

            _pictureBox.Refresh();
        }

        private void _spawnEntities()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Random.Next(1, 100000) < 40 && !Cells[i, j].ContainingObjects.Any())
                    {//спавн кораблей 
                        int shipType = Random.Next(1, 4);

                        if (shipType == 1)
                        {
                            var ship = new RegularShip(Cells[i, j]);
                            _ships.Add(ship);
                        }
                        else if (shipType == 2)
                        {
                            var ship = new NuclearShip(Cells[i, j]);
                            _ships.Add(ship);
                        }
                        else if (shipType == 3)
                        {
                            var ship = new SunShip(Cells[i, j]);
                            _ships.Add(ship);
                        }

                    }

                    if (Random.Next(1, 100000) < 15 && !Cells[i, j].ContainingObjects.Any())
                    {//спавн станций
                        var spaceStation = new SpaceStation(Cells[i, j]);
                        SpaceStations.Add(spaceStation);
                    }
                }
            }

            for (int i = 0; i < SpaceStations.Count(); i++)
            {//если станция не смогла заспавниться - удалить
                SpaceStations[i].CheckSpawn();
                if (!SpaceStations[i].isSpawned)
                {
                    SpaceStations.Remove(SpaceStations[i]);
                    i--;
                }
            }
        }

        public void LiveOneStep()
        {
            for (int i = 0; i < _ships.Count; i++)
            {
                _ships[i].LiveOneStep();
                
                if (_ships[i].IsDead)
                {
                    _ships[i].MakeLastWish();
                    _ships.Remove(_ships[i]);
                    i--;
                }
            }
            
            foreach (Planet planet in Planets)
            {
                planet.LiveOneStep();
            }
           
            foreach (Cell cell in ChangedCells)
            {
                _render.DrawCell(cell);
            }
          
            ChangedCells.Clear();

            _pictureBox.Refresh();
        }
    }
}

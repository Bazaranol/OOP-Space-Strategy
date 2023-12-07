using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class SpaceShip: MovingObject
    {
        protected double _balance;
        protected int _maxEngines;
        protected int _maxRobots;
        public List<Engine> Engines { get; protected set; }
        protected List<RobotItem> _robotsInStorage { get; set; }
        protected List<Robot> _robotsInSpace { get; set; }
        public SpaceShip(Cell containingCell) : base(containingCell)
        {
            Engines = new List<Engine>();
            _robotsInStorage = new List<RobotItem>();
            _robotsInSpace = new List<Robot>();
        }

        public override void LiveOneStep()
        {
            _countDeadOrNot();

            Engine usingEngine = null;
            foreach (var engine in Engines)
            {
                if (engine.Capacity >= 1)
                {
                    usingEngine = engine;
                }
            }
            if (usingEngine != null)
            {
                _balance += 1;
                if (_goal == null)
                {
                    _findGoal();
                }
                if (_goalOrNull() != null)
                {
                    _doSomethingWithGoal();
                    foreach (Engine engine in Engines)
                    {
                        engine.LiveOneStep(false);
                    }
                }
                else
                {
                    Cell newCell = _findNearestNeighbourCellToGoal();
                    _move(newCell);

                    usingEngine.LiveOneStep(true);

                    if (usingEngine.Condition <= 0)
                    {
                        Engines.Remove(usingEngine);
                    }
                }
            }
            else
            {
              
                ContainingCell.ContainingObjects.Remove(this);
                IsDead = true;
            }

            foreach (Robot robot in _robotsInSpace)
            {
                robot.LiveOneStep();
            }
        }

        public void MakeLastWish()
        {
            if (IsDead)
            {
                Engines.Clear();
                _robotsInStorage.Clear();
                ContainingCell.ContainingObjects.Remove(this);
                foreach (Robot robot in _robotsInSpace)
                {
                    robot.ContainingCell.ContainingObjects.Remove(robot);
                    robot.IsDead = true;
                    robot.LiveOneStep();
                }
                MapControll.ChangedCells.Add(ContainingCell);
            }
        }

        public void FindNewGoal()
        {
            _goal = null;
        }

        protected void _spawnRobots()
        {
            if (_goal.ContainingObjects[0] is OilPlanet)
            {
                for (int i = 0; i < _robotsInStorage.Count(); i++)
                {
                    if (_robotsInStorage[i] is OilRobotItem)
                    {
                        _robotsInSpace.Add(new OilRobot(_goal, this));
                        _robotsInStorage.Remove(_robotsInStorage[i]);
                        i--;
                    }
                }
            }

            else
            {
                for (int i = 0; i < _robotsInStorage.Count(); i++)
                {
                    if (_robotsInStorage[i] is OilRobotItem)
                    {
                        _robotsInSpace.Add(new OilRobot(_goal, this));
                    }
                    else
                    {
                        _robotsInSpace.Add(new UraniumRobot(_goal, this));
                    }

                    _robotsInStorage.Remove(_robotsInStorage[i]);
                    i--;
                }
            }

            MapControll.ChangedCells.Add(_goal);
        }

        protected void _makeEngineTransaction(EnginesModule module, string method, Engine engineToSell = null)
        {
            if (method == "buy")
            {
                foreach (Engine engine in module.Engines)
                {
                    if (_isEngineGood(engine))
                    {
                        _balance -= engine.Cost;

                        Engine newEngine;
                        if (engine is SunEngine)
                        {
                            newEngine = new SunEngine(this);
                        }
                        else if (engine is NuclearEngine)
                        {
                            newEngine = new NuclearEngine(this);
                        }
                        else
                        {
                            newEngine = new OilEngine(this);
                        }

                        newEngine.Condition = engine.Condition;
                        newEngine.Cost = engine.Cost;
                        newEngine.Capacity = engine.Capacity;
                        newEngine.MaxCapacity = engine.MaxCapacity;
                        
                        Engines.Add(newEngine);
                    }
                }
            }

            else if (method == "sell")
            {
                if (engineToSell != null)
                {
                    _balance += engineToSell.Cost;
                    Engines.Remove(engineToSell);
                }
            }
        }

        protected void _makeRobotTransaction(RobotsModule module, string method, RobotItem robotToSell = null)
        {
            if (method == "buy")
            {
                foreach (RobotItem robot in module.Robots)
                {
                    if (_isRobotGood(robot))
                    {
                        _balance -= robot.Cost;

                        OilRobotItem newRobot = new OilRobotItem(this);
                        _robotsInStorage.Add(newRobot);
                    }
                }
            }

            else if (method == "sell")
            {
                if (robotToSell != null)
                {
                    _balance += robotToSell.Cost;
                    _robotsInStorage.Remove(robotToSell);
                }
            }

        }

        protected void _callRobots()
        {
            foreach (Robot robot in _robotsInSpace)
            {
                double distance = _calculateDistance(robot.ContainingCell, _goal);

                if (robot.FuelInStorage > 20 && distance < 12)
                {
                    if (!robot.ShipNeedsYou)
                    {
                        robot.ShipNeedsYou = true;
                    }
                    _goal = ContainingCell;
                }
            }
        }

        protected void _buyFuel(Module module)
        {
            if (_balance > 0)
            {
                foreach (Engine engine in Engines)
                {
                    if (engine is NuclearEngine && module is UraniumModule || engine is OilEngine && module is OilModule)
                    {
                        double needsFuel = engine.MaxCapacity - engine.Capacity;
                        if (needsFuel >= _balance)
                        {
                            engine.Capacity += _balance;
                            _balance = 0;
                        }
                        else
                        {
                            _balance -= needsFuel;
                            engine.Capacity += engine.MaxCapacity;
                        }
                    }
                }
            }
        }

        protected Cell _findStationWithRequiredModule(Type moduleType)
        {
            Cell nearestCell = null;
            double distanceMin = 10000;
            foreach (var spaceStation in MapControll.SpaceStations)
            {
                bool containsRequired = false;
                foreach (Module module in spaceStation.ConnectedModules)
                {
                    if (module.GetType() == moduleType)
                    {
                        containsRequired = true;
                        break;
                    }
                }
                if (containsRequired)
                {
                    double distance = _calculateDistance(spaceStation.ContainingCell, ContainingCell);
                    if (distance < distanceMin)
                    {
                        distanceMin = distance;
                        nearestCell = spaceStation.ContainingCell;
                    }
                }
            }

            return nearestCell;
        }

        protected Cell _findNotInvadedPlanet()
        {
            Cell nearestCell = null;
            double distanceMin = 10000;

            foreach (var planet in MapControll.Planets)
            {
                double distance = _calculateDistance(planet.ContainingCell, ContainingCell);
                if (distance < distanceMin && planet.ContainingResources.Count() > 0)
                {
                    bool isGoodPlanet = false;
                    bool invadedPlanet = false;
                    foreach (Robot robot in _robotsInSpace)
                    {
                        if (Math.Abs(robot.ContainingCell.Coords.X - planet.ContainingCell.Coords.X) < 8 && Math.Abs(robot.ContainingCell.Coords.Y - planet.ContainingCell.Coords.Y) < 8)
                        {
                            invadedPlanet = true;
                            break;
                        }
                    }
                    if (invadedPlanet)
                    {
                        continue;
                    }
                    if (this is NuclearShip)
                    {
                        foreach (var contObj in planet.ContainingResources)
                        {
                            if (contObj is Uranium)
                            {
                                isGoodPlanet = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var contObj in planet.ContainingResources)
                        {
                            if (contObj is Oil)
                            {
                                isGoodPlanet = true;
                            }
                        }
                    }
                    if (!invadedPlanet && isGoodPlanet)
                    {
                        distanceMin = distance;
                        nearestCell = planet.ContainingCell;
                    }
                }
            }

            return nearestCell;
        }

        protected Cell _findRequiredRobot(double storageCapacity)
        {
            Cell nearestCell = null;
            double distanceMin = 10000;
            if (this is SunShip || this is NuclearShip)
            {
                foreach (Robot robot in _robotsInSpace)
                {
                    if (robot.FuelInStorage / robot.StorageCapacity > storageCapacity)
                    {
                        double distance = _calculateDistance(robot.ContainingCell, ContainingCell);
                        if (distance < distanceMin)
                        {
                            nearestCell = robot.ContainingCell;
                        }
                    }
                }
            }
            else
            {
                bool hasOilEngine = false;
                bool hasNuclearEngine = false;
                foreach (Engine engine in Engines)
                {
                    if (engine is NuclearEngine)
                    {
                        hasNuclearEngine = true;
                    }
                    else if (engine is OilEngine)
                    {
                        hasOilEngine = true;
                    }
                }
                foreach (Robot robot in _robotsInSpace)
                {
                    if (robot is UraniumRobot && hasNuclearEngine || robot is OilRobot && hasOilEngine)
                    {
                        if (robot.FuelInStorage / robot.StorageCapacity > storageCapacity)
                        {
                            double distance = _calculateDistance(robot.ContainingCell, ContainingCell);
                            if (distance < distanceMin)
                            {
                                nearestCell = robot.ContainingCell;
                            }
                        }
                    }
                }
            }

            return nearestCell;
        }

        private bool _isEngineGood(Engine engine)
        {
            if ((engine is SunEngine && this is SunShip || engine is NuclearEngine && (this is NuclearShip || this is RegularShip) || engine is OilEngine && this is RegularShip) && engine.Cost <= _balance)
            {
                return true;
            }
            return false;
        }

        private bool _isRobotGood(RobotItem robot)
        {
            if ((robot is OilRobotItem && (this is SunShip || this is RegularShip)) || (robot is UraniumRobotItem && (this is RegularShip || this is NuclearShip)) && robot.Cost <= _balance)
            {
                return true;
            }

            return false;
        }

        private void _countDeadOrNot()
        {
            if (Engines.Count() == 0)
            {
                ContainingCell.ContainingObjects.Remove(this);
                IsDead = true;
                return;
            }
            foreach (var contObj in ContainingCell.ContainingObjects)
            {
                if (contObj is DeathEnergy)
                {
                    ContainingCell.ContainingObjects.Remove(this);
                    IsDead = true;
                    return;
                }
            }

            double fuel = 0, maxFuel = 0;
            foreach (var engine in Engines)
            {
                fuel += engine.Capacity;
                maxFuel += engine.MaxCapacity;
            }
            //возможный исход смерти №3 - кончилось топливо
            if (fuel < 1)
            {
                ContainingCell.ContainingObjects.Remove(this);
                IsDead = true;
                return;
            }
        }

        protected override bool _goalOrNot(MapObject objGoal, MapObject objNeighbour)
        {
            //просто сравнивает, является ли предмет в соседней клетке предметом в целевой клетке
            if (objNeighbour == objGoal)
            {
                return true;
            }

            return false;
        }

        protected override Cell _findNearestNeighbourCellToGoal()
        {
            //функция смотрит, какая из доступных соседних клеток ближе к цели
            Cell nearestNeighbour = null;

            List<Cell> neighbours = _findNeighbours();

            double distanceMin = 10000;

            foreach (Cell neighbour in neighbours)
            {
                bool isCellAvailable = true;
                foreach (var contObj in neighbour.ContainingObjects)
                {
                    //если клетка занята планетой или звездой - не лети (так же есть шанс 0.1% попасть в смертельную энергию)
                    if (contObj is EmptyPlanet || contObj is OilPlanet || contObj is ResourcefulPlanet
                       || contObj is Star || (contObj is DeathEnergy && MapControll.Random.Next(1, 1001) < 999))
                    {
                        isCellAvailable = false;
                    }
                }
                //если клетка доступна - рассчитай расстояние от неё до цели, и выбери лучшего соседа
                if (isCellAvailable)
                {
                    double distance = _calculateDistance(neighbour, _goal);
                    if (distance < distanceMin)
                    {
                        distanceMin = distance;
                        nearestNeighbour = neighbour;
                    }
                }
            }

            return nearestNeighbour;
        }

        protected abstract void _buyWhatYouNeed(SpaceStation spaceStation);

        protected abstract void _sellWhatYouNeed(SpaceStation spaceStation);
    }
}

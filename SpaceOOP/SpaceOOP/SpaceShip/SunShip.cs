using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class SunShip: SpaceShip
    {
        public double OilStorageMaxCapacity { get; }
        public double OilStorageCapacity { get; set; }
        public SunShip(Cell containingCell) : base(containingCell)
        {
            Engines.Add(new SunEngine(this));
            _robotsInStorage.Add(new OilRobotItem(this));
            _balance = 200;
            _maxEngines = 3;
            _maxRobots = 3;
            OilStorageMaxCapacity = 200;
            OilStorageCapacity = 0;
        }

        protected override void _findGoal()
        {
            Cell newGoal = null;
            double fuel = 0, maxFuel = 0;

            //посчитает топливо
            foreach (var engine in Engines)
            {
                fuel += engine.Capacity;
                maxFuel += engine.MaxCapacity;
            }
            //если топлива мало - то должен искать солнечную энергию
            if (fuel / maxFuel < 0.5 || fuel < 30)
            {
                newGoal = _findUsefulEnergy();
            }
            else
            {//если с топливом все норм - смотрит на баланс
                if (_balance < 100)
                {
                    //ищет планету, где нет его роботов, чтобы на ней заспавнить новых роботов
                    if (_robotsInStorage.Count() > 0)
                    {
                        newGoal = _findNotInvadedPlanet();
                    }
                    //если роботов в хранилище нет, то полететь продавать нефть из хранилища, если оно заполнено на 50% и больше
                    else if (OilStorageCapacity / OilStorageMaxCapacity > 0.5)
                    {
                        newGoal = _findStationWithRequiredModule(typeof(OilModule));
                    }
                    else
                    {
                        //если и нефти нет, найти робота с содержащейся нефтью >= 30%
                        newGoal = _findRequiredRobot(0.3);
                        if (newGoal == null)
                        {
                            //если и робота не нашел, то продать нефть хотя бы если хранилище заполнено на 20%
                            if (OilStorageCapacity / OilStorageMaxCapacity > 0.2)
                            {
                                newGoal = _findStationWithRequiredModule(typeof(OilModule));
                            }
                            else
                            {
                                //ну если и с нефтью невезуха - лети зарядись, подожди пока роботы соберут нефти побольше
                                newGoal = _findUsefulEnergy();
                            }
                        }
                    }
                }

                else
                {
                    //если с деньгами все круто - лети покупай движки или роботов
                    if (Engines.Count() <= _robotsInStorage.Count() && Engines.Count() < _maxEngines)//с движками
                    {
                        newGoal = _findStationWithRequiredModule(typeof(EnginesModule));
                    }
                    else if (_robotsInStorage.Count() < Engines.Count() && _robotsInStorage.Count() < _maxRobots)//с роботами
                    {
                        newGoal = _findStationWithRequiredModule(typeof(RobotsModule));
                    }
                    else
                    {
                        //если ниче тебе не нужно и всего хватает емае - лети зарядись, подумай о жизни
                        newGoal = _findUsefulEnergy();
                    }
                }
            }

            _goal = newGoal;
        }
        protected override void _doSomethingWithGoal()
        {
            //функция, чтобы делать действие в зависимости от того, какова цель
            bool needToFindNewGoal = true;

            //если ты в энергии - заряжайся до конца
            if (_goal.ContainingObjects[0] is UsefulEnergy)
            {
                foreach (var engine in Engines)
                {
                    if (engine.MaxCapacity - engine.Capacity > 1)
                    {
                        needToFindNewGoal = false;
                    }
                }
            }

            else if (_goal.ContainingObjects[0] is OilPlanet || _goal.ContainingObjects[0] is ResourcefulPlanet)
            {
                //если прилетел к планете - зови роботов каких надо, забирай ресы
                _callRobots();
                needToFindNewGoal = false;
                //и спавнь новых если они есть в стораже
                if (_robotsInStorage.Any())
                {
                    _spawnRobots();
                    needToFindNewGoal = true;
                }
            }
            //прилетел к станции - покупай и продавай
            else if (_goal.ContainingObjects[0] is SpaceStation)
            {
                _sellWhatYouNeed((SpaceStation)_goal.ContainingObjects[0]);
                _buyWhatYouNeed((SpaceStation)_goal.ContainingObjects[0]);
                needToFindNewGoal = true;
            }

            if (needToFindNewGoal)
            {
                _goal = null;
            }
        }

        protected override void _buyWhatYouNeed(SpaceStation spaceStation)
        {
            foreach (Module module in spaceStation.ConnectedModules)
            {
                //первым делом - купи движки
                if (module is EnginesModule && Engines.Count() < _maxEngines)
                {
                    _makeEngineTransaction((EnginesModule)module, "buy");
                }
                //потом роботов
                else if (module is RobotsModule && _robotsInStorage.Count() < _maxRobots)
                {
                    _makeRobotTransaction((RobotsModule)module, "buy");
                }
            }
        }

        protected override void _sellWhatYouNeed(SpaceStation spaceStation)
        {
            foreach (Module module in spaceStation.ConnectedModules)
            {
                //продать все топливо что можешь
                if (module is OilModule && OilStorageCapacity > 0)
                {
                    _sellOil();
                }
                //если ситуация критична и роботов нет, а движков хватает - продай движок, заработай немного
                else if (module is EnginesModule && Engines.Count() < _maxEngines)
                {
                    if (_balance < 160 && _robotsInSpace.Count() < 2 && _robotsInStorage.Count() == 0)
                    {
                        _makeEngineTransaction((EnginesModule)module, "sell", Engines.Last());
                    }
                }
                //ну и с роботами то же самое
                else if (module is RobotsModule && _robotsInStorage.Count() < _maxRobots)
                {
                    if (_balance < 160 && Engines.Count() == 1 && Engines[0].Condition < 40)
                    {
                        _makeRobotTransaction((RobotsModule)module, "sell", _robotsInStorage.Last());
                    }
                }
            }
        }

        private Cell _findUsefulEnergy()
        {
            bool isUsefulEnergy = false;
            Cell nearestCell = null;
            Cell nearestStarCell = null;
            double distanceMin = 10000;

            //сначала найди ближайшую звезду
            foreach (var star in MapControll.Stars)
            {
                double distance = _calculateDistance(star.ContainingCell, ContainingCell);
                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    nearestStarCell = star.ContainingCell;
                }
            }

            distanceMin = 10000;
            //дальше ищем энергию сверху звезды
            for (int i = nearestStarCell.Coords.Y; isUsefulEnergy == false; i--)
            {
                if (i >= 0)
                {
                    double distance = _calculateDistance(MapControll.Cells[i, nearestStarCell.Coords.X], ContainingCell);
                    if (MapControll.Cells[i, nearestStarCell.Coords.X].ContainingObjects[0] is UsefulEnergy)
                    {
                        nearestCell = MapControll.Cells[i, nearestStarCell.Coords.X];
                        distanceMin = distance;
                        isUsefulEnergy = true;
                    }
                }
                else break;
            }

            isUsefulEnergy = false;
            //если энергия снизу звузды ближе, чем сверху - лети к нижней и наоборот
            for (int i = nearestStarCell.Coords.Y; isUsefulEnergy == false; i++)
            {
                if (i < MapControll.Size)
                {
                    double distance = _calculateDistance(MapControll.Cells[i, nearestStarCell.Coords.X], ContainingCell);
                    if (MapControll.Cells[i, nearestStarCell.Coords.X].ContainingObjects[0] is UsefulEnergy)
                    {
                        if (distance < distanceMin)
                        {
                            nearestCell = MapControll.Cells[i, nearestStarCell.Coords.X];
                        }
                        isUsefulEnergy = true;
                        break;
                    }
                }
                else break;
            }

            return nearestCell;
        }
        private void _sellOil()
        {//все просто
            _balance += OilStorageCapacity;
            OilStorageCapacity = 0;
        }

        public override Bitmap getImage()
        {
            return sprites.sunSpaceship;
        }
    }
}

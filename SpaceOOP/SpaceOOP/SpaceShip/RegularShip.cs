using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public class RegularShip: SpaceShip
    {
        public RegularShip(Cell containingCell) : base(containingCell)
        {
            Engines.Add(new OilEngine(this));
            _robotsInStorage.Add(new OilRobotItem(this));
            _balance = 300;
            _maxEngines = 3;
            _maxRobots = 3;
        }
     
        protected override void _buyWhatYouNeed(SpaceStation spaceStation)
        {
            foreach (Module module in spaceStation.ConnectedModules)
            {
                if (module is UraniumModule || module is OilModule)
                {
                    _buyFuel(module);
                }

                else if (module is EnginesModule && Engines.Count() < _maxEngines)
                {
                    _makeEngineTransaction((EnginesModule)module, "buy");
                }

                else if (module is RobotsModule && _robotsInStorage.Count() < _maxRobots)
                {
                    _makeRobotTransaction((RobotsModule)module, "buy");
                }
            }
        }

        protected override void _doSomethingWithGoal()
        {
            bool needToFindNewGoal = true;
            double fuel = 0;

           
            foreach (var engine in Engines)
            {
                fuel += engine.Capacity;
            }

            if (_goal.ContainingObjects[0] is ResourcefulPlanet || _goal.ContainingObjects[0] is OilPlanet)
            {
                _callRobots();

                if (_robotsInStorage.Any())
                {
                    _spawnRobots();
                    needToFindNewGoal = true;
                }
            }

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

        protected override void _findGoal()
        {

            Cell newGoal = null;
            double fuel = 0, maxFuel = 0;
            bool containsOilEngine = false;

            
            foreach (var engine in Engines)
            {
                fuel += engine.Capacity;
                maxFuel += engine.MaxCapacity;

                if (engine is OilEngine)
                {
                    containsOilEngine = true;
                }
            }

            if (fuel / maxFuel < 0.4 || fuel < 30)
            {
                if (_balance < 100)
                {
                    newGoal = _findRequiredRobot(0.3);
                    if (newGoal == null)
                    {
                        if (containsOilEngine)
                        {
                            newGoal = _findStationWithRequiredModule(typeof(OilModule));
                        }
                        else
                        {
                            newGoal = _findStationWithRequiredModule(typeof(UraniumModule));
                        }
                    }
                }
                else
                {
                    if (containsOilEngine)
                    {
                        newGoal = _findStationWithRequiredModule(typeof(OilModule));
                    }
                    else
                    {
                        newGoal = _findStationWithRequiredModule(typeof(UraniumModule));
                    }
                }
            }
            else
            {
                if (_balance < 150)
                {
                    if (_robotsInStorage.Count() > 0)
                    {
                        newGoal = _findNotInvadedPlanet();
                    }
                    else
                    {
                        if (containsOilEngine)
                        {
                            newGoal = _findStationWithRequiredModule(typeof(OilModule));
                        }
                        else
                        {
                            newGoal = _findStationWithRequiredModule(typeof(UraniumModule));
                        }
                    }
                }
                else
                {
                    if (Engines.Count() <= _robotsInStorage.Count() + 1 && Engines.Count() < _maxEngines)
                    {
                        newGoal = _findStationWithRequiredModule(typeof(EnginesModule));
                    }
                    else if (_robotsInStorage.Count() < Engines.Count() - 1 && _robotsInStorage.Count() < _maxRobots)
                    {
                        newGoal = _findStationWithRequiredModule(typeof(RobotsModule));
                    }
                    else
                    {
                        if (_robotsInStorage.Count() > 0)
                        {
                            newGoal = _findNotInvadedPlanet();
                        }
                        else
                        {
                            if (containsOilEngine)
                            {
                                newGoal = _findStationWithRequiredModule(typeof(OilModule));
                            }
                            else
                            {
                                newGoal = _findStationWithRequiredModule(typeof(UraniumModule));
                            }
                        }
                    }
                }
            }

            _goal = newGoal;
        }

        protected override void _sellWhatYouNeed(SpaceStation spaceStation)
        {
            foreach (Module module in spaceStation.ConnectedModules)
            {
                if (module is EnginesModule && Engines.Count() < _maxEngines)
                {
                    if (_balance < 60 && _robotsInSpace.Count() < 2 && _robotsInStorage.Count() == 0)
                    {
                        _makeEngineTransaction((EnginesModule)module, "sell", Engines.Last());
                    }
                }

                else if (module is RobotsModule && _robotsInStorage.Count() < _maxRobots)
                {
                    if (_balance < 100 && Engines.Count() > 2 && Engines[0].Condition < 50)
                    {
                        _makeRobotTransaction((RobotsModule)module, "sell", _robotsInStorage.Last());
                    }
                }
            }
        }

        public override Bitmap getImage()
        {
            return sprites.regularSpaceship;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class Robot: MovingObject
    {
        private SpaceShip _ownerShip;

        public double FuelInStorage { get; protected set; }
        public double StorageCapacity { get; protected set; }
        public bool ShipNeedsYou { get; set; }
        public Robot(Cell containingCell, SpaceShip ownerShip) : base(containingCell)
        {
            _ownerShip = ownerShip;
            FuelInStorage = 0;
            ShipNeedsYou = false;
        }

        public override void LiveOneStep()
        {
            if (IsDead)
            {
                MapControll.ChangedCells.Add(ContainingCell);
                return;
            }

            if (_goal == null)
            {
                _findGoal();
                return;
            }

            if (_goalOrNull() != null)
            {
                _doSomethingWithGoal();
            }
            else
            {
                Cell newCell = _findNearestNeighbourCellToGoal();
                _move(newCell);
            }
        }

        protected override Cell _findNearestNeighbourCellToGoal()
        {
            Cell nearestNeighbour = null;

            List<Cell> neighbours = _findNeighbours();

            double distanceMin = 10000;

            foreach (Cell neighbour in neighbours)
            {
                double distance = _calculateDistance(neighbour, _goal);
                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    nearestNeighbour = neighbour;
                }
            }

            return nearestNeighbour;
        }

        protected bool _dig(Resource resource)
        {
            if (FuelInStorage < StorageCapacity)
            {
                resource.ContainingFuel--;
                FuelInStorage++;

                if (!resource.CheckIsAlive())
                {
                    return false;
                }
            }

            return true;
        }

        protected override void _doSomethingWithGoal()
        {
           
            for (int i = 0; i < _goal.ContainingObjects.Count(); i++)
            {
                if (_goal.ContainingObjects[i] == _ownerShip)
                {
                    if (_ownerShip is SunShip)
                    {
                        _giveResourcesToStorage((SunShip)_ownerShip);
                    }
                    else
                    {
                        _giveResourcesToEngines(_ownerShip);
                    }
                    _ownerShip.FindNewGoal();
                    ShipNeedsYou = false;
                    _goal = null;

                    break;
                }
                
                if (FuelInStorage < StorageCapacity)
                {
                    if (_goal.ContainingObjects[i] is Oil || _goal.ContainingObjects[i] is Uranium)
                    {
                      
                        if (!_dig((Resource)_goal.ContainingObjects[i]))
                        {
                            i--;
                        }
                       
                        _goal = null;
                        break;
                    }
                }
                else break;
            }
        }

        protected override void _findGoal()
        {
            
            if (ShipNeedsYou)
            {
                _goal = _ownerShip.ContainingCell;
                return;
            }
        
            if (FuelInStorage < StorageCapacity)
            {
                Cell nearestCell = null;
                double distanceMin = 10000;

                Point coords = ContainingCell.Coords;

                int initialStartY = coords.Y - 8 >= 0 ? coords.Y - 8 : 0, initialStartX = coords.X - 8 >= 0 ? coords.X - 8 : 0;

                for (int i = initialStartY; i < coords.Y + 8 && i < MapControll.Size; i++)
                {
                    for (int j = initialStartX; j < coords.X + 8 && j < MapControll.Size; j++)
                    {
                        foreach (var contObj in MapControll.Cells[i, j].ContainingObjects)
                        {
                            if ((contObj is Oil && this is OilRobot) || (contObj is Uranium && this is UraniumRobot))
                            {
                                double distance = _calculateDistance(contObj.ContainingCell, ContainingCell);

                                if (distance < distanceMin)
                                {
                                    distanceMin = distance;
                                    nearestCell = contObj.ContainingCell;
                                }
                            }
                        }
                    }
                }

                _goal = nearestCell;
            }
        }

        protected override bool _goalOrNot(MapObject objGoal, MapObject objNeighbour)
        {
            if (objNeighbour == objGoal && ((objNeighbour is Oil && this is OilRobot) || (objNeighbour is Uranium && this is UraniumRobot) || (objNeighbour == _ownerShip)))
            {
                return true;
            }

            return false;
        }

        protected void _giveResourcesToStorage(SunShip ship)
        {
            double needsFuel = ship.OilStorageMaxCapacity - ship.OilStorageCapacity;

            if (needsFuel >= FuelInStorage)
            {
                ship.OilStorageCapacity += FuelInStorage;
                FuelInStorage = 0;
            }
            else
            {
                ship.OilStorageCapacity = ship.OilStorageMaxCapacity;
                FuelInStorage -= needsFuel;
            }
        }

        protected void _giveResourcesToEngines(SpaceShip ship)
        {
            foreach (Engine engine in _ownerShip.Engines)
            {
                if (FuelInStorage > 1)
                {
                    if (engine is NuclearEngine && this is UraniumRobot || engine is OilEngine && this is OilRobot)
                    {
                        double needsFuel = engine.MaxCapacity - engine.Capacity;
                        if (needsFuel >= FuelInStorage)
                        {
                            engine.Capacity += FuelInStorage;
                            FuelInStorage = 0;
                        }
                        else
                        {
                            engine.Capacity = engine.MaxCapacity;
                            FuelInStorage -= needsFuel;
                        }
                    }
                }
                else break;
            }
        }
    }
}

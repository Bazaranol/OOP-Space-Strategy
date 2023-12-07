using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceOOP
{
    public abstract class MovingObject: MapObject
    {
        public bool IsDead = false;
        protected Cell _goal;
        public MovingObject(Cell containingCell) : base(containingCell) { }

        protected void _move(Cell cell)
        {
        
            ContainingCell.ContainingObjects.Remove(this);
            MapControll.ChangedCells.Add(ContainingCell);

            ContainingCell = cell;
            ContainingCell.ContainingObjects.Add(this);
            //удалить если мертв
            if (!IsDead)
            {
                MapControll.ChangedCells.Add(ContainingCell);
            }
        }

        protected Cell _goalOrNull()
        {
        
            Cell nearestNeighbour = null;
            List<Cell> neighbours;

            neighbours = _findNeighbours();

            foreach (Cell neighbour in neighbours)
            {
                foreach (var contObjNeighbour in neighbour.ContainingObjects)
                {
                    foreach (var contObjGoal in _goal.ContainingObjects)
                    {
                        if (_goalOrNot(contObjGoal, contObjNeighbour))
                        {
                            nearestNeighbour = neighbour;
                        }
                    }
                }
            }

            return nearestNeighbour;
        }

        protected List<Cell> _findNeighbours()
        {

            List<Cell> neighbours = new List<Cell>();

            if (ContainingCell.Coords.Y - 1 >= 0) neighbours.Add(MapControll.Cells[ContainingCell.Coords.Y - 1, ContainingCell.Coords.X]);
            if (ContainingCell.Coords.X + 1 < MapControll.Size) neighbours.Add(MapControll.Cells[ContainingCell.Coords.Y, ContainingCell.Coords.X + 1]);
            if (ContainingCell.Coords.Y + 1 < MapControll.Size) neighbours.Add(MapControll.Cells[ContainingCell.Coords.Y + 1, ContainingCell.Coords.X]);
            if (ContainingCell.Coords.X - 1 >= 0) neighbours.Add(MapControll.Cells[ContainingCell.Coords.Y, ContainingCell.Coords.X - 1]);
            neighbours.Add(ContainingCell);

            return neighbours;
        }

        protected double _calculateDistance(Cell cell1, Cell cell2)
        {
         
            double distance = Math.Sqrt(Math.Pow(cell1.Coords.X - cell2.Coords.X, 2) + Math.Pow(cell1.Coords.Y - cell2.Coords.Y, 2));

            return distance;
        }

        protected abstract Cell _findNearestNeighbourCellToGoal();

        protected abstract void _findGoal();

        protected abstract void _doSomethingWithGoal();

        protected abstract bool _goalOrNot(MapObject objGoal, MapObject objNeighbour);
    }
}

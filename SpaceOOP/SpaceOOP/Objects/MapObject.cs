
using System.Drawing;


namespace SpaceOOP
{
    public abstract class MapObject
    {
        public Cell ContainingCell { set; get; }

        public MapObject(Cell containingCell)
        {
            ContainingCell = containingCell;
            ContainingCell.ContainingObjects.Add(this);
        }

        public abstract void LiveOneStep();
     
        public abstract Bitmap getImage();
    }
}

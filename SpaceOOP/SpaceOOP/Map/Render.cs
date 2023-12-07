using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceOOP
{
    public class Render
    {
        private PictureBox _map;
        private Graphics _graphics;
        private Pen _pen = new Pen(Color.FromArgb(255, 30, 30, 30));
        private SolidBrush _bgBrush = new SolidBrush(Color.Black);

        public Render(PictureBox map)
        {
            _map = map;
            _map.Image = new Bitmap(_map.Width, _map.Height);
            _graphics = Graphics.FromImage(_map.Image);
        }

        public void DrawCell(Cell cell)
        {
          
            var paintRectangle = new Rectangle(cell.Coords.X * cell.Size, cell.Coords.Y * cell.Size, cell.Size, cell.Size);

            _graphics.FillRectangle(_bgBrush, paintRectangle);
           
            foreach (var spaceObject in cell.ContainingObjects)
            {
                Bitmap obj = spaceObject.getImage();
                _graphics.DrawImage(obj, paintRectangle);
            }
           
            _graphics.DrawRectangle(_pen, paintRectangle);
        }
    }
}

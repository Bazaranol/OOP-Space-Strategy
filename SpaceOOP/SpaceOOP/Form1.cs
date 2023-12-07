using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceOOP
{
    public partial class Form1 : Form
    {
        private MapControll _map;
        private int counter = 0;

        public Form1()
        {
            InitializeComponent();
            startBtn.Enabled = true;
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if(counter == 0) {
                startBtn.Enabled = false;
                pictureBox1.Width =  1000;
                pictureBox1.Height = 1000;
                timer1.Interval = 500 / 10;

                Render render = new Render(pictureBox1);
                _map = new MapControll(render, pictureBox1);
                _map.InitializeCells();
                timer1.Start();
            }else
            {

                timer1.Start();
                startBtn.Enabled = false;
                stopBtn.Enabled = true;
            }

        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            startBtn.Enabled = true;
            stopBtn.Enabled = false;
            if (!timer1.Enabled)
            {
                return;
            }
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter++;
            _map.LiveOneStep();
        }
        public void Start()
        {
            if (timer1.Enabled)
            {
                return;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Explore
{
    public partial class Sensor1111 : UserControl
    {
        public int sensorValue;
        public Sensor1111()
        {
            InitializeComponent();
        }

        public void changeDisplay(int val)
        {
            trackBar1.Value = val;
        }

    }
}

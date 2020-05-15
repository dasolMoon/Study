using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Max_Min_Learnig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Class1 nn = new Class1();
            textBox1.Text = nn.run().ToString();

            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add("Draw");
            chart1.ChartAreas["Draw"].BackColor = Color.Black;

            chart1.ChartAreas["Draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["Draw"].AxisX.Maximum = 6;
            chart1.ChartAreas["Draw"].AxisX.Interval = 1;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["Draw"].AxisY.Minimum = 0;
            chart1.ChartAreas["Draw"].AxisY.Maximum = 6;
            chart1.ChartAreas["Draw"].AxisY.Interval = 1;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Series 추가(data)      
            chart1.Series.Add("Data");
            chart1.Series["Data"].ChartType = SeriesChartType.Point;
            chart1.Series["Data"].Color = Color.LightGreen;
            chart1.Series["Data"].BorderWidth = 1;
            chart1.Series["Data"].LegendText = "Data";

            // Series 추가(Cos)   
            chart1.Series.Add("Center");
            chart1.Series["Center"].ChartType = SeriesChartType.Point;
            chart1.Series["Center"].Color = Color.Orange;
            chart1.Series["Center"].BorderWidth = 1;
            chart1.Series["Center"].LegendText = "Center";


            chart1.Series["Data"].Points.AddXY(Class1.x[0, 0], Class1.x[0, 1]);
            chart1.Series["Data"].Points.AddXY(Class1.x[0, 2], Class1.x[0, 3]);
            chart1.Series["Data"].Points.AddXY(Class1.x[1, 0], Class1.x[1, 1]);
            chart1.Series["Data"].Points.AddXY(Class1.x[1, 2], Class1.x[1, 3]);
            chart1.Series["Data"].Points.AddXY(Class1.x[2, 0], Class1.x[2, 1]);
            chart1.Series["Data"].Points.AddXY(Class1.x[2, 2], Class1.x[2, 3]);
            chart1.Series["Data"].Points.AddXY(Class1.x[3, 0], Class1.x[3, 1]);
            chart1.Series["Data"].Points.AddXY(Class1.x[3, 2], Class1.x[3, 3]);


            chart1.Series["Center"].Points.AddXY(Class1.v[0, 0], Class1.v[0, 1]);
            chart1.Series["Center"].Points.AddXY(Class1.v[0, 2], Class1.v[0, 3]);
            
        }
    }
}

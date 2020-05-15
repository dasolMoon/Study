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

            chart2.ChartAreas.Clear();
            chart2.Series.Clear();
            chart2.ChartAreas.Add("Draw");
            chart2.ChartAreas["Draw"].BackColor = Color.Black;

            chart2.ChartAreas["Draw"].AxisX.Minimum = 0;
            chart2.ChartAreas["Draw"].AxisX.Maximum = 6;
            chart2.ChartAreas["Draw"].AxisX.Interval = 1;
            chart2.ChartAreas["Draw"].AxisX.MajorGrid.LineColor = Color.Gray;
            chart2.ChartAreas["Draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart2.ChartAreas["Draw"].AxisY.Minimum = 0;
            chart2.ChartAreas["Draw"].AxisY.Maximum = 6;
            chart2.ChartAreas["Draw"].AxisY.Interval = 1;
            chart2.ChartAreas["Draw"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart2.ChartAreas["Draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            // Series 추가(data)      
            chart2.Series.Add("Data");
            chart2.Series["Data"].ChartType = SeriesChartType.Point;
            chart2.Series["Data"].Color = Color.LightGreen;
            chart2.Series["Data"].BorderWidth = 1;
            chart2.Series["Data"].LegendText = "Data";

            // Series 추가(Cos)   
            chart2.Series.Add("Center");
            chart2.Series["Center"].ChartType = SeriesChartType.Point;
            chart2.Series["Center"].Color = Color.Orange;
            chart2.Series["Center"].BorderWidth = 1;
            chart2.Series["Center"].LegendText = "Center";

            chart2.Series["Data"].Points.AddXY(Class1.x[0, 0], Class1.x[0, 1]);
            chart2.Series["Data"].Points.AddXY(Class1.x[0, 2], Class1.x[0, 3]);
            chart2.Series["Data"].Points.AddXY(Class1.x[1, 0], Class1.x[1, 1]);
            chart2.Series["Data"].Points.AddXY(Class1.x[1, 2], Class1.x[1, 3]);
            chart2.Series["Data"].Points.AddXY(Class1.x[2, 0], Class1.x[2, 1]);
            chart2.Series["Data"].Points.AddXY(Class1.x[2, 2], Class1.x[2, 3]);
            chart2.Series["Data"].Points.AddXY(Class1.x[3, 0], Class1.x[3, 1]);
            chart2.Series["Data"].Points.AddXY(Class1.x[3, 2], Class1.x[3, 3]);

            chart2.Series["Center"].Points.AddXY(Class1.v[0, 0], Class1.v[0, 1]);
            chart2.Series["Center"].Points.AddXY(Class1.v[0, 2], Class1.v[0, 3]);

        }
    }
}

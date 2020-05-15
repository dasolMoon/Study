using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace K_means
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 nn = new Class1();
            textBox1.Text = nn.run().ToString();

            int i, j;
            Random r = new Random();

            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add("Draw");
            chart1.ChartAreas["Draw"].BackColor = Color.Black;

            chart1.ChartAreas["Draw"].AxisX.Minimum = 0;
            chart1.ChartAreas["Draw"].AxisX.Maximum = 500;
            chart1.ChartAreas["Draw"].AxisX.Interval = 50;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["Draw"].AxisY.Minimum = 0;
            chart1.ChartAreas["Draw"].AxisY.Maximum = 500;
            chart1.ChartAreas["Draw"].AxisY.Interval = 50;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            for (i = 0; i < Class1.CLUSTER; i++)
            {
                chart1.Series.Add(i.ToString());
                chart1.Series[i].ChartType = SeriesChartType.Point;
                chart1.Series[i].Color = Color.FromArgb(255, r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
                chart1.Series[i].BorderWidth = 1;
                chart1.Series[i].LegendText = "Data";
            }

            // Series 추가(Cos)   
            chart1.Series.Add("Center");
            chart1.Series["Center"].ChartType = SeriesChartType.Point;
            chart1.Series["Center"].Color = Color.Red;
            chart1.Series["Center"].BorderWidth = 1;
            chart1.Series["Center"].LegendText = "Center";

            for (i = 0; i < Class1.CLUSTER; i++)
            {
                for (j = 0; j < Class1.DATA; j++)
                {
                    if (Class1.U[j, i] == 1)
                    {
                        chart1.Series[i].Points.AddXY(Class1.x[j, 0], Class1.x[j, 1]);
                    }
                }
            }

            for (i = 0; i < Class1.CLUSTER; i++)
            {
                chart1.Series["Center"].Points.AddXY(Class1.v[i, 0], Class1.v[i, 1]);
            }
        }
    }   
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace PCM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strFilename = null;
            openFileDialog1.Title = "Open Text";
            openFileDialog1.Filter = " All Files(*.*) |*.*| Text File(*.txt) | *.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFilename = openFileDialog1.FileName;
            }
            Invalidate();

            PCM pcm = new PCM();
            pcm.run(strFilename);

            int i, j;
            Random r = new Random();
            
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add("Draw");
            chart1.ChartAreas["Draw"].BackColor = Color.Black;

            chart1.ChartAreas["Draw"].AxisX.Minimum = PCM.xmin - 5;
            chart1.ChartAreas["Draw"].AxisX.Maximum = PCM.xmax + 5;
            chart1.ChartAreas["Draw"].AxisX.Interval = (PCM.xmax - PCM.xmin) / 5;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["Draw"].AxisY.Minimum = PCM.ymin - 5;
            chart1.ChartAreas["Draw"].AxisY.Maximum = PCM.ymax + 5;
            chart1.ChartAreas["Draw"].AxisY.Interval = (PCM.ymax - PCM.ymin) / 5;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            for (i = 0; i < PCM.CLUSTER; i++)
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

            for (i = 0; i < PCM.CLUSTER; i++)
            {
                for (j = 0; j < PCM.DATA; j++)
                {
                    if (PCM.ct[j, i] == 1)
                    {
                        chart1.Series[i].Points.AddXY(PCM.x[j, 0], PCM.x[j, 1]);
                    }
                }
            }

            for (i = 0; i < PCM.CLUSTER; i++)
            {
                chart1.Series["Center"].Points.AddXY(PCM.v[i, 0], PCM.v[i, 1]);
            }
        }
    }
}

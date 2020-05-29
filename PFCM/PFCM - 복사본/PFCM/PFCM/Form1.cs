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

namespace PFCM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 데이터 입력
            string strFilename = null;
            openFileDialog1.Title = "Open Text";
            openFileDialog1.Filter = " All Files(*.*) |*.*| Text File(*.txt) | *.txt";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFilename = openFileDialog1.FileName;
            }
            Invalidate();
            #endregion

            // PFCM 실행
            PFCM PFCM = new PFCM();
            PFCM.run(strFilename);

            #region Chart에 PFCM 결과 출력
            int i, j;
            Random r = new Random();

            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.ChartAreas.Add("Draw");
            chart1.ChartAreas["Draw"].BackColor = Color.Black;

            chart1.ChartAreas["Draw"].AxisX.Minimum = PFCM.xmin - 5;
            chart1.ChartAreas["Draw"].AxisX.Maximum = PFCM.xmax + 5;
            chart1.ChartAreas["Draw"].AxisX.Interval = (PFCM.xmax - PFCM.xmin) / 5;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            chart1.ChartAreas["Draw"].AxisY.Minimum = PFCM.ymin - 5;
            chart1.ChartAreas["Draw"].AxisY.Maximum = PFCM.ymax + 5;
            chart1.ChartAreas["Draw"].AxisY.Interval = (PFCM.ymax - PFCM.ymin) / 5;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineColor = Color.Gray;
            chart1.ChartAreas["Draw"].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            for (i = 0; i < PFCM.CLUSTER; i++)
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

            for (i = 0; i < PFCM.CLUSTER; i++)
            {
                for (j = 0; j < PFCM.DATA; j++)
                {
                    if (PFCM.ct[j, i] == 1) chart1.Series[i].Points.AddXY(PFCM.x[j, 0], PFCM.x[j, 1]);
                }
            }

            for (i = 0; i < PFCM.CLUSTER; i++)
            {
                chart1.Series["Center"].Points.AddXY(PFCM.v[i, 0], PFCM.v[i, 1]);
            }
            #endregion
        }
    }
}

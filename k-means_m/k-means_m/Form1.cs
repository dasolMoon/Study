using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace k_means_m
{
    public partial class Form1 : Form
    {
        Input input = null;
        Point[] p = null;
        int k = 0;
        List<List<Point>> lastClusters = null, nowClusters = null;
        int lastAvgk = 0, nowAvgk = 0;
        double lastDistance = 0, nowDistance = 0;
        int lastChange = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Point get
            getPoint();
            Random r = new Random();
            Point firstPoint=p[r.Next(0,p.Length)];
            List<Point> points =null;

            //새로운 배열에 추가 
            points.Add(firstPoint);
            nowClusters.Add(points);

            //가장 가까운 클러스터 선정
            chooseCluster();

            //최고의 k를 구하기위한 검사
            chooseK();

            //현재 클러스터 목록을 이전 클러스터로 적용 후 초기화
            lastClusters = nowClusters;
            lastAvgk = nowAvgk;
            lastDistance = nowDistance;
           
        }

        void getPoint() //포인트 배열을 가져옴
        {
            input = new Input();
            p = input.getPoint();
        }

        double euclidean(Point a, Point b) //이전 k의 distance와 차이가 없으면 적음
        {
            double distance = 0;
            distance = Math.Sqrt(Math.Pow(b.X - a.X, 2.0)+ Math.Pow(b.Y - a.Y, 2.0));

            return distance; 
        }

        void chooseCluster() //가장 가까운 클러스터 선정
        {
            double distance = 0, minDistance = Double.MaxValue;
            int minIndex = 0;

            for (int j = 0; j < p.Length; j++)
            {
                for (int i = 0; i < nowClusters.Count; i++)
                {
                    minDistance = euclidean(nowClusters[i][0], p[j]);
                    if (distance < minDistance)
                    {
                        minIndex = i;
                    }
                }
                nowClusters[minIndex].Add(p[j]);
            }
        }

        void chooseK() //최고의 k를 구하기위한 검사 - 각 클러스터에서 좌표들과 centroid사이의 거리를 구하여 더한다
        {
            for (int i = 0; i < nowClusters.Count; i++)
            {
                for (int j = 0; j < nowClusters[i].Count; j++)
                    nowDistance += euclidean(nowClusters[i][0], nowClusters[i][j]);
            }

            double change = nowDistance - lastDistance;

            if (change < lastChange)
            {
                //최적의 k가 이미 등장했을 수 있다.
            }
        }
    }
}

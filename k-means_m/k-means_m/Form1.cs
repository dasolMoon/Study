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
        List<Point> pk= null;
        List<List<Point>> lastClusters = null, nowClusters = null;
        int lastAvgk = 0, nowAvgk = 0; //이거안썼음
        double lastDistance = 0, nowDistance = 0;
        int lastChange = 0;
        int k = 1;
        List<Point> centroid = null;
        public Form1()
        {
            InitializeComponent();
        }

        void init()
        {
            //초기화작업
            nowClusters = new List<List<Point>>();
            pk = new List<Point>();
            centroid = new List<Point>();

            //데이터 가져오기
            getPoint();
            for (int i = 0; i < p.Length; i++) pk.Add(p[i]);

            Console.WriteLine("초기화 단계 완료");

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //초기화 작업 진행
            init();

            //랜덤 클러스터 정하기
            Random random = new Random();
            int r = random.Next(0, pk.Count);
            addCluster(pk[r]);
            pk.RemoveAt(r);
            Console.WriteLine("첫번째 단계 완료");

            //가장 가까운 클러스터 선정
            chooseCluster();
            Console.WriteLine("두번째 단계 완료");

            //클러스터 내부의 새로운 centroid를 구함
            inCluster();

            //최고의 k를 구하기위한 검사
            chooseK();
            Console.WriteLine("n번째 단계 완료");
        }
        void getPoint() //포인트 배열을 가져옴
        {
            input = new Input();
            p = input.getPoint();

        }
        void addCluster(Point cent)
        {
           centroid.Add(cent);
        }
        double euclidean(Point a, Point b) //a와 b사이의 거리
        {
            double distance = 0;
            distance = Math.Sqrt(Math.Pow(b.X - a.X, 2.0) + Math.Pow(b.Y - a.Y, 2.0));

            return distance;
        }

        void chooseCluster() //가장 가까운 클러스터 선정
        {
            double distance = 0, minDistance = Double.MaxValue;
            int minIndex = 0;

            for (int j = 0; j < p.Length; j++)
            {
                for (int i = 0; i < centroid.Count; i++)
                {
                    distance = euclidean(centroid[i], p[j]);
                    if (distance < minDistance)
                    {
                        minIndex = i;
                        minDistance = distance;
                    }
                }
                nowClusters[minIndex].Add(p[j]);// 아무것도 없어 ~~~~~~~~ 리스트가 초기화되어있지않다는 말임. 세부리스트에 넣고 이후에 넣을것
            }
        }
        void inCluster() //클러스터 안에서 새로운 centroid를 정함
        {
            int sum_x = 0, sum_y = 0, count = 0;
            Point newCentroid = new Point(0,0);
            for(int i = 0; i<centroid.Count; i++)
            {
                for(int j = 0; j<nowClusters[i].Count; j++)
                {
                    sum_x += nowClusters[i][j].X;
                    sum_y += nowClusters[i][j].Y;
                    count++;
                }
                centroid[i] = new Point(sum_x / count, sum_y / count);
            }

        }
        void chooseK() //최고의 k를 구하기위한 검사 - 각 클러스터에서 좌표들과 centroid사이의 거리를 구하여 더한다
        {
            for (int i = 0; i < centroid.Count; i++)
            {
                for (int j = 0; j < nowClusters[i].Count; j++)
                    nowDistance += euclidean(centroid[i], nowClusters[i][j]);
            }

            double change = nowDistance - lastDistance;

            if (change < lastChange && lastClusters != null)
            {
                //최적의 k가 이미 등장했을 수 있다 !! 
                //과거를 ,. 이용하는 것 ,. 
            }
            else
            {
                //현재 클러스터 목록을 이전 클러스터로 적용 후 초기화
                lastClusters = nowClusters;
               // lastAvgk = nowAvgk;//이거안썼음
                lastDistance = nowDistance;
            }
        }
    }
}

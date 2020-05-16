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
        List<Point> pList = null;
        List<List<Point>> lastClusters = null, nowClusters = null;
        double lastDistance = 0;
        List<Point> centroid = null;
        bool re2 = true;
        int k = 1;
        public Form1()
        {
            InitializeComponent();
        }

        void init() //초기화작업
        {
             pList = new List<Point>();
            centroid = new List<Point>();

            //데이터 가져오기
            getPoint();
            for (int i = 0; i < p.Length; i++) pList.Add(p[i]);
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            init();

            while (re2)// 랜덤 부여 ~ 변화비교
            {
                Console.WriteLine("**********전체 반복**********");//임시
                //랜덤 클러스터 정하기
                setK();
                Console.WriteLine("첫번째 단계 완료");
                bool re1 = true;
                while (re1)//가까운 클러스터를 설정하고 새로운 centroid를 구함
                {
                    //nowcluster여기서초기화
                    clearnowCluster();
                    Console.WriteLine("**********부분 반복**********");//임시
                    List<Point> lastCentroid = new List<Point>(centroid);
                    //가장 가까운 클러스터 선정
                    chooseCluster();
                    Console.WriteLine("두번째 단계 완료");

                    //클러스터 내부의 새로운 centroid를 구함
                    inCluster();
                    Console.WriteLine("세번째 단계 완료");

                    bool isSame = true;
                    for (int i = 0; i < centroid.Count; i++) //수정
                    {
                        if (lastCentroid[i] != centroid[i])
                            isSame = false;
                    }
                    if (isSame == true) re1 = false;
                    Console.WriteLine("Centroid값 조정 끝");//임시
                }

                //최고의 k를 구하기위한 검사
                chooseK();
                Console.WriteLine("네번째 단계 완료");//임시
            }
        }
        void getPoint() //포인트 배열을 가져옴
        {
            input = new Input();
            p = input.getPoint();

        }
        void clearnowCluster()
        {
            nowClusters = new List<List<Point>>();
            for (int i = 0; i < 7; i++)
            {
                List<Point> temp = new List<Point>();
                nowClusters.Add(temp);
            }
        }
        void setK() //랜덤 클러스터 정하기
        {
            int item = k - centroid.Count;
            for (int i = 0; i < item; i++)
            {
                Random random = new Random();
                int r = random.Next(0, pList.Count);
                addCluster(pList[r]);
                pList.RemoveAt(r);
            }

            chek();
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
            for (int i = 0; i < p.Length; i++)
            {
                double distance = 0, minDistance = Int32.MaxValue;
                int minIndex = 0;
                for (int j = 0; j < centroid.Count; j++)
                {
                    distance = euclidean(centroid[j], p[i]);
                    if (distance < minDistance)
                    {
                        minIndex = j;
                        minDistance = distance;
                    }
                }
                nowClusters[minIndex].Add(p[i]);
            }
        }
        void inCluster() //클러스터 안에서 새로운 centroid를 정함
        {
           // chek();//임시
            for (int i = 0; i < centroid.Count; i++)
            {
                int sum_x = 0, sum_y = 0, count = 0;
                for (int j = 0; j < nowClusters[i].Count; j++)
                {
                    sum_x += nowClusters[i][j].X;
                    sum_y += nowClusters[i][j].Y;
                    count++;
                }
                centroid[i] = new Point(sum_x / count, sum_y / count); //centroid 값을 조정했는데 하나가 만약에 0이 나오면 ? 지워야됨
            }
           // chek();//임시
        }

        void chek()
        {
            Console.WriteLine(" **클러스터 비교** " );
            for (int i = 0; i<centroid.Count;i++)
                Console.WriteLine(i+"번째 클러스터"+ centroid[0]);
        }
        void chooseK() //최고의 k를 구하기위한 검사 - 각 클러스터에서 좌표들과 centroid사이의 거리를 구하여 더한다
        {
            double nowDistance = 0;
            for (int i = 0; i < centroid.Count; i++)
            {
                for (int j = 0; j < nowClusters[i].Count; j++)
                    nowDistance += euclidean(centroid[i], nowClusters[i][j]);
            }

            nowDistance = nowDistance / centroid.Count;
           

            if (nowDistance >= lastDistance && lastClusters != null)
            {
                //최적의 k가 이미 등장했을 수 있다 !! 
                //과거를 ,. 이용하는 것 ,. 
                Console.WriteLine("이제 어쩌지!");

                //일단 종료
                re2 = false;
                //좌표평면에 그리기 넣기
            }
            else
            {
                //현재 클러스터 목록을 이전 클러스터로 적용
                lastClusters = new List<List<Point>>(nowClusters);
                lastDistance = nowDistance;
                
                if (k == p.Length || pList.Count==0)
                    re2 = false;
                else
                {
                    k = k * 2;
                    if (pList.Count < k)
                    {
                        k = pList.Count;
                    }
                }
            }
        }
    }
}

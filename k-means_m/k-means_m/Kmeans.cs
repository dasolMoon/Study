using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* 2020.05.16
 * 
 * k의 수는 3개로 설정함 
 * 데이터의 갯수는 n개
 * 
 * input_x : input data - x와 y가 한줄로 입력되어있기 때문에 2행의 배열로 만들어서 입력받는다.
 * centroid : 각 클러스터의 중심점
 * old_centroid : 이전단계에서의 각 클러스터의 중심점
 * distance : n번째 데이터가 속한 클러스터의 중심까지의 거리 배열
 * U : n번째 데이터가 k번째 클러스터에 속하면 1 아니면 0인 값을 갖는 binary 변수 배열 
*/
namespace K_means_m
{
    class Kmeans
    {
        int data_count;
        double[,] input_x = null;
        double[,] centroid = null;
        double[,] old_centroid = null;
        int[,] U = null;
        int k = 3;
        bool re = true;

        public Kmeans()
        {
            Init();
        }

        private void Init()
        {
            //데이터 가져오기
            InputData input = new InputData();
            double[] data = input.GetInputData();
            data_count = data.Length/2;
            input_x = new double[data_count, 2];

            //x,y좌표 구분해서 input data 입력
            int count = 0;
            for (int i = 0; i < input_x.GetLength(0); i++)
                for (int j = 0; j < 2; j++)
                {
                    input_x[i, j] = data[count];
                    count++;
                }

            //소속값 배열 초기화 
            U = new int[data_count,k];

            //centroid 배열 초기화
            centroid = new double[3, 2];
            old_centroid = new double[3, 2];

            //실행
            Run();
        }

        private void Run()
        {
            //소속값 배열로 랜덤하게 소속도를 준다 
            for (int i = 0; i < data_count; )
                for (int j = 0; j < k; j++)
                {
                    U[i, j] = 1;
                    i++;
                }
            /*
             for (i = 0; i < DATA;)
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    U[i, j] = 1;
                    i++;
                }
                //i++;
            }
            */
            Console.WriteLine("소속값 배열로 랜덤하게 소속도를 준다  완료");

            //각 클러스터의 centroid값을 구한다
            SetCentroid();

            //centroid값 저장
            SaveOldcentroid();
            PrintCentroid();//임시
            while (re)
            {
                Console.WriteLine("********반복 시작********");
                //모든 데이터에 대하여 가장 가까운 cluster를 선택한다.
                FindCluster();

                //centroid값 저장 
                SaveOldcentroid();
                PrintCentroid();//임시
                //각 클러스터의 centroid값을 구한다
                SetCentroid();
                PrintCentroid();//임시
                //이전 centroid와 비교
                compare();
            }
            Console.WriteLine("반복할지말지선택");
        }

        void FindCluster() //모든 데이터에 대하여 가장 가까운 cluster를 선택한다.
        {
            for (int i = 0; i < data_count; i++)
            {
                double min_index = 0, min_distance = Int32.MaxValue;
                for (int j = 0; j < k; j++)
                {
                    double distance = Euclidean(input_x[i, 0], input_x[i, 1], centroid[j, 0], centroid[j, 1]);
                    if (distance < min_distance)
                    {
                        min_index = j;
                        min_distance = distance;
                    }
                }
                //해당하는 부분만 1표시하기 
                for (int r = 0; r < k; r++)
                {
                    if (r == min_index) U[i, r] = 1;

                    else U[i, r] = 0;

                }

            }
        }

        void SetCentroid() //각 클러스터의 centroid값을 구한다
        {
            for (int i = 0; i < k; i++)//클러스터 종류
            {
                double sum_x = 0, sum_y = 0, count = 0;
                for (int j = 0; j < data_count; j++)
                {
                    if (U[j, i] == 1)
                    {
                        sum_x += input_x[j, 0];
                        sum_y += input_x[j, 1];
                        count++;
                    }

                }
                centroid[i, 0] = sum_x / count;
                centroid[i, 1] = sum_y / count;
            }

        }

        void SaveOldcentroid() //centroid 값 저장
        {
            for (int i = 0; i < centroid.GetLength(0); i++)
            {
                old_centroid[i, 0] = centroid[i, 0];
                old_centroid[i, 1] = centroid[i, 1];
            }
        }

        void compare() //이전 centroid와 비교 //가안되네 ? 
        {
            bool isSame = true;
            for (int i = 0; i < centroid.GetLength(0); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (old_centroid[i, j] != centroid[i, j])
                    {
                        isSame = false;
                    }
                }
            }

            if (isSame == true) re = false;
        }
        
        double Euclidean(double x1, double y1, double x2, double y2) //유클리디언 a와 b사이의 거리
        {
            double distance = 0;
            distance = Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));

            return distance;
        }

        void PrintCentroid()
        {
            Console.WriteLine("##Centfoid 확인##");
            for (int i=0; i<centroid.GetLength(0);i++)
                for (int j = 0; j < centroid.GetLength(1); j++)
                {
                    Console.Write(centroid[i,j]+ " ");
                }
            Console.WriteLine("");
        }
    }
}

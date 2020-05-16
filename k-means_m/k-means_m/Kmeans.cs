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
            for (int i = 0; i < input_x.GetLength(0); i++)
                for (int j = 0; j < 2; j++)
                {
                    input_x[i, j] = data[i];
                    i++;
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
            for (int i = 0; i < data_count; i++)
                for (int j = 0; j < k; j++)
                {
                    U[i, j] = 1;
                    i++;
                }

            //각 클러스터의 centroid값을 구한다
            for (int i = 0; i < k; i++)
            {
                double sum_x = 0, sum_y = 0;
                for (int j = 0; j < data_count; j++)
                {
                    if (U[j,i] == 1)
                    {
                        sum_x += input_x[j, 0];
                        sum_y += input_x[j, 1];
                    }

                }
                centroid[i, 0] = sum_x / data_count;
                centroid[i, 1] = sum_y / data_count;
            }

            //모든 데이터에 대하여 가장 가까운 cluster를 선택한다.
        }
    }
}

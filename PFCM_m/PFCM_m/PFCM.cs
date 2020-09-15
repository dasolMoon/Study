using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PFCM_m
{
    class PFCM
    { //정적변수
        static int CLUSTER = 3; //클러스터 개수
        static int INPUT_TYPE = 2; //입력 데이터중 한 쌍이 되는 데이터의 개수
        static int M = 2;// 지수의 가중치 
        static double THRESHOLD = 0.002;

        //입력데이터
        double[,] inputData = null;
        int dataCount = 0; //입력 데이터의 갯수

        //전체 입력데이터의 소속도
        double[,] u = null;//, uFuzzy=null;
        double[,] lastU = null;

        //전체 입력 데이터의 전형성
        double[,] T = null;
        double[,] lastT = null;


        //클러스터별 중심값
        double[,] centroid = null;

        //반복
        bool replay_f = true;//반복을 결정하는 bool변수  fcm
        int reCount_f = 1;// ()반복 횟수 

        bool replay_p = true;//반복을 결정하는 bool변수  pcm
        int reCount_p = 1;// ()반복 횟수 

        public PFCM()
        {
            Init();
        }
        private void Init() //기초작업 메소드
        {
            //Input data 가져오기
            InputData input = new InputData();
            input.Run();
            this.inputData = input.GetinputData();
            dataCount = inputData.GetLength(0);


            //전체 배열 초기화
            //fcm
            u = new double[dataCount, CLUSTER];
            centroid = new double[CLUSTER, INPUT_TYPE];
            //pcm
            T = new double[dataCount, CLUSTER];
            centroid = new double[CLUSTER, INPUT_TYPE];
        }

        public void Run() //FCM 핵심 메소드 Run
        {
            fcm_start();
            pcm_start();
        }

        private void fcm_start()
        {

            //초기 랜덤 소속함수 정의
            for (int i = 0; i < dataCount; i++)
            {
                int c = i % CLUSTER;
                u[i, c] = 1;
            }

            do
            {
                // 각 클러스터에 대한 중심 벡터 계산
                SetCentroid();

                //각 데이터들과 클러스터 중심과의 거리를 구한 후 
                //새로운 소속행렬 구성
                SetNewU();

                //식 종료판정
                Comparison();
            } while (replay_f);//종료조건에 맞지 않으면 반복

            Console.WriteLine("종료");
        }

        private void pcm_start()
        {
            //임의의 전형성 초기화
            for (int i = 0; i < dataCount; i++)
            {
                int t = i % CLUSTER;
                T[i, t] = 1;
            }

            do
            {
                // 각 클러스터에 대한 중심 벡터 계산
                SetCentroid();

                //각 데이터들과 클러스터 중심과의 거리를 구한 후 
                //새로운 소속행렬 구성
                SetNewU();

                //식 종료판정
                Comparison();
            } while (replay_f);//종료조건에 맞지 않으면 반복

            Console.WriteLine("종료");
        }

        private void SetCentroid()// 각 클러스터에 대한 중심 벡터 계산
        {
            for (int k = 0; k < CLUSTER; k++)
            {
                for (int j = 0; j < INPUT_TYPE; j++)
                {
                    double numerator = 0;//분자 설정
                    double denominator = 0;//분모 설정
                    for (int i = 0; i < dataCount; i++)
                    {
                        numerator += Math.Pow(u[i, k], M) * inputData[i, j]; //분자
                        denominator += Math.Pow(u[i, k], M);//분모
                    }
                    centroid[k, j] = numerator / denominator;

                }
            }
        }


        private void SetNewU()//새로운 소속행렬 구성
        {
            double[,] dw = new double[dataCount, CLUSTER];

            /* new weight */
            for (int i = 0; i < CLUSTER; i++)
            {
                for (int j = 0; j < dataCount; j++)
                {
                    for (int k = 0; k < INPUT_TYPE; k++)
                    {
                        dw[j, i] += Math.Pow((inputData[j, k] - centroid[i, k]), 2);
                    }

                    dw[j, i] = 1 / dw[j, i];

                    if (dw[j, i].Equals(double.NaN))
                    {
                        dw[j, i] = 1;
                    }
                }
            }

            for (int i = 0; i < dataCount; i++)
            {
                for (int j = 0; j < CLUSTER; j++)
                {
                    double wsum = 0;
                    for (int k = 0; k < CLUSTER; k++)
                    {
                        wsum += dw[i, k];
                    }

                    u[i, j] = (dw[i, j] / wsum);
                }
            }
        }

        private void Comparison() //종료조건을 만족하는지 검사 
        {
            if (reCount_f > 1)
            {
                double max = 0.0;

                for (int k = 0; k < CLUSTER; k++)
                {
                    for (int i = 0; i < dataCount; i++)
                    {
                        double temp = Math.Abs(u[i, k] - lastU[i, k]);
                        if (max < temp) max = temp;
                    }
                }

                if (max < THRESHOLD)
                {
                    replay_f = false;
                }

            }

            lastU = (double[,])u.Clone();//비교를위해 현재 행렬 복사
            reCount_f++;
        }

        public string GetResult()
        {
            string temp = null;

            for (int k = 0; k < CLUSTER; k++)
            {
                temp += "[ " + (k + 1) + "번째 클러스터 ] \r\n\r\ncentroid x : " + inputData[k, 0] + " y : " + inputData[k, 1];
                int count = 0;
                for (int i = 0; i < dataCount; i++)
                {
                    if (u[i, k] != 0)
                    {
                        temp += "\r\n\r\n" + ++count + "번째 데이터\r\n소속도 : " + u[i, k] + "\r\nx : " + inputData[i, 0] + "\r\ny : " + inputData[i, 1] + "\r\n";
                    }
                }
                temp += "\r\n\r\n\r\n";
            }

            return temp;
        }
    }
}

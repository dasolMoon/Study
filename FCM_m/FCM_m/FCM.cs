using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * 2020.06.12
 * FCM을 실행하는 클래스
 * InputData클래스에서 inputData를 가져온 후 FCM작업을 수행한다
 * 1.전체(U)행렬 초기화 ㅇ
 * 2.클러스터 중심백터 계산
 * 3.전체(U)행렬 업데이트 
 * 4.UU(k+1) - UU(k)< (임계값)이면 정지, 아니면 2단계로 돌아가 반복
 *                                        
 *  
 */
namespace FCM_m
{
    class FCM
    {
        //정적변수
        static int CLUSTER = 3; //클러스터 개수
        static int INPUT_TYPE = 2; //입력 데이터중 한 쌍이 되는 데이터의 개수
        static int M = 2;// 지수의 가중치 
        static double THRESHOLD = 0.0002;

        //입력데이터
        int[,] inputData = null;
        int dataCount = 0; //입력 데이터의 갯수

        //전체 입력데이터의 소속도 입력하는 배열
        double[,] u = null;//, uFuzzy=null;
        double[,] lastU = null;


        //클러스터별 중심값
        double[,] centroid = null;

        bool replay = true;//반복을 결정하는 bool변수
        int reCount = 0;// ()반복 횟수 

        public FCM()
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
            u = new double[dataCount, CLUSTER];
            centroid = new double[CLUSTER, INPUT_TYPE];
            //uFuzzy = new double[CLUSTER, dataCount];


        }

        public void Run() //FCM 핵심 메소드 Run
        {
            //초기 랜덤 소속함수 정의
            for (int i = 0; i < dataCount; i++)
            {
                int c = i % 3;
                u[i, c] = 1;
            }

            do
            {
                reCount++;
                // 각 클러스터에 대한 중심 벡터 계산
                SetCentroid();

                //각 데이터들과 클러스터 중심과의 거리를 구한 후 
                //새로운 소속행렬 구성
                SetNewU();

                //식 종료판정
                Comparison();
            } while (replay);//종료조건에 맞지 않으면 반복

            Console.WriteLine("종료");
        }

        private void SetCentroid()// 각 클러스터에 대한 중심 벡터 계산
        {
            for (int j = 0; j < CLUSTER; j++)
            {
                for (int r = 0; r < INPUT_TYPE; r++)  //x와 y 따로 - 변수 r 사용
                {
                    double numerator = 0;//분자 설정
                    double denominator = 0;//분모 설정
                    for (int i = 0; i < dataCount; i++)
                    {
                        numerator += Math.Pow(u[i, j], M) * inputData[i, r]; //분자
                        denominator += Math.Pow(u[i, j], M);//분모
                    }
                    centroid[j, r] = numerator / denominator;
                }
            }
        }

        private void SetNewU()//새로운 소속행렬 구성
        {
            lastU = (double[,])u.Clone();//비교를위해 새로운 행렬 구하기전 복사

            for (int i = 0; i < dataCount; i++)
            { // i번째 데이터 Xi
                double numerator = 0; //거리 분자
                double denominator = 0; //거리 분모
                double sum = 0;
                for (int j = 0; j < CLUSTER; j++)
                {// j번째 클러스터 Cj
                    for (int k = 0; k < CLUSTER; k++)
                    {// k번째 클러스터 Ck
                        //각각의 값 말고 배열을 보낼 때사용하는것도 만들어볼것
                        numerator += Distance(inputData[i, 0], inputData[i, 1], centroid[j, 0], centroid[j, 1]);
                        denominator += Distance(inputData[i, 0], inputData[i, 1], centroid[k, 0], centroid[k, 1]);
                        sum += Math.Pow((numerator / denominator), 2 / (M - 1));
                    }
                    u[i, j] = 1 / sum;
                }
            }
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            double distance = 0;
            distance = Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));

            return distance;
        }

        private void Comparison()
        {
            if (reCount > 1)
            {
                double max = double.MinValue;
                for (int i = 0; i < dataCount; i++)
                {
                    for (int j = 0; j < CLUSTER; j++)
                    {
                        double temp = u[i, j] - lastU[i, j];
                        if (max < temp)
                        {
                            max = temp;
                        }
                    }
                }

                //임계값과 비교
                if (max <= THRESHOLD)
                {
                    replay = false;
                }
            }
        }
    }
}

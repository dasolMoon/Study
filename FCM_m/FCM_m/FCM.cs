using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * 2020.06.12
 * FCM을 실행하는 클래스
 * InputData클래스에서 inputData를 가져온 후 FCM작업을 수행한다
 * 1.전체(U)행렬 초기화 
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
        static int CLUSTER = 2; //클러스터 개수
        static int INPUT_TYPE = 2; //입력 데이터중 한 쌍이 되는 데이터의 개수
        static int M = 2;// 지수의 가중치 
        static double THRESHOLD = 0.01;

        //입력데이터
        double[,] inputData = null;
        int dataCount = 0; //입력 데이터의 갯수

        //전체 입력데이터의 소속함수
        double[,] u = null;//, uFuzzy=null;
        double[,] lastU = null;


        //클러스터별 중심값
        double[,] centroid = null;

        //반복
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
                int c = i % CLUSTER;
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
                        numerator += Distance(inputData[i, 0], inputData[i, 1], centroid[j, 0], centroid[j, 1]);//분자
                        denominator += Distance(inputData[i, 0], inputData[i, 1], centroid[k, 0], centroid[k, 1]);//분모
                        double temp = Math.Pow((numerator / denominator), 2 / (M - 1));

                        if (double.IsNaN(temp)) //만약 0으로 나누는 경우가 생겨 값이 무한대가 된다면
                        {                       //숫자가 아니라면 
                            temp = 1;           //해당 계산값을 1로 설정한다.
                        }                       //영의 몫에 가까운 숫자가 0.00(...)1이면 1% temp는 0.00(...)1가 됨
                        sum += temp;
                    }
                    u[i, j] = 1 / sum;
                }
            }
        }

        private double Distance(double x1, double y1, double x2, double y2)//Data1과 Data2 거리 계산
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
        }

        private void Comparison() //종료조건을 만족하는지 검사 
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

        public string GetResult()
        {
            string temp = null;


            for (int i = 0; i < CLUSTER; i++)
            {
                temp += "[ " + (i + 1) + "번째 클러스터 ] \r\n\r\ncentroid x : " + inputData[i, 0] + " y : " + inputData[i, 1];
                int count = 0;
                for (int j = 0; j < dataCount; j++)
                {
                    if (u[j, i] != 0)
                    {
                        temp += "\r\n\r\n" + ++count + "번째 데이터\r\n소속도 : " + u[j, i] + "\r\nx : " + inputData[j, 0] + "\r\ny : " + inputData[j, 1] + "\r\n";
                    }
                }
                temp += "\r\n\r\n\r\n";
            }

            return temp;
        }
    }
}

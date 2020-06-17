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
        static double THRESHOLD = 0.002;

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
        int reCount = 1;// ()반복 횟수 

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
            for (int i = 0; i<CLUSTER; i++)
            {
                for (int j = 0; j<dataCount; j++)
                {
                    for (int k = 0; k<INPUT_TYPE; k++)
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

            for (int i = 0; i<dataCount; i++)
            {
                for (int j = 0; j<CLUSTER; j++)
                {
                    double wsum = 0;
                    for (int k = 0; k<CLUSTER; k++)
                    {
                        wsum += dw[i, k];
                    }

                    u[i, j] = (dw[i, j] / wsum);
                }
            }

            /*
            for (int i = 0; i < dataCount; i++)
            {
                for (int k = 0; k < CLUSTER; k++)
                {
                    double temp = 1 / Math.Pow(Distance(inputData[i, 0], inputData[i, 1], centroid[k, 0], centroid[k, 1]), 2 / (M - 1));
                    if (double.IsNaN(temp)) //만약 0으로 나누는 경우가 생겨 값이 무한대가 된다면
                    {                       //숫자가 아니라면 
                        temp = 1;           //해당 계산값을 1로 설정한다.
                    }

                    double sum = 0;
                    for (int j = 0; j < CLUSTER; j++)
                    {
                        double temp2 = Distance(centroid[j, 0], centroid[j, 1], centroid[k, 0], centroid[k, 1]);
                        double temp3 = 1 /  Math.Pow(temp2, 2 / (M - 1));
                        if (double.IsNaN(temp3)) //만약 0으로 나누는 경우가 생겨 값이 무한대가 된다면
                        {                       //숫자가 아니라면 
                            temp3 = 1;           //해당 계산값을 1로 설정한다.
                        }

                        sum += temp3;

                    }
                    u[i, k] = temp / sum;

                }
            }*/

            /*
            for (int i = 0; i < dataCount; i++)
            { // i번째 데이터 Xi
                for (int j = 0; j < CLUSTER; j++)
                {// j번째 클러스터 Cj
                    double sum = 0;
                    for (int k = 0; k < CLUSTER; k++)
                    {//분모를 위한 k번째 클러스터 Ck
                        double numerator = Distance(inputData[i, 0], inputData[i, 1], centroid[j, 0], centroid[j, 0]); //클러스터 개수 바뀌면 수정
                        double denominator = Distance(inputData[i,0], inputData[i,1], centroid[k,0], centroid[k,1]);
                        double temp = Math.Pow((numerator / denominator), (2 / (M - 1)));

                        if (double.IsNaN(temp)) //만약 0으로 나누는 경우가 생겨 값이 무한대가 된다면
                        {                       //숫자가 아니라면 
                            temp = 1;           //해당 계산값을 1로 설정한다.
                        }

                        sum += temp;
                    }
                    u[i, j] = 1 / sum;
                }
            }*/
        }

        private double Distance(double x1, double y1, double x2, double y2)//Data1과 Data2 거리 계산
        {
          //  return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
            return Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0);

        }

        private void Comparison() //종료조건을 만족하는지 검사 
        {
            if (reCount > 1)
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

                if(max < THRESHOLD)
                {
                    replay = false;
                }

            }

            lastU = (double[,])u.Clone();//비교를위해 현재 행렬 복사
            reCount++;
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

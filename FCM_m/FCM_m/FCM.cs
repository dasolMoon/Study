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
 * 3.전체(U)행렬 업데이트 <-UU(k+1) - UU(k)< (어떤기호)이면 정지, 
 *                                        아니면 2단계로 돌아감
 *                                        
 *   U끼리의 계산을 할 때(행렬계산) 따로 메소드가 필요하다. 
 *   U는 포함 된다 안 된다 배열(uBinary)과 소속도를 표현하는 배열(uFuzzy)로 두개 필요하다 
 */
namespace FCM_m
{
    class FCM
    {
        int CLUSTER = 3; //임의로 설정해도 되나? ->되는듯

        //입력데이터
        int[,] inputData = null;
        int dataCount = 0; //입력 데이터의 갯수

        //전체 입력데이터의 소속도 입력하는 배열
        double[,] u = null;//, uFuzzy=null;

        //클러스터별 중심값
        double[] centroid = null;

        bool replay = true;//반복을 결정하는 bool변수
        int R = 0;// ()반복 횟수 
       
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
            u = new double[CLUSTER, dataCount];
            centroid = new double[CLUSTER];
            //uFuzzy = new double[CLUSTER, dataCount];


        }

        private void Run() //FCM 핵심 메소드 Run
        {
            //초기 랜덤 소속함수 정의
            for (int j = 0; j < u.GetLength(0); j++)
            {
                for (int i = 0; i < CLUSTER; i++)
                {
                    u[i, j] = 1;
                }
            }


            // 각 클러스터에 대한 중심 벡터 계산
            setCentroid();

            //조건에 맞지 않으면(메소드에서 수정해주지않으면) while구간 반복
            while (replay)
            {

            }
        }

        private void setCentroid()// 각 클러스터에 대한 중심 벡터 계산
        {
            double numerator = 0;//분자 설정
            double denomintor = 0;//분모 설정

            for (int i = 0; i < CLUSTER; i++)
            {

                //centroid[i] = /
            }
        }
    }
}

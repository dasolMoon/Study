using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
/*
 *2020.05.19
 * input_x = 입력 데이터. 0행은 x 1행은 y값 입력
 * U = 소속값 배열
 * c = 클러스터의 갯수
 */
namespace HCM_m
{
    class hcm
    {
        double[] data = null;
        double[,] input_x = null;
        double[,] U = null;
        int c = 2;
        int data_count;
        public hcm()
        {
            Init();
        }
        void Init()
        {
            //data = Properties.Resources.test6;
            data = new double[] { 1.5, 2.3, 3.1, 2.5, 3.3, 4.2, 5.0, 3.7 };
            data_count = data.Length / 2;
            input_x = new double[data_count, 2];
            U = new double[data.Length, c];
        }

        public double[,] Run()
        {
            //입력 데이터 설정
            int count = 0;
            for (int i = 0; i < data_count; i++)
                for (int j = 0; j < 2; j++)
                {
                    input_x[i, j] = data[count];
                    count++;
                }
            

            return input_x;
        }
        
       
    }

}
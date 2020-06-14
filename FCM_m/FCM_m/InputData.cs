﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * 2020.06.12
 * 입력데이터를 처리하는 클래스 
 * 데이터 입력방식은 향후 수정
 * x, y 쌍으로 된 데이터가 1차원 배열로 늘어져있음
 * 
 */
namespace FCM_m
{
    class InputData
    {
        int INPUT_TYPE = 2; //입력 데이터중 한 쌍이 되는 데이터의 갯수
        double[] data = null;

        double[,] inputData = null;
        public InputData()
        {
            data = new double[] {119,168,131,168,129,161,127,168,127,172,128,172,133,172,138,183,140,169,139,159,137,159,150,157,157,162,152,
                173,148,179,143,177,144,168,158,159,160,154,168,154,180,161,180,175,167,181,152,181,161,165,182,158,206,157,
                218,164,214,177,179,177,166,173,180,150,200,150,216,160,195,175,195,169,192,171,183,181,185,178,172,162,182,
                161,206,161,226,155,229,153,212,150,208,153,197,168,209,176,249,177,254,159,242,149,229,159,223,165,221,176,
                232,171,255,163,254,162,242,166,232,177,203,179,185,183,176,187,158,187,174,191,209,195,224,195,260,187,272,
                181,280,159,278,150,242,141,201,144,168,150,153,153,151,160,159,171,245,174,286,174,328,173,340,169,346,165,
                312,154,273,153,231,155,216,155,287,149,329,153,336,156,306,169,276,175,245,183,223,185,233,183,247,181,274,
                184,296,177,302,171,281,160,259,164,254,173,269,168,264,168,239,144,251,141,269,150,246,148,272,148,282,150,
                293,149,294,150,303,155,319,156,322,162,320,166,311,170,300,176,279,183,243,188,233,188,212,191,199,191,197,
                185,197,180,212,170,215,150,202,153,202,153,195,163,188,166,179,168,181,163,186,155,188,152,196,156,208,147,
                213,147,221,147,229,150,230,150,232,153,240,165,242,165,243,157,244,156,249,155,254,152,264,147,264,147,278,
                141,289,143};

           // { 1.0, 1.0,6.0,6.0,7.0,7.0,6.0,7.0,2.0,3.0,4.0,4.0,6.0,4.0,4.5,5.5,2.0,6.0,3.0,7.0};


            inputData = new double[data.Length/INPUT_TYPE, INPUT_TYPE];
        }

        public void Run() //입력된 데이터를 x, y구분하여 input_data에 2차원배열로 대입한다.
        {
            int count = 0;
            for (int i = 0; i < data.Length / 2; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    inputData[i, k] = data[count];
                    count++;
                }
            }
        }

        public double[,] GetinputData() //배열 inputData를 배열 temp에 통째로 복사 후 temp를 return함 (보안)
        {
            double[,] temp = (double[,])inputData.Clone();
            //= new int[inputData.GetLength(0), inputData.GetLength(1)];
            return temp;



        }


    }
}

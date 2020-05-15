using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Max_Min_Learnig
{
    class Class1
    {
        public const int CLUSTER =2;
        public const int DATA = 125;
        const int COORD = 2;/*No of input ouput*/
        const double e = 1.0e-6;
        public static int[,] U = new int[DATA, CLUSTER];
        public static int[,] U_OLD = new int[DATA, CLUSTER];
        public static double[,] v = new double[CLUSTER, COORD];
        public static double[,] d = new double[DATA, CLUSTER];
        public static double[,] x = new double[DATA, COORD];
        public static double[] array = new double[DATA * 3];
        StringBuilder sb = new StringBuilder();
        public String run()
        {
            int i, j, k, b, iter = 0;
            double num, den, t_num, t_den, min;
            double t_dist, t2_dist;
            double Jm, Old_Jm, Jm_error;
            int count = 0;            
            FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\아톰\\퍼지\\test8(125).txt", FileMode.Open);
            StreamReader fp1 = new StreamReader(fp11);
            string line;

            //파일에서데이터값을스트링으로받아오기때문에스트링변수잡아줌
            //파일을불러와서ull값만날때까지읽어오는소스
            //파일의내용을차원배열에넣어준다
            while ((line = fp1.ReadLine()) != null)
            {
                array[count] = Convert.ToDouble(line);
                //스트링으로받아온데이터를ouble형으로바꿔준다
                count++;
            }

            //count값을다시초기화
            count = 0;

            //일차원배열에있는데이터를이차원배열에다시넣어준다
            //이차원배열에있는내용출력
            for (i = 0; i < DATA; i++)
                for (j = 0; j < COORD; j++)
                {
                    x[i, j] = array[count];
                    //sb.AppendLine("x :" + "[" + i + "]" + "[" + j + "]" + " = " + x[i, j]);
                    count++;
                }

            /****************************
            *Make U - matrix*
            ****************************/

            for (i = 0; i < DATA; i++)
            {/*Initialize U*/
                for (j = 0; j < CLUSTER; j++)
                {
                    U[i, j] = 0;
                    //sb.Append(U[i, j]);
                }
            }

            for (i = 0; i < DATA; i++)
            {
                U[i, 0] = 1;
                
            }

            for (i = 1; i < CLUSTER; i++)
            {
                U[DATA - i, i] = 1;
                U[DATA - i, 0] = 0;
            }

            //for (i = 0; i < CLUSTER; i++)
            //{/*Initialize U*/
            //    for (j = 0; j < DATA; j++)
            //    {
            //        //U[i, j] = 0;
            //        sb.Append(U[i, j]);
            //    }
            //}

            do
            {
                iter++;

                /****************************************
                *calculate center of cluster*
                ****************************************/

                for (i = 0; i < CLUSTER; i++)
                {/*Initialize vetor center(v) */
                    for (j = 0; j < COORD; j++)
                    {
                        num = 0;
                        //   den = 0.000001F;
                        den = 0.1;
                        for (k = 0; k < DATA; k++)
                        {
                            t_num = 0;
                            t_den = 0;
                            t_num = U[k, i] * x[k, j];
                            //sb.AppendLine("t_num = U[" + i + "," + k + "] * x[" + k + "," + j + "] : " + num);
                            t_den = U[k, i];
                            //sb.AppendLine("t_den = U[" + i + "," + j + "] : " + den);
                            num = num + t_num;
                            den = den + t_den;
                        }
                        v[i, j] = num / den;
                        //sb.AppendLine("v[" + i + "," + j + "] : " + v[i, j]);
                    }
                }
                /****************************************
                *Calculate distance detween data*
                *and cluster center*
                ****************************************/
                
                for (i = 0; i < CLUSTER; i++)
                {//Calculate distance(d)
                    for (j = 0; j < DATA; j++)
                    {
                        t2_dist = 0;
                        for (k = 0; k < COORD; k++)
                        {
                            t_dist = Math.Pow((x[j, k] - v[i, k]), 2);
                        //sb.AppendLine("x[" + j + "," + k + "] : " + x[j, k]);
                        //sb.AppendLine("v[" + i + "," + k + "] : " + x[i, k]);
                        t2_dist = t2_dist + t_dist;
                    }
                    d[j, i] = Math.Sqrt(t2_dist);
                    //sb.AppendLine("d[" + i + "," + j + "] : " + d[i, j]);
                    }   
                }
                 
                /*
                //맨하튼
                for (i = 0; i < CLUSTER; i++)
                {//Calculate distance(d)
                    for (j = 0; j < DATA; j++)
                    {
                        t2_dist = 0;
                        for (k = 0; k < COORD; k++)
                        {
                            t_dist = Math.Abs(x[j, k] - v[i, k]);
                            t2_dist = t2_dist + t_dist;
                        }
                        d[i, j] = t2_dist;
                    }
                }
                */

                Jm = 0;/*Calculation of new Jm*/

                for (k = 0; k < DATA; k++)
                {
                    for (i = 0; i < CLUSTER; i++)
                    {
                        Jm = Jm + U[k, i] * Math.Pow(d[k, i], 2);
                    }
                }

                for (i = 0; i < CLUSTER; i++)
                {/*Change of new U*/
                    for (j = 0; j < DATA; j++)
                    {
                        U_OLD[j, i] = U[j, i];
                        U[j, i] = 0;
                    }
                }

                /****************************************
                *Upgrade U matrix*
                ****************************************/

                for (i = 0; i < DATA; i++)
                {/*Calculater of new U*/
                    min = 0;
                    b = 0;
                    min = d[i, 0];
                    for (j = 1; j < CLUSTER; j++)
                    {
                        if (min > d[i, j])
                        {
                            min = d[i, j];
                            b = j;
                        }
                    }
                    U[i, b] = 1;
                }

                for (i = 0; i < CLUSTER; i++)
                {/*Calculation of vector center(v)*/
                    for (j = 0; j < COORD; j++)
                    {
                        num = 0;
                        den = 0;
                        for (k = 0; k < DATA; k++)
                        {
                            t_num = 0;
                            t_den = 0;
                            t_num = U[k, i] * x[k, j];
                            t_den = U[k, i];
                            num = num + t_num;
                            den = den + t_den;
                        }
                        v[i, j] = num / den;
                        //sb.AppendLine("v[" + i + "," + j + "] : " + v[i, j]);
                    }
                }
                Old_Jm = Jm;/*Difference of Jm*/
                Jm = 0.0;
                
                for (k = 0; k < DATA; k++)
                {
                    for (i = 0; i < CLUSTER; i++)
                    {
                        Jm = Jm + U[k, i] * Math.Pow(d[k, i], 2);
                    }
                }
                Jm_error = Math.Abs(Old_Jm - Jm);
            } while (e < Jm_error);/*do end*/


            for (i = 0; i < CLUSTER; i++)
            {
                sb.Append("{");
                for (j = 0; j < COORD; j++)
                {
                    sb.Append(v[i, j] + "  ");
                }
                sb.AppendLine("}\n");
            }

            //for (i = 0; i < CLUSTER; i++)
            //{
            //    for (j = 0; j < DATA; j++)
            //    {
            //        sb.Append(U[j, i]);
            //    }
            //    sb.AppendLine();
            //}
            return sb.ToString();
        }
    }
}

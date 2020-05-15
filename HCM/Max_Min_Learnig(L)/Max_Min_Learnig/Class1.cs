using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Max_Min_Learnig
{
    class Class1
    {
        const int CLUSTER =2;
        const int DATA = 4;
        const int COORD = 4;/*No of input ouput*/
        const double e = 1.0e-6;
        public static int[,] U = new int[CLUSTER, DATA];
        public static int[,] U_OLD = new int[CLUSTER, DATA];
        public static double[,] v = new double[CLUSTER, COORD];
        public static double[,] d = new double[CLUSTER, DATA];
        public static double[,] x = new double[DATA, COORD];
        public static double[] array = new double[80];
        StringBuilder sb = new StringBuilder();
        public String run()
        {
            int i, j, k, b, iter = 0;
            double num, den, t_num, t_den, min;
            double t_dist, t2_dist;
            double Jm, Old_Jm, Jm_error;
            int count = 0;            
            FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\아톰\\HCM\\test.txt", FileMode.Open);
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
                    sb.AppendLine("x :" + "[" + i + "]" + "[" + j + "]" + " = " + x[i, j]);
                    count++;
                }

            /****************************
            *Make U - matrix*
            ****************************/

            for (i = 0; i < CLUSTER; i++)
            {/*Initialize U*/
                for (j = 0; j < DATA; j++)
                {
                    U[i, j] = 0;
                }
            }
            for (i = 0; i < DATA; i++)
            {
                U[0, i] = 1;

            }
            for (i = 1; i < CLUSTER; i++)
            {
                U[i, DATA - i] = 1;
                U[0, DATA - i] = 0;
            }
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
                            t_num = U[i, k] * x[k, j];
                            //sb.AppendLine("t_num = U[" + i + "," + k + "] * x[" + k + "," + j + "] : " + num);
                            t_den = U[i, k];
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
                            t_dist = (x[j, k] - v[i, k]) * (x[j, k] - v[i, k]);
                        //sb.AppendLine("x[" + j + "," + k + "] : " + x[j, k]);
                        //sb.AppendLine("v[" + i + "," + k + "] : " + x[i, k]);
                        t2_dist = t2_dist + t_dist;
                    }
                    d[i, j] = Math.Sqrt(t2_dist);
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
                        Jm = Jm + U[i, k] * Math.Pow(d[i, k], 2);
                    }
                }

                for (i = 0; i < CLUSTER; i++)
                {/*Change of new U*/
                    for (j = 0; j < DATA; j++)
                    {
                        U_OLD[i, j] = U[i, j];
                        U[i, j] = 0;
                    }
                }

                /****************************************
                *Upgrade U matrix*
                ****************************************/

                for (i = 0; i < DATA; i++)
                {/*Calculater of new U*/
                    min = 0;
                    b = 0;
                    min = d[0, i];
                    for (j = 1; j < CLUSTER; j++)
                    {
                        if (min > d[j, i])
                        {
                            min = d[j, i];
                            b = j;
                        }
                    }
                    U[b, i] = 1;
                }

                for (i = 0; i < CLUSTER; i++)
                {/*Calculation of vector center(v)*/
                    num = 0;
                    den = 0;
                    for(int a=0; a<4; a++)
                    {
                        for (j = 0; j < COORD; j++)
                        {
                            for (k = 0; k < DATA; k++)
                            {
                                t_num = 0;
                                t_den = 0;
                                t_num += Math.Pow((Math.Abs(x[k, j])), 2);
                                t_den = Math.Min(v[i, a], x[k, j]);
                                num = num + t_num;
                                den = den + t_den;
                                sb.AppendLine("min : " + t_den);
                            }
                        }
                        num = Math.Sqrt(num);
                        //sb.AppendLine("num : " + num);
                    }
                    
                }
                Old_Jm = Jm;/*Difference of Jm*/
                Jm = 0.0;

                for (k = 0; k < DATA; k++)
                {
                    for (i = 0; i < CLUSTER; i++)
                    {
                        Jm = Jm + U[i, k] * Math.Pow(d[i, k], 2);
                    }
                }
                Jm_error = Math.Abs(Old_Jm - Jm);
            } while (e < Jm_error);/*do end*/


            for (i = 0; i < CLUSTER - 1; i++)
            {
                sb.Append("{");
                for (j = 0; j < COORD; j++)
                {
                    sb.Append(v[i, j] + "  ");
                }
                sb.AppendLine("}\n");
            }

            for (i = 0; i < CLUSTER; i++)
            {
                for (j = 0; j < DATA; j++)
                {
                    sb.Append(U[i, j]);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}

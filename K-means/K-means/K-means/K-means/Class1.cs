using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace K_means
{
    class Class1
    {
        StringBuilder sb = new StringBuilder();
        Random r = new Random();
        public static double[] array = new double[80];
        const int CLUSTER = 3;
        const int DATA = 7;
        const int COORD = 2;
        public static double[,] x = new double[DATA, COORD];
        public static double[,] d = new double[DATA, CLUSTER];
        public static double[,] v = new double[CLUSTER, COORD];
        public static double[,] v_OLD = new double[CLUSTER, COORD];
        public static int[,] U = new int[DATA, CLUSTER];

        public String run()
        {
            int i, j, k, count = 0, v_count = 0;
            double num, min;
            FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\아톰\\K-means\\test.txt", FileMode.Open);
            StreamReader fp1 = new StreamReader(fp11);
            string line;
            //파일에서데이터값을스트링으로받아오기때문에스트링변수잡아줌
            //파일을불러와서ull값만날때까지읽어오는소스
            //파일의내용을차원배열에넣어준다
                
            while ((line = fp1.ReadLine()) != null)
            {
                array[count] = Convert.ToDouble(line);
                //스트링으로받아온데이터를 double형으로바꿔준다
                count++;
            }

            //count값을다시초기화
            count = 0;

            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < COORD; j++)
                {
                    x[i, j] = array[count];
                    sb.AppendLine("x :" + "[" + i + "]" + "[" + j + "]" + " = " + x[i, j]);
                    count++;
                }
            }

            for (i = 0; i < CLUSTER; i++)
            {
                for (j = 0; j < COORD; j++)
                {
                    v[i, j] = r.Next(0, 10);
                    sb.AppendLine("v[" + i + "," + j + "] : " + v[i, j]);
                }
            }

            //v[0, 0] = 4;
            //v[0, 1] = 3;
            //v[1, 0] = 9;
            //v[1, 1] = 2;

            //sb.AppendLine("v[0,0] : " + v[0, 0]);
            //sb.AppendLine("v[0,1] : " + v[0, 1]);
            //sb.AppendLine("v[1,0] : " + v[1, 0]);
            //sb.AppendLine("v[1,1] : " + v[1, 1]);

            do
            {
                for (i = 0; i < DATA; i++)
                {/*Initialize U*/
                    for (j = 0; j < CLUSTER; j++)
                    {
                        U[i, j] = 0;
                    }
                }

                v_count = 0;
                                
                for ( i= 0; i < DATA; i++)
                {
                    min = 0;
                    count = 0;
                    for (j = 0; j < CLUSTER; j++)
                    {
                        d[i,j] = Math.Sqrt(Math.Pow((v[j, 0] - x[i, 0]), 2) + Math.Pow((v[j, 1] - x[i, 1]), 2));
                        if (j == 0)
                        {
                            min = d[i, j];                         
                        }
                        else if (j>0 && (d[i, j] < min))
                        {
                            min = d[i, j];
                            count=j;
                        }
                    }
                    U[i, count] = 1;
                }

                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < COORD; j++)
                    {
                        v_OLD[i, j] = v[i, j];
                    }
                }

                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < COORD; j++)
                    {
                        count = 0;
                        num = 0;
                        for (k = 0; k < DATA; k++)
                        {
                            num = num + (U[k, i] * x[k, j]);
                            if (U[k, i] == 1)
                            {
                                count++;
                            }
                        }
                        if ((num / count).Equals(double.NaN))
                        {
                            v[i, j] = 0;
                        }
                        else
                        {
                            v[i, j] = num / count;
                        }
                    }
                }

                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < COORD; j++)
                    {
                        if (v_OLD[i, j] != v[i, j])
                            v_count++;
                    }
                }
            } while (v_count != 0);

            //sb.AppendLine("v_count(수정) : " + v_count);
            for (i = 0; i < CLUSTER; i++)
            {
                for (j = 0; j < COORD; j++)
                {
                    sb.AppendLine("new_v[" + i + "," + j + "] : " + v[i, j]);
                }
            }

            //for(i=0; i<DATA; i++)
            //{
            //    for(j=0; j<CLUSTER; j++)
            //    {
            //        sb.AppendLine("d[" + i + "," + j + "] : " + d[i, j]);
            //    }
            //}

            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    sb.Append(U[i, j]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

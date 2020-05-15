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
        public static double[] array = new double[DATA*3];
        public const int CLUSTER = 3;
        public const int DATA = 10;
        const int COORD = 2;
        public static double[,] x = new double[DATA, COORD];
        public static double[,] d = new double[DATA, CLUSTER];
        public static double[,] v = new double[CLUSTER, COORD];
        public static double[,] v_OLD = new double[CLUSTER, COORD];
        public static double[] v_dist = new double[DATA];
        public static double[,] U = new double[DATA, CLUSTER];

        public String run()
        {
            int i, j, k, count = 0, v_count = 0;
            double num, max, min, sum, first, dist, distx;
            FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\아톰\\K-means\\test7.txt", FileMode.Open);
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

            for(i=0; i<COORD; i++)
            {
                sum = 0;
                for(j=0; j<DATA; j++)
                {
                    sum += x[j, i];
                }
                v[0, i] = sum / DATA;
                sb.AppendLine("v[0,i]" + v[0, i]);
            }

            count = 0;
            first = Math.Sqrt(Math.Pow((v[0, 0] - x[0, 0]), 2) + Math.Pow((v[0, 1] - x[0, 1]), 2));
            
            for (i=1; i<DATA; i++)
            {
                dist = 0;
                dist = Math.Sqrt(Math.Pow((v[0, 0] - x[i, 0]), 2) + Math.Pow((v[0, 1] - x[i, 1]), 2));
                sb.AppendLine("first" + i + ":" + first);
                first = Math.Min(first, dist);
                if(first == dist)
                {
                    count = i;
                }
            }

            for(i=0; i<COORD; i++)
            {
                v[0, i] = x[count, i];
            }

            
            for(k=1; k<CLUSTER; k++)
            {
                count = 0;
                max = 0;
                for (i = 0; i < DATA; i++)
                {
                    sum = 0;
                    for (j = 0; j < DATA; j++)
                    {
                        if (i != j)
                        {
                            dist = 0; distx = 0;
                            dist = Math.Sqrt(Math.Pow((v[k-1, 0] - x[j, 0]), 2) + Math.Pow((v[k-1, 1] - x[j, 1]), 2));
                            distx = Math.Sqrt(Math.Pow((x[i, 0] - x[j, 0]), 2) + Math.Pow((x[i, 1] - x[j, 1]), 2));
                            sum += Math.Max(dist - distx, 0);
                        }
                    }
                    v_dist[i] = sum;
                    if (i > 0)
                    {
                        max = Math.Max(v_dist[i], v_dist[i - 1]);
                        if (max == v_dist[i])
                        {
                            count = i;
                        }
                    }
                }
                v[k, 0] = x[count, 0];
                v[k, 1] = x[count, 1];
            }

            for (i = 0; i < CLUSTER; i++)
            {
                for (j = 0; j < COORD; j++)
                {
                    sb.AppendLine("v[" + i + "," + j + "] : " + v[i, j]);
                }
            }

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

            //for (i = 0; i < DATA; i++)
            //{
            //    for (j = 0; j < CLUSTER; j++)
            //    {
            //        sb.Append(U[i, j]);
            //    }
            //    sb.AppendLine();
            //}

            return sb.ToString();
        }
    }
}

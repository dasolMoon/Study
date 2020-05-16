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
        public const int DATA = 150;
        const int COORD = 2; ///  xy 정리
        public static double[,] x = new double[DATA, COORD];
        public static double[,] d = new double[DATA, CLUSTER];
        public static double[,] v = new double[CLUSTER, COORD];
        public static double[,] v_OLD = new double[CLUSTER, COORD];
        public static double[,] U = new double[DATA, CLUSTER];

        public String run()
        {
            int i, j, k, count = 0, v_count = 0;
            double num, min, sum, sumx, sumy;
            FileStream fp11 = new FileStream("D:\\GitRoom\\Study\\K-means\\test6.txt", FileMode.Open);
            StreamReader fp1 = new StreamReader(fp11);
            string line;
            //파일에서데이터값을스트링으로받아오기때문에스트링변수잡아줌
            //파일을불러와서ull값만날때까지읽어오는소스
            //파일의내용을차원배열에넣어준다
                
            ///array배열에 읽어들인 데이터를 모두 넣어준다
            while ((line = fp1.ReadLine()) != null)
            {
                array[count] = Convert.ToDouble(line);
                //스트링으로받아온데이터를 double형으로바꿔준다
                count++;
            } 

            //count값을다시초기화
            ///x와 y값을 구분해서 x배열에 입력
            count = 0;
            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < COORD; j++)
                {
                    x[i, j] = array[count];
                    count++;
                }
            }
            /*
            //int temp;
            //for (i = 0; i < DATA; i++)
            //{
            //    for (j = 0; j < COORD; j++)
            //    {
            //        if (j == 0)
            //        {
            //            temp = r.Next(30, 41);
            //            x[i, j] = temp * 0.1f;
            //        }
            //        else
            //        {
            //            temp = r.Next(10, 71);
            //            x[i, j] = temp * 0.1f;
            //        }
            //        //Position[i, j] = r.Next(0, (int)Math.Sqrt(DATA));
            //        //sb.Append(Position[i, j] + " ");
            //    }
            //    //sb.AppendLine();
            //}
            */
            ///U를 0으로 초기화함 <-꼭 해야될까..? 
            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    U[i, j] = 0;
                }
            }

            ///간혈적으로 U에 1을 넣어줌 아하 ! 
            for (i = 0; i < DATA;)
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    U[i, j] = 1;
                    i++;
                }
                //i++;
            }
            
            ///거리을 계산하나봅니다 아마 centroid겠쥬 
            for (i = 0; i < CLUSTER; i++)
            {
                sumx = 0;
                sumy = 0;
                count = 0;
                for (j = 0; j < DATA; j++)
                {
                    if(U[j,i] == 1)
                    {
                        sumx += x[j, 0];
                        sumy += x[j, 1];
                        count++;
                    }                                   
                }
                v[i, 0] = sumx / count;
                v[i, 1] = sumy / count;
                sb.AppendLine("v[" + i + ", 0] : " + v[i, 0]);
                sb.AppendLine("v[" + i + ", 1] : " + v[i, 1]);
            }
            ///여기까지 
            
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

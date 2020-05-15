using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FCM
{
    class Class1
    {
        StringBuilder sb = new StringBuilder();
        public static double[] array = new double[DATA * 2];
        Random r = new Random();
        public const int CLUSTER = 2;
        public const int DATA = 323;
        const int COORD = 2;
        const double ERROR = 0.4;
        const double a = 0.5;
        public static double[,] x = new double[DATA, COORD];
        public static double[,] v = new double[CLUSTER, COORD];
        public static double[,] w = new double[DATA, CLUSTER];
        public static double[,] w_OLD = new double[DATA, CLUSTER];
        public static double[,] d = new double[DATA, COORD];
        public static double[,] dw = new double[DATA, CLUSTER];
        public static int[,] cw = new int[DATA, CLUSTER];

        public static double xmax, xmin;
        public static double ymax, ymin;

        public String run()
        {
            int count = 0, i, j, k, c;
            double sum, wsum, total, max;
            FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\4학년 1학기\\캡스톤디자인\\PCM\\test6(323).txt", FileMode.Open);
            //FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\3학년 2학기\\컴퓨터비젼\\기말\\Face_Data\\31236(f1)\\f1.txt", FileMode.Open);
            //FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\3학년 2학기\\컴퓨터비젼\\기말\\Face_Data\\34944(f2)\\f2.txt", FileMode.Open);
            //FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\3학년 2학기\\컴퓨터비젼\\기말\\Face_Data\\46346(f3)\\f3.txt", FileMode.Open);
            //FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\3학년 2학기\\컴퓨터비젼\\기말\\Face_Data\\35614(f4)\\f4.txt", FileMode.Open);
            //FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\3학년 2학기\\컴퓨터비젼\\기말\\Face_Data\\30828(f5)\\f5.txt", FileMode.Open);
            StreamReader fp1 = new StreamReader(fp11);
            string line;

            while ((line = fp1.ReadLine()) != null)
            {
                array[count] = Convert.ToDouble(line);
                //스트링으로받아온데이터를 double형으로바꿔준다
                count++;
            }

            count = 0;

            xmin = double.MaxValue; xmax = double.MinValue;
            ymin = double.MaxValue; ymax = double.MinValue;

            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < COORD; j++)
                {
                    x[i, j] = array[count];
                    switch (j)
                    {
                        case 0:
                            xmin = Math.Min(x[i, j], xmin);
                            xmax = Math.Max(x[i, j], xmax);
                            break;
                        default:
                            ymin = Math.Min(x[i, j], ymin);
                            ymax = Math.Max(x[i, j], ymax);
                            break;
                    }
                    count++;
                    //Console.Write(x[i, j] + ", ");
                }
                //Console.WriteLine();
            }

            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    w[i, j] = 0;
                }
            }

            c = 0;
            do
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    w[c, j] = 1;
                    if (c < DATA - 1)
                    {
                        c++;
                    }
                }
            } while (c < DATA - 1);

            c = 0;
            int test_c = 0;

            do
            {
                for (i = 0; i < DATA; i++)
                {
                    for (j = 0; j < CLUSTER; j++)
                    {
                        w_OLD[i, j] = w[i, j];
                    }
                }

                for (i = 0; i < CLUSTER; i++)
                {
                    //wsum = 0;
                    for (j = 0; j < COORD; j++)
                    {
                        sum = 0;
                        wsum = 0;
                        for (k = 0; k < DATA; k++)
                        {
                            d[k, j] = Math.Pow(w[k, i], 2) * x[k, j];
                            sum += d[k, j];
                            wsum += Math.Pow(w[k, i], 2);

                            //sum += Math.Abs(Math.Min(x[k, j], w[k, i]));
                            //wsum += Math.Abs(w[k, i]);

                            //sum += Math.Min(x[k, j], w[k, i]);
                            //wsum += w[k, i];
                        }
                        //v[i, j] = Math.Abs(sum) / (a + Math.Abs(wsum));
                        v[i, j] = sum / wsum;
                    }
                }

                /* new weight */
                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < DATA; j++)
                    {
                        for (k = 0; k < COORD; k++)
                        {
                            dw[j, i] += Math.Pow((x[j, k] - v[i, k]), 2);
                        }
                        dw[j, i] = 1 / dw[j, i];
                        if (dw[j,i].Equals(double.NaN))
                        {
                            dw[j, i] = 1;
                        }
                    }
                }
                
                for (i = 0; i < DATA; i++)
                {
                    for (j = 0; j < CLUSTER; j++)
                    {
                        wsum = 0;
                        for (k = 0; k < CLUSTER; k++)
                        {
                            wsum += dw[i, k];
                        }
                        //w[i, j] = Math.Round((dw[i, j] / wsum), 1);
                        w[i, j] = (dw[i, j] / wsum);
                    }
                }



                test_c = 0;

                //for (i = 0; i < DATA; i++)
                //{
                //    for (j = 0; j < CLUSTER; j++)
                //    {
                //        if (w[i, j] != w_OLD[i, j])
                //        {
                //            test_c++;
                //        }
                //    }
                //}

                total=0;

                //for (i = 0; i < CLUSTER; i++)
                //{
                //    sum = 0;
                //    wsum = 0;
                //    for (j = 0; j < DATA; j++)
                //    {
                //        sum += w_OLD[j, i];
                //        wsum += w[j, i];
                //    }
                //    total += Math.Abs(wsum - sum);                    
                //}

                //if (total > ERROR)
                //{
                //    test_c++;
                //}

                for (i = 0; i < CLUSTER; i++)
                {
                    wsum = 0;
                    sum = 0;
                    for (j = 0; j < DATA; j++)
                    {
                        wsum += Math.Abs(w[j, i] - w_OLD[j, i]);
                    }
                    if(wsum > ERROR)
                    {
                        test_c++;
                    }
                }

                c++;
            } while (test_c != 0);
            

            for (i = 0; i < CLUSTER; i++)
            {
                sb.Append("v[" + i + "] : ");
                for (j = 0; j < COORD; j++)
                {
                    sb.Append(v[i, j] + "\t");
                }
                sb.AppendLine();
            }

            sb.AppendLine("c : " + c);
            sb.AppendLine("test_c : " + test_c);

            //for (i = 0; i < DATA; i++)
            //{
            //    for (j = 0; j < CLUSTER; j++)
            //    {
            //        sb.AppendLine("w : " + w[i, j]);
            //        sb.AppendLine("w_old : " + w_OLD[i, j]);
            //    }
            //    sb.AppendLine();
            //}

            for (i = 0; i < DATA; i++)
            {
                max = 0;
                count = 0;
                for (j = 0; j < CLUSTER; j++)
                {
                    if (j == 0)
                    {
                        max = w[i, j];
                    }
                    else if (j > 0 && (w[i, j] > max))
                    {
                        max = w[i, j];
                        count = j;
                    }
                }
                cw[i, count] = 1;
            }

            //for (i = 0; i < DATA; i++)
            //{
            //    for (j = 0; j < CLUSTER; j++)
            //    {
            //        sb.Append(cw[i, j]);
            //    }
            //    sb.AppendLine();
            //}

            return sb.ToString();
        }
    }
}

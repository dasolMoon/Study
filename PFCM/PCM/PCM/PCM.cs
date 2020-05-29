using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCM
{
    class PCM
    {
        public static double[] array = new double[DATA * 2];
        Random random = new Random();
        public const int CLUSTER = 2;
        public const int DATA = 323;
        const int COORD = 2;
        const double ERROR = 0.00001;
        public static double[,] x = new double[DATA, COORD];
        public static double[,] v = new double[CLUSTER, COORD];
        public static double[,] v_OLD = new double[CLUSTER, COORD];
        public static double[] dv = new double[CLUSTER];
        public static double[] vu = new double[CLUSTER];
        public static double[,] cv = new double[DATA, CLUSTER];
        public static double[,] t = new double[DATA, CLUSTER];
        public static double[,] t_OLD = new double[DATA, CLUSTER];
        public static double[,] d = new double[DATA, COORD];
        public static double[,] dt = new double[DATA, CLUSTER];
        public static double[,] vt = new double[DATA, CLUSTER];
        public static int[,] ct = new int[DATA, CLUSTER];

        public static double xmax, xmin;
        public static double ymax, ymin;

        public int[,] run(string fileName)
        {
            int count = 0, i, j, k, c;
            double sum, wsum, tsum, max;
            FileStream fp11 = new FileStream(fileName, FileMode.Open);
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
                            xmin = Math.Min(x[i,j], xmin);
                            xmax = Math.Max(x[i, j], xmax);
                            break;
                        default:
                            ymin = Math.Min(x[i, j], ymin);
                            ymax = Math.Max(x[i, j], ymax);
                            break;
                    }
                    //Console.Write(x[i, j] + ", ");
                    count++;
                }
                //Console.WriteLine();
            }            

            /* Typicality 값 초기화 */
            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    t[i, j] = random.NextDouble();
                    if (t[i,j] == 0)
                    {
                        t[i, j] = random.NextDouble();
                    }
                    //Console.Write(t[i, j] + ", ");
                }
                //Console.WriteLine();
            }

            c = 0;
            int test_c = 0;

            do
            {
                for (i = 0; i < DATA; i++)
                {
                    for (j = 0; j < CLUSTER; j++)
                    {
                        t_OLD[i, j] = t[i, j];
                    }
                }

                //for (i = 0; i < CLUSTER; i++)
                //{
                //    for (j = 0; j < COORD; j++)
                //    {
                //        v_OLD[i, j] = v[i, j];
                //    }
                //}

                /* center */
                for (i = 0; i < CLUSTER; i++)
                {
                    
                    for (j = 0; j < COORD; j++)
                    {
                        sum = 0;
                        wsum = 0;
                        tsum = 0;
                        for (k = 0; k < DATA; k++)
                        {
                            sum = Math.Pow(t[k, i], 2);
                            wsum += sum * x[k, j];
                            tsum += sum;
                        }
                        v[i, j] = wsum / tsum;
                    }                    
                }

                /* volume */
                double a;
                for (i = 0; i < CLUSTER; i++)
                {
                    wsum = 0;
                    tsum = 0;
                    for (j = 0; j < DATA; j++)
                    {
                        sum = 0;
                        for (k = 0; k < COORD; k++)
                        {
                            sum += Math.Pow(v[i, k] - x[j, k], 2);
                        }
                        wsum += Math.Pow(t[j, i], 2) * sum;
                        tsum += Math.Pow(t[j, i], 2);
                    }
                    dv[i] = wsum / tsum;
                }

                /* Typicality */
                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < DATA; j++)
                    {
                        sum = 0;
                        for (k = 0; k < COORD; k++)
                        {
                            sum += Math.Pow(v[i, k] - x[j, k], 2);
                        }
                        t[j, i] = 1 / (1 + (sum / dv[i]));
                    }
                }
                
                /* new Typicality */
                //for (i = 0; i < CLUSTER; i++)
                //{
                //    for (j = 0; j < DATA; j++)
                //    {
                //        t[j, i] = 1 / (1 + Math.Sqrt(dt[j, i] / cv[i]));
                //    }
                //}

                test_c = 0;

                for (i = 0; i < CLUSTER; i++)
                {
                    wsum = 0;
                    sum = 0;
                    for (j = 0; j < DATA; j++)
                    {
                        wsum += Math.Abs(t[j, i] - t_OLD[j, i]);
                    }
                    if (wsum > ERROR)
                    {
                        test_c++;
                    }
                }

                //for (i = 0; i < CLUSTER; i++)
                //{
                //    for (j = 0; j < COORD; j++)
                //    {
                //        if (v_OLD[i,j] != v[i,j])
                //        {
                //            test_c++;
                //        }
                //    }
                //}

                c++;
                //Console.WriteLine("C : " + c);
            } while (test_c != 0);
            Console.WriteLine("C : " + c);
            //Console.WriteLine();

            for (i = 0; i < CLUSTER; i++)
            {
                Console.WriteLine("v[" + i + "] : ");
                for (j = 0; j < COORD; j++)
                {
                    Console.WriteLine(v[i, j] + "\t");
                }
                Console.WriteLine();
            }

            for (i = 0; i < DATA; i++)
            {
                max = double.MinValue;
                count = 0;
                for (j = 0; j < CLUSTER; j++)
                {
                    if (max < t[i, j])
                    {
                        max = t[i, j];
                        count = j;
                    }
                }
                ct[i, count] = 1;
            }

            return ct;
        }
    }
}

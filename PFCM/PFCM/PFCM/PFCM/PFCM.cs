using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFCM
{
    class PFCM
    {
        #region Variable
        Random random = new Random();

        public static double[] array = new double[DATA * 2];
        public const int CLUSTER = 2;
        public const int DATA = 370;
        const int COORD = 2;
        const int a = 3;    
        const int b = 1;
        const double ERROR = 0.00001;
        public static double[,] x = new double[DATA, COORD];    // 데이터
        public static double[,] v = new double[CLUSTER, COORD]; // 중심
        public static double[,] v_OLD = new double[CLUSTER, COORD];
        public static double[] dv = new double[CLUSTER];
        public static double[] vu = new double[CLUSTER];
        public static double[,] cv = new double[DATA, CLUSTER];
        public static double[,] u = new double[DATA, CLUSTER];  // 소속도
        public static double[,] u_OLD = new double[DATA, CLUSTER];
        public static double[,] du = new double[DATA, CLUSTER];
        public static double[,] t = new double[DATA, CLUSTER];  // 전형성
        public static double[,] t_OLD = new double[DATA, CLUSTER];
        public static double[,] d = new double[DATA, COORD];
        public static double[,] dt = new double[DATA, CLUSTER];
        public static double[,] vt = new double[DATA, CLUSTER];
        public static int[,] ct = new int[DATA, CLUSTER];   // 소속된 값
        public static double xmax, xmin;
        public static double ymax, ymin;
        #endregion

        public int[,] run(string fileName)
        {
            #region Initialize
            int count = 0, i, j, k, c;
            double sum, wsum, tsum, max;
            FileStream fp11 = new FileStream(fileName, FileMode.Open);
            StreamReader fp1 = new StreamReader(fp11);
            string line;

            // txt 파일에서 데이터값 불러오기
            while ((line = fp1.ReadLine()) != null)
            {
                array[count] = Convert.ToDouble(line);
                //스트링으로받아온데이터를 double형으로바꿔준다
                count++;
            }

            // 데이터의 최대, 최소값 찾기
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
                }
            }
            #endregion

            #region Membership 초기화
            c = 0;
            do
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    u[c, j] = 1;
                    if (c < DATA - 1) c++;
                }
            } while (c < DATA - 1);
            #endregion

            #region Typicality 값 초기화
            for (i = 0; i < DATA; i++)
            {
                for (j = 0; j < CLUSTER; j++)
                {
                    t[i, j] = random.NextDouble();
                }
            }
            #endregion

            #region PFCM
            int exit = 0;
            c = 0;

            do
            {
                // 현 전형성을 이전 전형성으로 설정
                for (i = 0; i < DATA; i++)
                {
                    for (j = 0; j < CLUSTER; j++)
                    {
                        t_OLD[i, j] = t[i, j];
                    }
                }

                // 클러스터 중심벡터 계산
                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < COORD; j++)
                    {
                        sum = 0; wsum = 0; tsum = 0;
                        for (k = 0; k < DATA; k++)
                        {
                            sum = (a * Math.Pow(u[k, i], 2)) + (b * Math.Pow(t[k, i], 2));
                            wsum += sum * x[k, j];
                            tsum += sum;
                        }
                        v[i, j] = wsum / tsum;
                    }
                }

                // 전형성 계산
                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < DATA; j++)
                    {
                        for (k = 0; k < COORD; k++)
                        {
                            du[j, i] += Math.Pow((x[j, k] - v[i, k]), 2);
                        }
                        du[j, i] = 1 / du[j, i];
                        if (du[j, i].Equals(double.NaN)) du[j, i] = 0;
                    }
                }

                for (i = 0; i < DATA; i++)
                {
                    for (j = 0; j < CLUSTER; j++)
                    {
                        wsum = 0;
                        for (k = 0; k < CLUSTER; k++)
                        {
                            wsum += du[i, k];
                        }
                        u[i, j] = (du[i, j] / wsum);
                    }
                }

                // 부피값 계산
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

                // 전형성 갱신
                for (i = 0; i < CLUSTER; i++)
                {
                    for (j = 0; j < DATA; j++)
                    {
                        sum = 0;
                        for (k = 0; k < COORD; k++)
                        {
                            sum += Math.Pow(v[i, k] - x[j, k], 2);
                        }
                        t[j, i] = 1 / (1 + (b * sum / dv[i]));
                    }
                }

                exit = 0;

                // 변화율 계산
                for (i = 0; i < CLUSTER; i++)
                {
                    wsum = 0;
                    for (j = 0; j < DATA; j++)
                    {
                        wsum += Math.Abs(t[j, i] - t_OLD[j, i]);
                    }
                    if (wsum > ERROR) exit++;
                }

                c++;
            } while (exit != 0);

            // 반복 횟수 출력
            Console.WriteLine("C : " + c);
            #endregion

            #region 중심값 출력
            for (i = 0; i < CLUSTER; i++)
            {
                Console.WriteLine("v[" + i + "] : ");
                for (j = 0; j < COORD; j++)
                {
                    Console.WriteLine(v[i, j] + "\t");
                }
                Console.WriteLine();
            }
            #endregion

            #region Membership이 가장 큰 값 저장
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
            #endregion

            return ct;
        }
    }
}

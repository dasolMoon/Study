using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    class Program

    {

        static void Main(string[] args)
        {
            int Clust;
            int Pair;   //x,y??
            int Coordinates;    //데이터 개수?
            Random r = new Random();
            double[,] MemberShip;
            double[,] NMemberShip;
            double[,] Position; //데이터 값??
            double Max;
            double[,] Distance;
            double[] Means; //position 평균
            double[,] CovarianceMatrix; //공분산행렬
            double[,] iCovarianceMatrix;    //공분산 역행렬
            double tempNum; //역행렬 조건식
            int Check;
            int EXIT;
            Pair = 2;
            Coordinates = 30;
            Clust = 3;

            Console.WriteLine("Cluster 갯수: {0}", Clust);
            MemberShip = new double[Clust, Coordinates];
            NMemberShip = new double[Clust, Coordinates];
            Position = new double[Coordinates, Pair];
            Max = 0;
            Distance = new double[Clust, Coordinates];
            Means = new double[Pair];
            CovarianceMatrix = new double[Pair, Pair];
            iCovarianceMatrix = new double[Pair, Pair];
            Check = 0;
            EXIT = 1;

            for (int j = 0; j < Coordinates; j++)       //랜덤으로 소속도 만들기
            {
                Check = 0;
                for (int i = 0; i < Clust; i++)
                {
                    if (Check == 0)
                    {
                        if (i == Clust - 1)
                        {
                            MemberShip[i, j] = 1;
                        }
                        else
                        {
                            MemberShip[i, j] = r.Next(0, 2);
                        }
                        if (MemberShip[i, j] == 1)
                        {
                            Check = 1;
                        }//end if
                    }//end if
                }//end for
            }//end for

            for (int i = 0; i < Coordinates; i++)       //0~데이터개수의 루트??의 수미만까지 랜덤으로 position 할당
            {
                for (int j = 0; j < Pair; j++)
                {
                    Position[i, j] = r.Next(0, (int)Math.Sqrt(Coordinates));
                    //Console.Write(Position[i, j] + " ");
                }
                //Console.WriteLine();
            }

            //Mahalanobis Distance
            for (int j = 0; j < Pair; j++)
            {
                for (int i = 0; i < Coordinates; i++)
                {
                    Means[j] += Position[i, j];
                }
                Means[j] = Math.Round((Means[j] / Coordinates), 2);     //position평균 소수2째자리까지
                //Console.WriteLine(Means[j]);
            }
            for (int i = 0; i < Coordinates; i++)   //공분산행렬
            {
                CovarianceMatrix[0, 0] += Position[i, 0] - Means[0];
                CovarianceMatrix[0, 1] += (Position[i, 0] - Means[0]) * (Position[i, 1] - Means[1]);
                CovarianceMatrix[1, 0] += (Position[i, 0] - Means[0]) * (Position[i, 1] - Means[1]);
                CovarianceMatrix[1, 1] += Position[i, 1] - Means[1];

            }
            for (int i = 0; i < Pair; i++)
            {
                for (int j = 0; j < Pair; j++)
                {
                    CovarianceMatrix[i, j] = CovarianceMatrix[i, j] / Coordinates;
                }
            }
            //end

            tempNum = 1 / ((CovarianceMatrix[0, 0] * CovarianceMatrix[1, 1]) - (CovarianceMatrix[0, 1] * CovarianceMatrix[1, 0]));
            iCovarianceMatrix[0, 0] = tempNum * CovarianceMatrix[1, 1];
            iCovarianceMatrix[0, 1] = -tempNum * CovarianceMatrix[0, 1];
            iCovarianceMatrix[1, 0] = -tempNum * CovarianceMatrix[1, 0];
            iCovarianceMatrix[1, 1] = tempNum * CovarianceMatrix[0, 0];
            //Console.WriteLine("X평균값: {0} Y평균값: {1}", Means[0], Means[1]);

            double[,] SVector = new double[Clust, Pair];
            double[,] MVector = new double[Clust, Pair];
            double[,] Cvector = new double[Clust, Pair];    //중심값


            //Console.WriteLine("Do Clust");
            do
            {
                //Vector Initailize();
                for (int k = 0; k < Clust; k++)
                {
                    for (int j = 0; j < Pair; j++)
                    {
                        SVector[k, j] = 0;
                        MVector[k, j] = 0;
                        Cvector[k, j] = 0;
                    }
                }                

                for (int k = 0; k < Clust; k++)     //중심값 구하기
                {
                    for (int j = 0; j < Pair; j++)
                    {
                        for (int i = 0; i < Coordinates; i++)
                        {
                            MVector[k, j] += Math.Round(Math.Pow(MemberShip[k, i], 2), 2);
                            SVector[k, j] += Math.Round(Math.Pow(MemberShip[k, i], 2) * Position[i, j], 2);
                        }
                        Cvector[k, j] = Math.Round(SVector[k, j] / MVector[k, j], 2);
                    }
                }



                for (int j = 0; j < Clust; j++)     //유클리드 거리 계산
                {
                    for (int i = 0; i < Coordinates; i++)
                    {
                        Distance[j, i] = Math.Round(Math.Sqrt(Math.Pow(Position[i, 0] - Cvector[j, 0], 2) + Math.Pow(Position[i, 1] - Cvector[j, 1], 2)), 2);
                    }
                }

                // ----------------------------소속도 초기화----------------------------------
                for (int i = 0; i < Clust; i++)
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        NMemberShip[i, j] = 0;
                    }
                }
                //-------------------------------초기화 완료-----------------------------------------------

                for (int i = 0; i < Clust; i++)
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        for (int k = 0; k < Clust; k++)
                        {
                            NMemberShip[i, j] = NMemberShip[i, j] + Math.Pow(Distance[i, j] / Distance[k, j], 2);       // ????
                            if (NMemberShip[i, j].Equals(Double.NaN))
                            {
                                NMemberShip[i, j] = 1;
                            }
                        }
                    }
                }
                                
                for (int i = 0; i < Clust; i++)
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        NMemberShip[i, j] = Math.Round(1 / NMemberShip[i, j], 3);
                    }
                }

                Max = 0;
                
                for (int i = 0; i < Clust; i++)     //오차 범위??
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        if (Max < Math.Abs(MemberShip[i, j] - NMemberShip[i, j]))
                        {
                            Max = Math.Abs(MemberShip[i, j] - NMemberShip[i, j]);
                        }
                    }
                }
                //Console.WriteLine("Max: " + Max);

                if (Max < 0.01)
                {
                    EXIT = 0;
                }
                else
                {
                    for (int i = 0; i < Clust; i++)
                    {
                        for (int j = 0; j < Coordinates; j++)
                        {
                            MemberShip[i, j] = NMemberShip[i, j];
                        }//end for
                    }//end for
                    //Console.WriteLine("-------------------------------------------------Restart");
                }//end else
            } while (EXIT != 0);
            //Console.WriteLine("-----------------------------------------------------------------");

            for (int i = 0; i < Clust; i++)
            {
                for (int j = 0; j < Coordinates; j++)
                {
                    //Console.WriteLine(MemberShip[i, j]);
                }//end for
                //Console.WriteLine();
                //Console.WriteLine();
                //Console.WriteLine();
                //Console.WriteLine();
            }//end for

            for (int i = 0; i < Clust; i++)
            {
                //Console.WriteLine("{0}번째 클러스터 중심 벡터 Vector", i + 1);
                for (int j = 0; j < Pair; j++)
                {
                    //Console.WriteLine(Cvector[i, j]);
                }//end for
            }//end for
           // Console.WriteLine("-----------------------------------------------------------------");

            EXIT = 1;
            /*
            do      //왜 유클리드하고 또 마할라노비스로 거리계산해서 한번더 구함?????
            {
                //Vector Initailize();
                for (int k = 0; k < Clust; k++)
                {
                    for (int j = 0; j < Pair; j++)
                    {
                        SVector[k, j] = 0;
                        MVector[k, j] = 0;
                        Cvector[k, j] = 0;
                    }
                }                

                for (int k = 0; k < Clust; k++)
                {
                    for (int j = 0; j < Pair; j++)
                    {
                        for (int i = 0; i < Coordinates; i++)
                        {
                            MVector[k, j] += Math.Round(Math.Pow(MemberShip[k, i], 2), 2);
                            SVector[k, j] += Math.Round(Math.Pow(MemberShip[k, i], 2) * Position[i, j], 2);
                        }
                        Cvector[k, j] = SVector[k, j] / MVector[k, j];
                    }
                }

                double x, y;

                for (int j = 0; j < Clust; j++)     //마할라노비스 거리 계산
                {
                    for (int i = 0; i < Coordinates; i++)
                    {
                       // Distance[j, i] = Math.Round(Math.Sqrt(Math.Pow(Position[i, 0] - Cvector[j, 0], 2) + Math.Pow(Position[i, 1] - Cvector[j, 1], 2)), 2);
                        x = Position[i, 0] - Cvector[j, 0];
                        y = Position[i, 1] - Cvector[j, 1];
                        Distance[j, i] = Math.Round((iCovarianceMatrix[0, 0] * x + iCovarianceMatrix[1, 0] * y) * x + (iCovarianceMatrix[0, 1] * x + iCovarianceMatrix[1, 1] * y) * y,2);
                    }
                }

                // ----------------------------소속도 초기화----------------------------------
                for (int i = 0; i < Clust; i++)
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        NMemberShip[i, j] = 0;
                    }
                }
                //-------------------------------초기화 완료-----------------------------------------------

                for (int i = 0; i < Clust; i++)
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        for (int k = 0; k < Clust; k++)
                        {
                            NMemberShip[i, j] = NMemberShip[i, j] + Math.Pow(Distance[i, j] / Distance[k, j], 2);
                            if (NMemberShip[i, j].Equals(Double.NaN))
                            {
                                NMemberShip[i, j] = 1;
                            }
                        }
                    }
                }

                for (int i = 0; i < Clust; i++)
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        NMemberShip[i, j] = Math.Round(1 / NMemberShip[i, j], 3);
                    }
                }

                Max = 0;

                for (int i = 0; i < Clust; i++)
                {
                    for (int j = 0; j < Coordinates; j++)
                    {
                        if (Max < Math.Abs(MemberShip[i, j] - NMemberShip[i, j]))
                        {
                            Max = Math.Abs(MemberShip[i, j] - NMemberShip[i, j]);
                        }
                    }
                }
                //Console.WriteLine("Max: " + Max);

                if (Max < 0.01)
                {
                    EXIT = 0;
                }
                else
                {
                    for (int i = 0; i < Clust; i++)
                    {
                        for (int j = 0; j < Coordinates; j++)
                        {
                            MemberShip[i, j] = NMemberShip[i, j];
                        }//end for
                    }//end for
                    //Console.WriteLine("-------------------------------------------------Restart");
                }//end else
            } while (EXIT != 0);
            //Console.WriteLine("-----------------------------------------------------------------");
            */

            for (int i = 0; i < Clust; i++)
            {
                for (int j = 0; j < Coordinates; j++)
                {
                    //Console.WriteLine( MemberShip[i, j]);
                }//end for
                //Console.WriteLine();
                //Console.WriteLine();
                //Console.WriteLine();
                //Console.WriteLine();
            }//end for
            for (int i = 0; i < Clust; i++)
            {
                //Console.WriteLine("{0}번째 클러스터 중심 벡터 Vector", i + 1);
                for (int j = 0; j < Pair; j++)
                {
                    Console.WriteLine(Cvector[i, j]);
                }//end for
            }//end for
            //Console.WriteLine("-----------------------------------------------------------------");
        }
    }
}
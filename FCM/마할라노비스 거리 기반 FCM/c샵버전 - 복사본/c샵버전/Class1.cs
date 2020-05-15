using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace c샵버전
{
    class Class1
    {
        StringBuilder sb = new StringBuilder();
        public static double[] array = new double[Coordinates * 3];
        Random r = new Random();
        public const int Clust = 3;
        public const int Coordinates = 700;
        const int Pair = 2;
        double Max = 0, tempNum;
        int Check = 0, EXIT = 1;
        public static double[,] MemberShip = new double[Clust, Coordinates];
        public static double[,] NMemberShip = new double[Clust, Coordinates];
        public static double[,] Position = new double[Coordinates, Pair];
        public static double[,] Distance = new double[Clust, Coordinates];
        public static double[] Means = new double[Pair];
        public static double[,] CovarianceMatrix = new double[Pair, Pair];
        public static double[,] iCovarianceMatrix = new double[Pair, Pair];
        public double[,] SVector = new double[Clust, Pair];
        public double[,] MVector = new double[Clust, Pair];
        public static double[,] Cvector = new double[Clust, Pair];    //중심값
        public static int[,] cw = new int[Coordinates, Clust];

        public String run()
        {
            int count = 0;
            FileStream fp11 = new FileStream("C:\\Users\\hopo5\\Desktop\\아톰\\퍼지\\test11(700).txt", FileMode.Open);
            StreamReader fp1 = new StreamReader(fp11);
            string line;

            while ((line = fp1.ReadLine()) != null)
            {
                array[count] = Convert.ToDouble(line);
                //스트링으로받아온데이터를 double형으로바꿔준다
                count++;
            }

            count = 0;

            for (int i = 0; i < Coordinates; i++)
            {
                for (int j = 0; j < Pair; j++)
                {
                    Position[i, j] = array[count];
                    //sb.AppendLine("x :" + "[" + i + "]" + "[" + j + "]" + " = " + x[i, j]);
                    count++;
                }
            }

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

            //int temp;
            //for (int i = 0; i < Coordinates; i++)
            //{
            //    for (int j = 0; j < Pair; j++)
            //    {
            //        if (j == 0)
            //        {
            //            temp = r.Next(30, 41);
            //            Position[i, j] = temp * 0.1f;
            //        }
            //        else
            //        {
            //            temp = r.Next(10, 71);
            //            Position[i, j] = temp * 0.1f;
            //        }
            //        //Position[i, j] = r.Next(0, (int)Math.Sqrt(DATA));
            //        //sb.Append(Position[i, j] + " ");
            //    }
            //    //sb.AppendLine();
            //}

            //Mahalanobis Distance
            for (int j = 0; j < Pair; j++)
            {
                for (int i = 0; i < Coordinates; i++)
                {
                    Means[j] += Position[i, j];
                }
                Means[j] = Math.Round((Means[j] / Coordinates), 2);     //position평균 소수2째자리까지
                //sb.AppendLine(Means[j]);
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
            //sb.AppendLine("X평균값: {0} Y평균값: {1}", Means[0], Means[1]);
            
            
            //sb.AppendLine("Do Clust");
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
                //sb.AppendLine("Max: " + Max);

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
                    //sb.AppendLine("-------------------------------------------------Restart");
                }//end else
            } while (EXIT != 0);
            //sb.AppendLine("-----------------------------------------------------------------");

            /*
            sb.AppendLine("Position-----------------------------------------------------------------");
            for (int i = 0; i < Coordinates; i++)
            {
                for (int j = 0; j < Pair; j++)
                {
                    sb.Append(Position[i, j] + ",");
                }
                sb.AppendLine();
            }

            sb.AppendLine("MemberShip-----------------------------------------------------------------");
            for (int i = 0; i < Coordinates; i++)
            {
                for (int j = 0; j < Clust; j++)
                {
                    sb.Append(MemberShip[j, i] + "/");
                }
                sb.AppendLine();
            }
            */

            sb.AppendLine("중심-----------------------------------------------------------------");
            for (int i = 0; i < Clust; i++)
            {
                //sb.AppendLine("{0}번째 클러스터 중심 벡터 Vector", i + 1);
                for (int j = 0; j < Pair; j++)
                {
                    sb.Append(Cvector[i, j] + "\t");
                }//end for
                sb.AppendLine();
            }
            
            sb.AppendLine("-----------------------------------------------------------------");
            
            EXIT = 1;            
            
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
                        Distance[j, i] = Math.Round((iCovarianceMatrix[0, 0] * x + iCovarianceMatrix[1, 0] * y) * x + (iCovarianceMatrix[0, 1] * x + iCovarianceMatrix[1, 1] * y) * y, 2);
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
                //sb.AppendLine("Max: " + Max);                

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
                    //sb.AppendLine("-------------------------------------------------Restart");
                }//end else
            } while (EXIT != 0);
            
            Max = 0;  
            for (int i = 0; i < Coordinates; i++)
            {
                Max = 0;
                count = 0;
                for (int j = 0; j < Clust; j++)
                {
                    if (j == 0)
                    {
                        Max = MemberShip[j, i];
                    }
                    else if (j > 0 && (MemberShip[j, i] > Max))
                    {
                        Max = MemberShip[j, i];
                        count = j;
                    }
                }
                cw[i, count] = 1;
            }
            
            /*
            sb.AppendLine("Position-----------------------------------------------------------------");
            for(int i=0; i<Coordinates; i++)
            {
                for(int j=0; j<Pair; j++)
                {
                    sb.Append(Position[i, j] + ",");
                }
                sb.AppendLine();
            }

            sb.AppendLine("MemberShip-----------------------------------------------------------------");
            for(int i=0; i<Coordinates; i++)
            {
                for(int j=0; j<Clust; j++)
                {
                    sb.Append(MemberShip[j, i] + "/");
                }
                sb.AppendLine();
            }
            */
            sb.AppendLine("중심-----------------------------------------------------------------");
            for (int i = 0; i < Clust; i++)
            {
                //sb.AppendLine("{0}번째 클러스터 중심 벡터 Vector", i + 1);
                for (int j = 0; j < Pair; j++)
                {
                    sb.Append(Cvector[i, j] + "\t");
                }//end for
                sb.AppendLine();
            }//end for
            sb.AppendLine("-----------------------------------------------------------------");    
            
            return sb.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kmwf
{
    public partial class Form1 : Form
    {
        k_means k;
        Bitmap bitmap;
        Image image;
        Graphics gr;
        public Form1()
        {         
            InitializeComponent();
            this.Size = new Size(1060, 500);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string openstrFilename;

            openFileDialog1.Title = "이미지 읽기";
            openFileDialog1.Filter = "All Files(*.*)|*.*|Bitmap File(*.bmp)|*.bmp|JPEG File(*.jpg)|*.jpg";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openstrFilename = openFileDialog1.FileName;
                image = Image.FromFile(openstrFilename);
                this.pictureBox1.Image = image;
                bitmap = new Bitmap(image);
            }
        }      
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        public class k_means
        {
            public Bitmap start(Bitmap gBitmap)
            {
                #region Declaration
                int cluster = 10;
                int kcnt = 0;
                int[] k = new int[cluster];
                int[] Dcount = new int[cluster];
                int[] old_k = new int[cluster];
                int[] Dsum = new int[cluster];
                Color color;
                bool check = true;

                int[,] bit = new int[gBitmap.Width, gBitmap.Height];
                int[,] g = new int[gBitmap.Width, gBitmap.Height];

                for (int x = 0; x < gBitmap.Width; x++)
                    for (int y = 0; y < gBitmap.Height; y++)
                    {
                        color = gBitmap.GetPixel(x, y);
                        bit[x, y] = color.R;
                    }
                #endregion
                #region initialize

                Random r = new Random();                
                
                for (int i = 0; i < cluster; i++)
                {
                    k[i] = r.Next(0, 255);  
                    old_k[i] = -1;
                }

                for (int i = 0; i < cluster; i++)
                {
                    Dcount[i] = 0;
                    Dsum[i] = 0;
                }

                #endregion

                #region Distance
                while (check)
                {
                    for (int x = 0; x < gBitmap.Width; x++)
                        for (int y = 0; y < gBitmap.Height; y++)
                        {
                            int count = 0;
                            int min = 256;

                            for (int i = 0; i < cluster; i++)
                            {
                                if (min > Math.Abs(k[i] - bit[x, y]))
                                {
                                    min = Math.Abs(k[i] - bit[x, y]);
                                    g[x, y] = count;
                                }
                                count++;
                            }
                            Dsum[g[x, y]] += bit[x, y];
                            Dcount[g[x, y]]++;

                        }

                    for (int i = 0; i < cluster; i++)
                    {
                        if (Dcount[i] == 0)
                            k[i] = 0;
                        else
                            k[i] = (int)(Dsum[i] / Dcount[i]);
                    }

                    for (int i = 0; i < cluster; i++)
                    {
                        if (k[i] == old_k[i])
                            kcnt++;
                    }

                    if (kcnt >= cluster)
                    {
                        break;
                    }
                    else
                    {
                        for (int i = 0; i < cluster; i++)
                        {
                            old_k[i] = k[i];
                            Dcount[i] = 0;
                            Dsum[i] = 0;
                        }
                        Array.Sort(k);
                        Array.Sort(old_k);
                    }

                }
                #endregion
                #region Quantization
                for (var i = 0; i < gBitmap.Width; i++)
                    for (var j = 0; j < gBitmap.Height; j++)
                        switch (g[i, j])
                        {
                            case 0:
                                gBitmap.SetPixel(i, j, Color.Black);
                                break;
                            case 1:
                                gBitmap.SetPixel(i, j, Color.FromArgb(255, 100, 0)); //주황
                                break;
                            case 2:
                                gBitmap.SetPixel(i, j, Color.FromArgb(200, 255, 0)); //노랑
                                break;
                            case 3:
                                gBitmap.SetPixel(i, j, Color.FromArgb(100, 255, 0)); //연두
                                break;
                            case 4:
                                gBitmap.SetPixel(i, j, Color.FromArgb(0, 255, 0)); // 초록
                                break;
                            case 5:
                                gBitmap.SetPixel(i, j, Color.FromArgb(0, 255, 100)); // 연두
                                break;
                            case 6:
                                gBitmap.SetPixel(i, j, Color.FromArgb(0, 255, 255)); // 하늘
                                break;
                            case 7:
                                gBitmap.SetPixel(i, j, Color.FromArgb(0, 100, 255)); // 연파랑
                                break;
                            case 8:
                                gBitmap.SetPixel(i, j, Color.FromArgb(0, 0, 255)); //  파랑
                                break;
                            case 9:
                                gBitmap.SetPixel(i, j, Color.FromArgb(100, 0, 255)); //  보라                             
                                break;
                        }
                #endregion
                return gBitmap;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            k = new k_means();
            bitmap = k.start(bitmap);   
            pictureBox2.Image = bitmap;
        }
    }    
}

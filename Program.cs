using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

namespace KB
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;
using System.Text;

namespace KB
{
    class Program
    {
        static void Main(string[] args)
        {
            var openText = "11112222333344445555666677770000ffffeeeeddddccccbbbbaaaa99998888";
            var key = "2244a12922392521577cdc081bca5e2c6e022ace5d201c0f04b19a13781fe09d";
            var alpha = "0000ffff";
            //var cipher = new KB_256(key, alpha);
            //cipher.AddText(openText);
            //for (var i = 0; i < 16; i++)
            //{
            //    cipher.Round();
            //    cipher.PrintRound(16);
            //}

            //var str = Equations.GenText();
            //var str = "0b466272ce97a3e5abd61ceb250da188160db42b797c0332c8697ea3c730297b";
            //  Console.WriteLine(str);
            //PaintLinearyMatrixes(str); //Матрицы перемешивания на практике
            //PaintLinearyMatrixes_Multi(str); //Матрицы перемешивания через перемножение первого раунда
            //for (int i = 0; i < 16; i++) //Нахождение экспонента
            //{
            //    var x = CheckRoundLinear(i);
            //    Console.WriteLine(String.Format("Перемешивающиая матрица {0} раунда : {1}", i + 1, x));
            //}
            //for (int i = 0; i < 16; i++)
            //{
            //    var x = CheckRoundUnlinear(i); //Нахождение 2-экспонента
            //    Console.WriteLine(String.Format("Нелинейная матрица {0} раунда : {1}", i + 1, x));
            //}
            var r_7_50000 = CheckRoundLinear(7);
            Console.WriteLine(r_7_50000);
            var Sum_5 = new SumSomeVectors_mt(5).GetBitmap();
            Sum_5.Save("C:/Users/a.gryaznov/source/repos/KB/Elements/SumVect_5.bmp");
            var Sum_Key = new SumSomeVectors_mt(1).GetBitmap();
            Sum_Key.Save("C:/Users/a.gryaznov/source/repos/KB/Elements/SumVect_Key.bmp");
            var Xor_End = new XorSomeVectors_mt(2).GetBitmap();
            Xor_End.Save("C:/Users/a.gryaznov/source/repos/KB/Elements/Xor_End.bmp");
            var Shift_11 = new Shifting_mt(11).GetBitmap();
            Shift_11.Save("C:/Users/a.gryaznov/source/repos/KB/Elements/Shifting_11.bmp");

        }
        public static void PaintLinearyMatrixes(string str)
        {
            var valMat = new ValMat(str);
            valMat.MakeAllMatrix();
            for (int i = 0; i < 16; i++)
            {
                var bit = CreateTemplate(256,256); // ширина - количество столбцов, высота - количество строк
                for (int j = 0; j < 256; j++)
                {
                    for (int r = 0; r < 256; r++)
                    {
                        if (valMat.Rounds[i][r, j] == 1) 
                            FillSquare(bit,j*5,r*5,Color.Green);
                    }
                }
                bit.Save(String.Format("C:/Users/a.gryaznov/source/repos/KB/valMatrixes/{0} Раунд.bmp", i+1));
            }
        }
        public static void PaintLinearyMatrixes_Multi(string str)
        {
            var valMat = new ValMat(str);
            valMat.MakeAllMatrix();
            Matrix<double>[] Rounds = new Matrix<double>[16];
            Rounds[0] = valMat.Rounds[0];
            for (int i = 1; i < 16; i++)
            {
                Rounds[i] = Matrix<double>.Build.Dense(256, 256, 0);
            }
            for (int i = 1; i < 16; i++)
            {
                Rounds[i] = Rounds[i - 1] * Rounds[0];
            }
            for (int i = 0; i < 16; i++)
            {
                var bit = CreateTemplate(256, 256); // ширина - количество столбцов, высота - количество строк
                for (int j = 0; j < 256; j++)
                {
                    for (int r = 0; r < 256; r++)
                    {
                        if (Rounds[i][r, j] >= 1)
                            FillSquare(bit, j * 5, r * 5, Color.Green);
                    }
                }
                bit.Save(String.Format("C:/Users/a.gryaznov/source/repos/KB/valMatrixes_X/valMat_{0}.bmp", i+1));
            }
        }
        public static Bitmap CreateTemplate(int columns, int rows)
        {
            var bit = new Bitmap(columns*5+1,rows*5+1);
            var h = 0;
            for (int i = 0; i < bit.Width; i++)
            {
                for (int j = 0; j < bit.Height; j++)
                {
                    bit.SetPixel(i, j, Color.White);
                }
            }

            while (h < bit.Height)
            {
                for (int i = 0; i < bit.Width; i++)
                {
                    bit.SetPixel(i, h, Color.Gray);
                  
                }
                h += 5;
            }
            for (int i = 0; i < bit.Width; i++)
            {
                bit.SetPixel(i, bit.Height - 1, Color.Gray);
            }
            h = 0;
            while (h < bit.Width)
            {
                for (int i = 0; i < bit.Height; i++)
                {
                    bit.SetPixel(h,i, Color.Gray);

                }
                h += 5;
            }
            for (int i = 0; i < bit.Height; i++)
            {
                bit.SetPixel(bit.Width - 1,i, Color.Gray);
            }
            return bit;
        }
        public static void FillSquare(Bitmap bit,int x, int y, Color color)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bit.SetPixel(x+1+i, y+1+j, color);
                }
            }
        }
        public static string[] GenerateString(int numberOfBit)
        {
            var x_1 = new StringBuilder();
            var x_2 = new StringBuilder();
            var rnd = new Random();
            for (int k = 0; k < 256; k++)
            {
                var next = rnd.Next(0, 2);
                if (k == numberOfBit)
                {
                    x_1.Append(0);
                    x_2.Append(1);
                }
                else
                {
                    x_1.Append(next);
                    x_2.Append(next);
                }
            }
            var test = x_1.ToString().Length;
            var X_1 = new StringBuilder();
            var X_2 = new StringBuilder();
            for (int i = 0;  i< 8; i++)
            {
                var x = Convert.ToString(Equations.ToInt32_10(x_1.ToString().Substring(0 + 32 * i, 32), 2), 16);
                while(x.Length!=8)
                {
                    x = "0" + x;
                }
                X_1.Append(x);
                x = Convert.ToString(Equations.ToInt32_10(x_2.ToString().Substring(0 + 32 * i, 32), 2), 16);
                while (x.Length != 8)
                {
                    x = "0" + x;
                }
                X_2.Append(x);
            }
            var test_1 = X_1.ToString().Length;
            return new string[] {X_1.ToString(),X_2.ToString()};
        }
        public static bool CheckRoundLinear(int numberOfRound)
        {
            var key = "2244a12922392521577cdc081bca5e2c6e022ace5d201c0f04b19a13781fe09d";
            var alpha = "0000ffff";
            var boolIsFull = new bool[256];
            var allAcc = new uint[256][];
            var acc = new uint[8];
            var cipher = new KB_256(key, alpha);
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    var strs = GenerateString(i);
                    cipher.AddText(strs[0]);
                    for (int r = 0; r < numberOfRound; r++)
                    {
                        cipher.Round();
                    }
                    var round_1 = cipher.GetRound();
                    cipher.AddText(strs[1]);
                    for (int r = 0; r < numberOfRound; r++)
                    {
                        cipher.Round();
                    }
                    var round_2 = cipher.GetRound();
                    var xor = new uint[8];
                    for (int h = 0; h < 8; h++)
                    {
                        xor[h] = round_1[h] ^ round_2[h];
                        acc[h] |= xor[h];
                    }
                    var count = 0;
                    for (int h = 0; h < 8; h++)
                    {
                        if (acc[h] == uint.MaxValue)
                        {
                            count++;
                        }
                    }
                    if (count == 8)
                    {
                        boolIsFull[i] = true;
                        allAcc[i] = acc;
                        break;
                    }
                    else if (j == 499)
                    {
                        allAcc[i] = xor;
                    }

                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (!boolIsFull[i])
                    return false;
            } 
            return true;
        }
        public static bool CheckRoundUnlinear(int numberOfRound)
        {
            var key = "2244a12922392521577cdc081bca5e2c6e022ace5d201c0f04b19a13781fe09d";
            var alpha = "0000ffff";
            var boolIsFull = new bool[256];
            var allAcc = new uint[256][];
            var acc = new uint[8];
            var cipher = new KB_256(key, alpha);
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 500; j++)
                {
                    var strs = GenerateString(i);
                    cipher.AddText(strs[0]);
                    for (int r = 0; r < numberOfRound; r++)
                    {
                        cipher.Round();
                    }
                    var round_1 = cipher.GetRound();
                    cipher.AddText(strs[1]);
                    for (int r = 0; r < numberOfRound; r++)
                    {
                        cipher.Round();
                    }
                    var round_2 = cipher.GetRound();
                    var xor = new uint[8];
                    for (int h = 0; h < 8; h++)
                    {
                        xor[h] = round_1[h] & round_2[h];
                        acc[h] |= xor[h];
                    }
                    var count = 0;
                    for (int h = 0; h < 8; h++)
                    {
                        if (acc[h] == uint.MaxValue)
                        {
                            count++;
                        }
                    }
                    if (count == 8)
                    {
                        boolIsFull[i] = true;
                        allAcc[i] = acc;
                        break;
                    }
                    else if (j == 499)
                    {
                        allAcc[i] = xor;
                    }

                }
            }
            for (int i = 0; i < 256; i++)
            {
                if (!boolIsFull[i])
                    return false;
            }
            return true;
        }

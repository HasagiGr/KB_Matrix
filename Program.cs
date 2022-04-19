using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

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

           // var str = Equations.GenText();
            var str = "0b466272ce97a3e5abd61ceb250da188160db42b797c0332c8697ea3c730297b";
              Console.WriteLine(str);
            PaintLinearyMatrixes(str);
            PaintLinearyMatrixes_Multi(str);
        }

        public static void PaintLinearyMatrixes(string str)
        {
            var valMat = new ValMat(str);
            valMat.MakeAllMatrix();
            for (int i = 0; i < 16; i++)
            {
                var bit = new Bitmap(258, 258); // ширина - количество столбцов, высота - количество строк
                for (int j = 1; j < 257; j++)
                {
                    for (int r = 1; r < 257; r++)
                    {
                        if (valMat.Rounds[i][r - 1, j - 1] == 1)
                            bit.SetPixel(j, r, Color.Green);
                        else
                            bit.SetPixel(j, r, Color.White);
                    }
                }
                for (int h = 0; h < 258; h++)
                {
                    bit.SetPixel(0, h, Color.Black);
                    bit.SetPixel(257, h, Color.Black);
                    bit.SetPixel(h, 0, Color.Black);
                    bit.SetPixel(h, 257, Color.Black);
                }
                bit.Save(String.Format("C:/Users/a.gryaznov/source/repos/KB/valMatrixes/valMat_{0}.bmp", i+1));
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
                var bit = new Bitmap(258, 258); // ширина - количество столбцов, высота - количество строк
                for (int j = 1; j < 257; j++)
                {
                    for (int r = 1; r < 257; r++)
                    {
                        if (Rounds[i][r - 1, j - 1] >= 1)
                            bit.SetPixel(j, r, Color.Green);
                        else
                            bit.SetPixel(j, r, Color.White);
                    }
                }
                for (int h = 0; h < 258; h++)
                {
                    bit.SetPixel(0, h, Color.Black);
                    bit.SetPixel(257, h, Color.Black);
                    bit.SetPixel(h, 0, Color.Black);
                    bit.SetPixel(h, 257, Color.Black);
                }
                bit.Save(String.Format("C:/Users/a.gryaznov/source/repos/KB/valMatrixes_X/valMat_{0}.bmp", i+1));
            }
        }
    }

}

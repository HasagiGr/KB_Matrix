using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace KB
{
    public class ValMat
    {

        private readonly string OriginalKey = "2244a12922392521577cdc081bca5e2c6e022ace5d201c0f04b19a13781fe09d";
        private readonly string Alpha = "ae6903ce";
        private string[] BaseStrings = new string[16];
        private KB_256 KB_256 { get; set; }
        public Matrix<double>[] Rounds = new Matrix<double>[16];
        public string[] texts = new string[257];
        private string[][] enchStrings { get; set; }

        public ValMat(string basic) //строка в 16-ричном виде
        {
            texts[0] = basic;
            for (int i = 0; i < 16; i++)
            {
                Rounds[i] = Matrix<double>.Build.Dense(256, 256, 0);
            }
            KB_256 = new KB_256(OriginalKey, Alpha);
            this.enchStrings = new string[256][];
            CreateBaseStrings();
            MakeTexts();
            CreateAllStrings();
        }

        public void MakeAllMatrix()
        {
            for (int i = 0; i < 16; i++)
            {
                CreateMatrix(i);
            }
        }
        private void CreateBaseStrings()
        {
            BaseStrings = CreateEncStrings(texts[0]);
        }
        private void CreateAllStrings()
        {
            for (int i = 0; i < 256; i++)
            {
                var str = CreateEncStrings(texts[i + 1]);
                this.enchStrings[i] =str;
            }
        }
        public void MakeTexts() //создание 256 строк, где каждый из которых отличается от первоначальной строки одним битом одним битом (представление в 16-ричном виде)
        {
            var original = Equations.BlocksToInt32(Equations.DivideToBlocks(texts[0], 8), 8);
            var count = 1;
            for (int i = 0; i < 8; i++)
            {
                var massive = new uint[8];
                for (int j = 0; j < 8; j++)
                {
                    if (j != i)
                        massive[j] = original[j];

                }
                for (int j = 0; j < 32; j++)
                {
                    var massive_I = massive;
                    massive_I[i] = original[i] ^ (uint)Math.Pow(2, 31 - j);
                    var str = new StringBuilder();
                    for (int r = 0; r < 8; r++)
                    {
                        var a = Convert.ToString(massive_I[r], 16);
                        while (a.Length != 8)
                        {
                            a = "0" + a;
                        }
                        str.Append(a);
                    }
                    texts[count] = str.ToString();
                    count++;
                }
            }
        }
        public string[] CreateEncStrings(string baseString)
        {
            var encStrings = new string[16];
            KB_256.AddText(baseString);
            for (int i = 0; i < 16; i++)
            {
                KB_256.Round();
                var massive = KB_256.GetRound();
                var str = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    var a = Convert.ToString(massive[j], 2);
                    while (a.Length != 32)
                    {
                        a = "0" + a;
                    }
                    str.Append(a);
                }
                encStrings[i] = str.ToString();
            }
            return encStrings;
        }
        private void CreateMatrix(int numberOfMatrix)
        {
            var currentStrings = new string[256];
            double[][] massive = new double[256][];
            for (int i = 0; i < 256; i++)
            {
                var currentMassive = new double[256];
                currentStrings[i] = this.enchStrings[i][numberOfMatrix];
                for (int j = 0; j < 256; j++)
                {
                    if (BaseStrings[numberOfMatrix][j] == currentStrings[i][j])
                    {
                        currentMassive[j] = 0;
                    }
                    else
                        currentMassive[j] = 1;
                }
                massive[i] = currentMassive;
            }
            this.Rounds[numberOfMatrix] = Matrix<double>.Build.DenseOfColumnArrays(massive);
        }
    }
}

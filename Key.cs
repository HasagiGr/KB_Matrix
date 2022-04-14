using System;
using System.Collections.Generic;
using System.Text;

namespace KB
{
    public class Key
    {
        public string OriginalKey { get; private set; }

        public uint LKGBeginnerKey { get; set; }

        public uint[] LKGAllSequence { get; set; }

        public uint[] GRKAllSequence { get; set; }

        public uint Alpha { get; private set; }

        public int Lenght { get; set; }
        //ключи для i-того раунда преобразования
        public int[][] Numbers = new int[][] { new int[] { 12, 14, 17 },
                                               new int[] { 19, 21, 23 },
                                               new int[] { 26, 29, 31 },
                                               new int[] { 34, 36, 40 },
                                               new int[] { 43, 45, 48 },
                                               new int[] { 51, 55, 56 },
                                               new int[] { 58, 60, 64 },
                                               new int[] { 67, 70, 72 },
                                               new int[] { 77, 79, 80 },
                                               new int[] { 83, 85, 86 },
                                               new int[] { 88, 90, 92 },
                                               new int[] { 95, 96, 99 },
                                               new int[] { 101, 102, 105 },
                                               new int[] { 107, 109, 112 },
                                               new int[] { 114, 115, 116 },
                                               new int[] { 118, 121, 123 }};


        public Key(string key, string alpha)
        {
            this.OriginalKey = key;
            this.Alpha = Equations.ToInt32_10(alpha, 16);
            this.Lenght = key.Length * 4;
            var l = (Lenght - 96) / 32 - 1;
            this.GRKAllSequence = new uint[128];
            var blocks = Equations.DivideToBlocks(key, 8);
            for (int i = 0; i < 7; i++)
            {
                this.GRKAllSequence[i] = Equations.ToInt32_10(blocks[i], 16);
            }
            this.LKGBeginnerKey = Equations.ToInt32_10(blocks[7], 16);
            SequenceOfLKG();
            SequenceOfGRK();
        }

        public void SequenceOfLKG()
        {
            var massive = new uint[128];
            massive[0] = LKGBeginnerKey;
            for (int i = 1; i < 128; i++)
            {
                massive[i] = Equations.GetSum(massive[i - 1], Alpha);
            }
            LKGAllSequence = massive;
        }
        public void SequenceOfGRK()
        {
            for (int i = 7; i < 128; i++)
            {
                var sum = Equations.GetSum(this.GRKAllSequence[i - 7], this.GRKAllSequence[i - 5], this.GRKAllSequence[i - 3], this.GRKAllSequence[i - 1]);
                var sumStr = Convert.ToString(sum, 2);
                while (sumStr.Length != 32)
                {
                    sumStr = "0" + sumStr;
                }
                sumStr = sumStr.Substring(1) + sumStr.Substring(0, 1);
                sum = Equations.ToInt32_10(sumStr, 2);
                this.GRKAllSequence[i] = Equations.GetSum(sum, this.LKGAllSequence[i - 7], Alpha);
            }
        }
        public void AllGRK() //Check GRK
        {
            var i = 1;
            foreach (var number in GRKAllSequence)
            {
                Console.WriteLine(String.Format("{0}: {1}", i, Convert.ToString(number, 16)));
                i++;
            }
        }
        public void AllLKG() //Check LKG
        {
            var i = 1;
            foreach (var number in LKGAllSequence)
            {
                Console.WriteLine(String.Format("{0}: {1}", i, Convert.ToString(number, 16)));
                i++;
            }
        }
    }
}

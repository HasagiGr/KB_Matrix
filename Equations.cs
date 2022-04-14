using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KB
{
    public class Equations
    {
        public static uint GetSum(params uint[] args) // сумма по модулю 2^(32)
        {
            uint sum = 0;
            foreach (var arg in args)
            {
                sum += arg;
            }
            return sum;
        }

        public static uint GetSumXOR(params uint[] args) //XOR нескольких 32 битных векторов 
        {
            uint sum = 0;
            foreach (var arg in args)
            {
                sum = sum ^ arg;
            }
            return sum;
        }

        public static uint[] BlocksToInt32(string[] blocks, int amount) // Отображение всех 16-ричных представлений
        {
            var blocksToInt32 = new uint[amount];
            for (int i = 0; i < amount; i++)
            {
                blocksToInt32[i] = ToInt32_10(blocks[i], 16);
            }
            return blocksToInt32;
        }

        public static uint ToInt32_10(string str, int h) //превращение строки с h-ричным числом в 10-ричное число()
        {
            uint number = 0;
            var strInt = str.Select(x => ConvertToInt(x))
                          .ToArray();
            for (var i = 0; i < strInt.Length; i++)
            {
                number += (uint)strInt[strInt.Length - 1 - i] * (uint)Math.Pow(h, i);
            }
            return number;
        }


        public static int[] GetInt32Func(string block)// Отображение строкового 16-ричного представления в 32 битные числа(вектора)
        {
            var endNumber = new int[32];
            for (var i = 0; i < 8; i++)
            {
                var currentBlock = From16to2(block[7 - i]);
                for (var j = 0; j < 4; j++)
                {
                    endNumber[j + 4 * i] = currentBlock[j];
                }
            }
            return endNumber;
        }

        public static int[] From16to2(char number) // Отображение элемента из 16-ричного представления в 4 битный вектор в 2-ичной
        {
            var numValue = ConvertToInt(number);
            int[] endNumber = new int[4];
            for (var i = 0; i < 4; i++)
            {
                endNumber[i] = numValue % 2;
                numValue /= 2;
            }
            return endNumber;
        }

        public static int[] From16to2(int number) // Отображение элемента из 16-ричного представления в 4 битный вектор в 2-ичной(для перевода номера Sbox-сов в сравнимый вид)
        {
            var numValue = number;
            int[] endNumber = new int[4];
            for (var i = 0; i < 4; i++)
            {
                endNumber[i] = numValue % 2;
                numValue /= 2;
            }
            return endNumber;
        }

        public static int ConvertToInt(char number) // Костыль чтобы сконвертить из 16 в 10
        {
            var numValue = Convert.ToInt32(number);
            if (numValue > 57)
                numValue -= 87;
            else
                numValue -= 48;
            return numValue;
        }
        public static string[] DivideToBlocks(string text, int amount) // разделение строчки на блоки
        {
            var blocks = new string[amount];
            for (int i = 0; i < amount; i++)
            {
                blocks[i] = text.Substring(i * 8, 8);
            }
            return blocks;
        }

        public static string Shifting(string str, int amount)
        {
            return str.Substring(amount) + str.Substring(0, amount);
        }

        public static bool IsEqual(int[] v1, int[] v2)
        {
            for (int i = 0; i < v1.Length; i++)
            {
                if (v1[i] != v2[i]) return false;
            }
            return true;
        }

        public static string GenText()
        {
            var str = new StringBuilder();
            var rnd = new Random();
            for (int i = 0; i < 64; i++)
            {
                var x = rnd.Next(0, 15);
                str.Append(Convert.ToString(x, 16));
            }
            return str.ToString();
        }
    }
}

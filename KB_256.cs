using System;
using System.Collections.Generic;
using System.Text;

namespace KB
{
    public class KB_256
    {
        public string OpenText { get; private set; }
        public string OriginalKey { get; private set; }
        public string Alpha { get; private set; }
        public Sbox SBox { get; private set; }
        public Key Keys { get; private set; }

        private uint[] CurrentText { get; set; }
        public int RoundCount { get; private set; } = 0;
        public int[] NumbersForXor = new int[] { 2, 5, 0 };

        public KB_256(string key, string alpha)
        {
            this.OriginalKey = key;
            this.Alpha = alpha;
            this.SBox = new Sbox();
            this.Keys = new Key(key, alpha);
        }

        public void AddText(string text)
        {
            this.OpenText = text;
            this.CurrentText = Equations.BlocksToInt32(Equations.DivideToBlocks(OpenText, 8), 8);
            this.RoundCount = 0;
        }
        public void Round()
        {
            var currentT = this.CurrentText;
            var sum = Equations.GetSum(currentT[1], currentT[3], currentT[4], currentT[6], currentT[7]);
            uint[] func = new uint[3];
            for (var i = 0; i < 3; i++)
            {
                var current = Equations.GetSum(sum, this.Keys.GRKAllSequence[this.Keys.Numbers[RoundCount][i]]);
                var str = Convert.ToString(current, 16);
                while (str.Length != 8)
                {
                    str = "0" + str;
                }
                string afterBox = null;
                for (int j = 0; j < 8; j++)
                {
                    afterBox += this.SBox.s_Box[j][str[j]];
                }
                var afterBoxInt = Equations.ToInt32_10(afterBox, 16);
                str = Convert.ToString(afterBoxInt, 2);
                while (str.Length != 32)
                {
                    str = "0" + str;
                }
                str = Equations.Shifting(str, 11);
                afterBoxInt = Equations.ToInt32_10(str, 2);
                func[i] = Equations.GetSumXOR(afterBoxInt, currentT[this.NumbersForXor[i]]);
            }
            CurrentText[0] = currentT[1];
            CurrentText[1] = func[0];
            CurrentText[2] = currentT[3];
            CurrentText[3] = currentT[4];
            CurrentText[4] = func[1];
            CurrentText[5] = currentT[6];
            CurrentText[6] = currentT[7];
            CurrentText[7] = func[2];

            RoundCount++;
        }
        public void PrintRound(int numeric)
        {
            Console.WriteLine(String.Format("Результат шифрования {0} раунда\n", this.RoundCount));
            foreach (var block in CurrentText)
            {
                Console.Write(String.Format("{0} ", Convert.ToString(block, numeric)));
            }
            Console.WriteLine();
        }
        public uint[] GetRound()
        {
            return CurrentText;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace KB
{
    public class Sbox //старшие биты идут с 0 индекса
    {
        private string box = "17ed05834fa69cb2" + //8:
                             "8e25691cf4b0da37" + //7
                             "5df692cab78143e0" + //6
                             "7f5a816d093eb42c" + //5
                             "c821d4f670a53e9b" + //4
                             "b3582fade174c960" + //3
                             "68239a5c1e47bd0f" + //2
                             "c462a5b9e8d703f1";  //1

        public char[] Numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public Dictionary<char, char>[] s_Box { get; set; }

        public Sbox()
        {
            var convert = box.Select(x => x)
                             .ToArray();
            s_Box = new Dictionary<char, char>[8];
            var number = 0;
            for (int i = 0; i < 8; i++)
            {
                s_Box[i] = new Dictionary<char, char>();
                for (int j = 0; j < 16; j++)
                {
                    s_Box[i].Add(Numbers[j], convert[number]);
                    number++;
                }
            }
        }
    }
}

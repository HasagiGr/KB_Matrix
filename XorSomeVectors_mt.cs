using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using System.Threading.Tasks;
using System.Drawing;
using static KB.Program;

namespace KB
{
    public class XorSomeVectors_mt
    {
        public Matrix<double> MatrixOfSum { get; private set; }
        public int NumberOfElements { get; private set; }
        private Bitmap Data { get; set; }
        public XorSomeVectors_mt(int number)
        {
            this.NumberOfElements = number;
            var mas = CreateMassives();
            MatrixOfSum = Matrix<double>.Build.DenseOfColumnArrays(mas);
            FillTemplate();
        }

        private double[][] CreateMassives()
        {
            var x = new double[32 * NumberOfElements][];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new double[32];
                for (int j = 0; j < 32; j++)
                {
                    var current = i % 32;
                    x[i][j] = 0;
                    if (current == j)
                        x[i][j] = 1;
                    else if (current > j)
                        x[i][j] = 2;
                }
            }
            return x;
        }
        private void FillTemplate()
        {
            var template = CreateTemplate(MatrixOfSum.ColumnCount, MatrixOfSum.RowCount);
            for (int i = 0; i < MatrixOfSum.ColumnCount; i++)
            {
                for (int j = 0; j < MatrixOfSum.RowCount; j++)
                {
                    if (MatrixOfSum[j, i] == 1)
                        FillSquare(template, i * 5, j * 5, Color.Green);       
                }
            }
            this.Data = template;
        }
        public Bitmap GetBitmap()
        {
            return Data;
        }
    }
}

using System;
using System.Linq;

namespace Lab_5
{
    public class Lab5
    {
        static public void Main_(string[] args)
        {
            double[][] Matrix = new double[][] {
                new double[] { 0.4, 0.25, 0.2, 0.1, 0.05},
                new double[] { 0, 0.35, 0.3, 0.25, 0.1},
                new double[] { 0, 0, 0.45, 0.4, 0.15},
                new double[] { 0, 0, 0, 0.4, 0.6},
                new double[] { 0, 0, 0, 0, 1}
            };
            var Probablities = new double[1][];
            Probablities[0] = new double[] { 1, 0, 0, 0, 0 };
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("\tTransition probability matrix:");
            Console.WriteLine(new string('-', 25));
            DisplayMatrix(Matrix);
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("\tInitial probabilities:");
            Console.WriteLine(new string('-', 25));
            DisplayMatrix(Probablities);
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine(new string('-', 25));
                Console.WriteLine($"\tCalculation of probabilities after step #{i}:");
                Console.WriteLine(new string('-', 25));
                Probablities = MultiplyMatrixes(Probablities, Matrix);
                DisplayMatrix(Probablities);
            }
            Console.ReadKey();
        }

        static void DisplayMatrix(double [][] Matrix)
        {
            for (int i = 0; i < Matrix.Length; i++)
            {
                Console.Write("( ");
                Matrix[i].ToList().ForEach(j => Console.Write($"{j,7:0.0000000000}  "));
                Console.WriteLine(")");
            }
            Console.WriteLine();
        }

        static double [][] MultiplyMatrixes(double [][] Matrix1, double [][] Matrix2)
        {
            if (Matrix1.Length == 0 || Matrix2.Length == 0)
            {
                throw new ArgumentException("Matrix size must not be zero");
            }
            for (int i = 0; i < Matrix1.Length - 1; i++)
            {
                if (Matrix1[i].Length != Matrix1[i + 1].Length)
                {
                    throw new ArgumentException("Array 1 is not a matrix");
                }
            }
            for (int i = 0; i < Matrix2.Length - 1; i++)
            {
                if (Matrix2[i].Length != Matrix2[i + 1].Length)
                {
                    throw new ArgumentException("Array 2 is not a matrix");
                }
            }
            if (Matrix1[0].Length != Matrix2.Length)
            {
                throw new ArgumentException("Matrix 1 column length does not match Matirx 2 row size");
            }
            double Temp;
            double[][] Result = new double[Matrix1.Length][];
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i] = new double[Matrix2[0].Length];
                for (int j = 0; j < Result[i].Length; j++)
                {
                    Temp = 0;
                    for (int k = 0; k < Matrix1[0].Length; k++)
                    {
                        Temp += Matrix1[i][k] * Matrix2[k][j];
                    }
                    Result[i][j] = Temp;
                }
            }
            return Result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    public class SLAE
    {
        public static void DisplayMatrix(double[][] Matrix, double[] Vector = null, double[] Result = null)
        {
            if (Matrix.Length == 0 || (Vector != null && Vector.Length == 0))
            {
                throw new ArgumentException("Matrix dimensions must not be 0");
            }
            if (Enumerable.Any(Matrix, i => i.Length != Matrix.Length) || (Vector != null && Matrix.Length != Vector.Length))
            {
                throw new ArgumentException("Matrix sizes must be equal");
            }
            for (int i = 0; i < Matrix.Length; i++)
            {
                Console.Write("( ");
                Matrix[i].ToList().ForEach(j => Console.Write($"{j,7:0.000}  "));
                if (Vector != null)
                    Console.Write($"| {Vector[i],7:0.000}");
                Console.Write(")");
                if (Result != null)
                {
                    Console.Write($"{(i == Matrix.Length / 2 ? " = " : ""),3} | { Result[i],7:0.000} |");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        
        private static void SortMatrix(double[][] Matrix, double[] Vector = null) {
            double Temp;
            double[] TempRow;
            for (int i = 0; i < Matrix.Length; i++)
            {
                // If diagonal element is zero, find non-zero element below it and swap rows
                if (Matrix[i][i] == 0)
                {
                    int j = 0;
                    while (j < Matrix.Length && Matrix[j][i] == 0)
                    {
                        j++;
                    }
                    if (j == Matrix.Length) // No non-zero element found below diagonal element, matrix cannot be sorted
                    {
                        throw new ArgumentException("Matrix could not be sorted, because diagonal contains 0");
                    }
                    // Swap rows i and j = Matrix[i];
                    TempRow = Matrix[i];
                    Matrix[i] = Matrix[j];
                    Matrix[j] = TempRow;
                    if (Vector != null) {
                        Temp = Vector[i];
                        Vector[i] = Vector[j];
                        Vector[j] = Temp;
                    }
                }
            }
        }

        public static double[] Solve(double[][] Matrix, double[] Vector, double EPSILON, bool DEBUG = false)
        {
            if (Matrix.Length == 0 || Vector.Length == 0)
            {
                throw new ArgumentException("Matrix dimensions must not be 0");
            }
            if (Enumerable.Any(Matrix, i => i.Length != Matrix.Length) || Matrix.Length != Vector.Length)
            {
                throw new ArgumentException("Matrix sizes must be equal");
            }
            double[][] Clone = CloneMatrix(Matrix);
            Matrix = Clone;
            Vector = (double[])Vector.Clone();
            double Temp;
            SortMatrix(Matrix, Vector);
            for (int i = 0; i < Matrix.Length; i++)
            {
                Temp = 0;
                for (int j = 0; j < Matrix.Length; j++)
                {
                    if (i != j)
                    {
                        Temp += Math.Abs(Matrix[i][j]);
                    }
                }
                if (Math.Abs(Matrix[i][i]) < Temp)
                {
                    throw new ArgumentException("It is impossible to find a numerical solution for the system");
                }
            }
            double[] Result = new double[Vector.Length];
            double[] PreviousResult = new double[Vector.Length];
            Array.Clear(Result, 0, Result.Length);
            Array.Clear(PreviousResult, 0, PreviousResult.Length);

            int k = 0;
            while (true)
            {
                if (DEBUG)
                    Console.WriteLine($"K = {k}");
                for (int i = 0; i < Matrix.Length; i++)
                {
                    Temp = 0;
                    for (int j = 0; j < Matrix.Length; j++)
                    {
                        if (i != j)
                        {
                            Temp -= Matrix[i][j] * PreviousResult[j];
                        }
                    }
                    Temp += Vector[i];
                    //PreviousResult[i] = Result[i];
                    Result[i] = Temp / Matrix[i][i];

                    if (DEBUG)
                        Console.WriteLine($"Temp = {Temp}; Matrix [{i}, {i}] = {Matrix[i][i]};" +
                            $"x({k}){i} = {Result[i]}; x({k - 1}){i} = {PreviousResult[i]}");
                }
                k++;
                bool Condition = true;
                if (DEBUG)
                    Console.WriteLine("Deltas");
                for (int i = 0; i < Result.Length; i++)
                {
                    if (DEBUG)
                        Console.WriteLine($"x{i} = {Math.Abs(Result[i] - PreviousResult[i])}");
                    if (Math.Abs(Result[i] - PreviousResult[i]) > EPSILON)
                    {
                        Condition = false;
                    }
                }
                if (Condition)
                {
                    break;
                }
                else
                {
                    for (int i = 0; i < Result.Length; i++)
                    {
                        PreviousResult[i] = Result[i];
                    }
                }
            }
            return Result;
        }

        private static double[][] CloneMatrix(double[][] Matrix) {
            double[][] Clone = new double[Matrix.Length][];
            for (int i = 0; i < Clone.Length; i++)
            {
                Clone[i] = new double[Matrix[i].Length];
                for (int j = 0; j < Clone[i].Length; j++)
                {
                    Clone[i][j] = Matrix[i][j];
                }
            }
            return Clone;
        }

        public static double Determinant(double[][] Matrix, bool DEBUG = false)
        {
            if (Matrix.Length == 0)
            {
                throw new ArgumentException("Matrix dimensions must not be 0");
            }
            if (Enumerable.Any(Matrix, i => i.Length != Matrix.Length))
            {
                throw new ArgumentException("Matrix sizes must be equal");
            }
            double[][] Clone;
            
            Clone = CloneMatrix(Matrix);

            SortMatrix(Clone);
            if (DEBUG)
                DisplayMatrix(Clone);
            double Temp;
            for (int i = 0; i < Clone.Length; i++)
            {
                for (int j = i + 1; j < Clone.Length; j++)
                {
                    if (Matrix[i][i] != 0) {
                    Temp = -Matrix[j][i] / Matrix[i][i];
                        for (int k = 0; k < Clone.Length; k++)
                        {
                            Clone[j][k] += Matrix[i][k] * Temp;
                        }
                    }
                }
            }
            if (DEBUG)
                DisplayMatrix(Clone);
            Temp = 1;
            for (int i = 0; i < Clone.Length; i++)
            {
                Temp *= Clone[i][i];
            }
            return Temp;
        }

        public static double[] AccurateSolve(double[][] Matrix, double[] Vector, bool DEBUG = false)
        {
            if (Matrix.Length == 0 || Vector.Length == 0)
            {
                throw new ArgumentException("Matrix dimensions must not be 0");
            }
            if (Enumerable.Any(Matrix, i => i.Length != Matrix.Length) || Matrix.Length != Vector.Length)
            {
                throw new ArgumentException("Matrix sizes must be equal");
            }
            //double Temp;
            double[][] Clone;// = CloneMatrix(Matrix);
            double determinant = Determinant(Matrix);
            if (DEBUG)
            {
                Console.WriteLine(new string('-', 25));
                DisplayMatrix(Matrix, Vector);
                Console.WriteLine($"Matrix Determinant: {determinant}\n" + new string('-', 25));
            }
            if (Enumerable.Any(Matrix, row => Enumerable.All(row, element => element == 0)))
            {
                throw new ArgumentException("The solution of the system is undetermined (Infinite number of solutions).");
            }
            double[] Result = new double[Vector.Length];
            //double[] Determinants = new double[Vector.Length];
            for (int i = 0; i < Matrix.Length; i++)
            {
                Clone = CloneMatrix(Matrix);
                for (int j = 0; j < Clone.Length; j++)
                {
                    Clone[j][i] = Vector[j];
                }
                if (DEBUG)
                {
                    Console.WriteLine(new string('-', 25) + $"\nAdditional Matrix #{i}\n" + new string('-', 25));
                }
                Result[i] = Determinant(Clone, DEBUG);
                if (DEBUG)
                    Console.WriteLine($"Determinant #{i}: {Result[i]}\n" + new string('-', 25));
                Result[i] /= determinant;
            }
            return Result;
        }
    }
}

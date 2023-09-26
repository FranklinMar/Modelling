using MathNet.Symbolics;
using System;
using System.Linq;

namespace Lab_2
{

    class Lab2_2
    {
        static public void Main_(string[] args)
        {
            double[][] matrix;
            double[] vector;
            double EPSILON = 0.001;
            matrix = new double[][] {
                new double[] { 20.9, 1.2, 2.1},
                new double[] { 1.2, 21.2, 1.5},
                new double[] { 2.1, 1.5, 19.8}
            };
            vector = new double[] { 20, 19.2, 21.3 };
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("\tРІШЕННЯ #1 (3x3):");
            Console.WriteLine(new string('-', 25));
            double[] result = null;
            try
            {
                 result = SLAE.Solve(matrix, vector, EPSILON);
                
            } catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                /*for (int i = 0; i < matrix.Length; i++)
                {
                    Console.Write("( ");
                    matrix[i].ToList().ForEach(j => Console.Write($"{j, 7:0.000}  "));
                    Console.WriteLine($"| {vector[i], 7:0.000} )");
                }*/
                result = null;
            } finally
            {
                SLAE.DisplayMatrix(matrix, vector, result);
            }
            matrix = new double[][] {
                new double[]{ 3, 12, -1, 0},
                new double[]{ -5, 2, 0, 32},
                new double[]{ 2, 0, 16, -3 },
                new double[]{ 12, 3, 0, 0 }
            };
            vector = new double[] { 18, -15, 0, 21 };
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("\tРІШЕННЯ #2 (4x4):");
            Console.WriteLine(new string('-', 25));
            try
            {
                result = SLAE.Solve(matrix, vector, EPSILON);
                /*for (int i = 0; i < matrix.Length; i++)
                {
                    Console.Write("( ");
                    matrix[i].ToList().ForEach(j => Console.Write($"{j, 7:0.000}  "));
                    Console.WriteLine($"| {vector[i], 7:0.000} ) {(i == matrix.Length / 2 ? " = " : ""), 5} | {result[i], 7:0.000} |");
                }*/
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                /*for (int i = 0; i < matrix.Length; i++)
                {
                    Console.Write("( ");
                    matrix[i].ToList().ForEach(j => Console.Write($"{j, 7:0.000}  "));
                    Console.WriteLine($"| {vector[i], 7:0.000} )");
                }*/
                result = null;
            }
            finally
            {
                SLAE.DisplayMatrix(matrix, vector, result);
            }
            Console.WriteLine(new string('-', 25));
            Console.WriteLine("\tТОЧНЕ РІШЕННЯ (4x4):");
            Console.WriteLine(new string('-', 25));
            try
            {
                result = SLAE.AccurateSolve(matrix, vector);
                /*for (int i = 0; i < matrix.Length; i++)
                {
                    Console.Write("( ");
                    matrix[i].ToList().ForEach(j => Console.Write($"{j, 7:0.000}  "));
                    Console.WriteLine($"| {vector[i], 7:0.000} ) {(i == matrix.Length / 2 ? " = " : ""), 5} | {result[i], 7:0.000} |");
                }*/
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                /*for (int i = 0; i < matrix.Length; i++)
                {
                    Console.Write("( ");
                    matrix[i].ToList().ForEach(j => Console.Write($"{j, 7:0.000}  "));
                    Console.WriteLine($"| {vector[i], 7:0.000} )");
                }*/
                result = null;
            }
            finally
            {
                SLAE.DisplayMatrix(matrix, vector, result);
            }
            Console.ReadKey();
        }
    }
}

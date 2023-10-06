using System;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using System.Linq;
using MathNet.Symbolics;
using Expression = MathNet.Symbolics.SymbolicExpression;

namespace Lab_8
{
    public class Lab8
    {

        static public double Main_(Expression Function, string[] args)
        {
            //var X = Expression.Variable("x");
            string Line = new string('-', 50);
            //var Function = X * X * X - 3 * X * X - 30 * X + 140;
            Func<Expression, double, double> Eval = (function, _X) => function.Evaluate(new Dictionary<string, FloatingPoint>() { { "x", _X } }).RealValue;
            Console.WriteLine(Line);
            Console.WriteLine("\tПошук мінімуму функції:");
            Console.WriteLine($"f(x) = {ToString(Function)} -> min");
            Console.WriteLine(Line);
            double a, b, Epsilon, x, y, z, L;
            a = GetVariable("Введіть нижню границю проміжка a [a, ...]: ");
            b = GetVariable("Введіть верхню границю проміжка b [..., b]: ", $"Значення не повинно бути менше ніж нижня границя a = {a}", a);
            Epsilon = GetVariable("Введіть точність ε: ", "Точність повинна бути додатнім числом", 0);
            //List<double> A = new List<double>(), B = new List<double>(), x;
            int k = 0;
            Console.WriteLine(Line);
            Console.WriteLine($"|{"k",4}|{"a_k",6}|{"b_k",6}|{"x_k",6}|{"L2k = [a_k;b_k]",10}|[L2k] |");
            Console.WriteLine(Line);
            x = (a + b) / 2;
            while (true)
            {
                L = Math.Abs(b - a);
                y = a + L / 4;
                z = b - L / 4;
                if (L <= Epsilon)
                {
                    break;
                }
                Console.WriteLine($"|{k,4}|{a, 6:0.000}|{b, 6:0.000}|{x, 6:0.000}|[{a, 6:0.000};{b, 6:0.000}]|{L, 6:0.000}|");
                if (Eval(Function, y) < Eval(Function, x))
                {
                    b = x;
                    x = y;
                } else if (Eval(Function, z) < Eval(Function, x)) {
                    a = x;
                    x = z;
                } else {
                    a = y;
                    b = z;
                }
                k++;
            }
            Console.WriteLine(Line);
            x = (a + b) / 2;
            Console.WriteLine($"x min = {x}");
            Console.WriteLine($"F(x min) = {Eval(Function, x)}");
            Console.Read();
            return x;
        }

        static public string ToString(Expression function) => Infix.Format(Algebraic.Expand(function.Expression));

        static public double GetVariable(string Prompt, string ErrorMessage="", double? MinValue = null)
        {
            double Variable;
            string Line;
            while (true)
            {
                Console.Write(Prompt);
                Line = Console.ReadLine().Replace(',', '.');
                if (Line == "" || Line.Length == 0)
                {
                    continue;
                }
                try
                {
                    Variable = Evaluate(Line);
                }
                catch (Exception)
                {
                    try
                    {
                        Variable = Convert.ToDouble(Line);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                if (MinValue != null && Variable < MinValue)
                {
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine('\t' + ErrorMessage);
                    Console.WriteLine(new string('-', 50));
                    //Thread.Sleep(5000);
                    //Console.Clear();
                }
                else
                {
                    break;
                }
            }
            return Variable;
        }
        static double Evaluate(string Expression)
        {
            var loDataTable = new DataTable();
            var loDataColumn = new DataColumn("Eval", typeof(double), Expression);
            loDataTable.Columns.Add(loDataColumn);
            loDataTable.Rows.Add(0);
            return (double)(loDataTable.Rows[0]["Eval"]);
        }

        static public double Factorial(int Number)
        {
            if (Number < 0)
            {
                throw new ArgumentException("Cannot be negative");
            }
            int Result = 1;
            for (int i = 1; i <= Number; i++)
            {
                Result *= i;
            }
            return Result;
        }
    }
}

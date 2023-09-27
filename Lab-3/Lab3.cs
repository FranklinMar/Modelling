using System;
using System.Collections.Generic;
//using WebApiContrib.Formatting;
//using MathNet.Symbolics;
//using Expression = MathNet.Symbolics.SymbolicExpression;

namespace Lab_3
{
    class Lab3
    {
        /*private double h = 0.1;
        public double Epsilon = 0.0001;
        public static Expression x { get; private set; } = Expression.Variable("x");

        static void Main(string[] args)
        {
            var Function = x; 
        }*/
        static public void Main_(List<string> Names, List<List<double[]>> Results)
        {
            //List<string> Names = new List<string>();
            List<double []> AccurateResults = new List<double []>();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            double x0 = 1.8;
            double y0 = 2.6;
            double h = 0.1;
            string Line = new string('-', 20);
            string DifferentialEquation = "y' = y + cos(x / √5)";
            string Condition = $"y({x0}) = {y0}";
            string AccurateFormula = "y = √5/6 * (sin(x / √5) - √5 * cos (x / √5)) + C * e^x";
            string Solution = AccurateFormula.Replace("+ C * e^x", 
                "-\n\t- 1 / 30 * (5√5 * sin(9 / 5√5) - 25 * cos (9 / 5√5) - 78) * e ^ (x - 1.8)");
            Console.WriteLine($"\tЗадача Коші\n{Line}"); //($"\tDifferential equation\n{Line}");
            Console.WriteLine($"Диференційне рівняння: {DifferentialEquation}");//($"Differential Equation: {DifferentialEquation}");
            Console.WriteLine($"Умова: {Condition}");//($"Condition: {Condition}");
            Console.WriteLine($"Аналітичний розв'язок ДР: {AccurateFormula}");//($"Accurate solution function: {AccurateFormula}");
            Console.WriteLine($"Розв'язок задачі Коші: {Solution}\n{Line}\n\n");//($"Full solution function: {Solution}\n{Line}\n\n");

            string Header = $"|  {"x",-3}|  {"y(x)",-10}|";
            string FormatString = "|{0, 5:0.00}|{1, 12:0.000000}|";

            double Sqrt5 = Math.Sqrt(5);
            Func<double, double> AccurateFunction = x => Sqrt5 / 6 * (Math.Sin(x / Sqrt5) - Sqrt5 * Math.Cos(x / Sqrt5)) -
                   (5 * Sqrt5 * Math.Sin(9 / (5 * Sqrt5)) - 25 * Math.Cos(9 / (5 * Sqrt5)) - 78) / 30 * Math.Exp(x - x0);


            Names.Add("Аналітичний");//("Accurate");
            Console.WriteLine($"\tАналітичний розв'язок");//($"\tAccurate solution");
            Console.WriteLine($"{Line}\n{Header}\n{Line}");
            for (double x = x0, y; Math.Round(x, 1) <= x0 + 1; x = Math.Round(x + h, 1))
            {
                y = AccurateFunction(x);
                AccurateResults.Add(new double[] {x, y});
                Console.WriteLine(string.Format(FormatString, x, y));
            }
            Console.WriteLine($"{Line}\n\n");
            Results.Add(AccurateResults);

            int i = 0;
            Line += new string('-', 13);
            Header += $"  {"|y-y(x)|",-10}|";
            FormatString += "{2, 12:0.000000}|";


            Func <double, double, double> Function = (x, y) => y + Math.Cos(x / Math.Sqrt(5));


            Names.Add("Ейлера");//("Euler's");
            List<double []> EulerResults = new List<double []>();
            Console.WriteLine($"\tМетод Ейлера");//($"\tEuler's Method");
            Console.WriteLine($"{Line}\n{Header}\n{Line}");
            for (double x = x0, y = y0; Math.Round(x, 1) <= x0 + 1; x = Math.Round(x + h, 1), y += h * Function(x, y), i++)
            {
                EulerResults.Add(new double[] {x, y});
                Console.WriteLine(string.Format(FormatString, x, y, Math.Abs(AccurateResults[i][1] - y)));
            }
            Console.WriteLine($"{Line}\n\n");
            Results.Add(EulerResults);


            i = 0;
            Names.Add("Ейлера-Коші");//("Cauchy-Euler's");
            List<double []> CauchyEulerResults = new List<double []>();
            Console.WriteLine($"\tМетод Ейлера-Коші");//($"\tCauchy-Euler's Method");
            Console.WriteLine($"{Line}\n{Header}\n{Line}");
            for (double x = x0, y = y0, y_new; Math.Round(x, 1) <= x0 + 1; x = Math.Round(x + h, 1), i++)
            {
                CauchyEulerResults.Add(new double[] { x, y });
                Console.WriteLine(string.Format(FormatString, x, y, Math.Abs(AccurateResults[i][1] - y)));
                y_new = y + h * Function(x, y);
                y += h / 2 * (Function(x, y) /*f(x, y)*/ + Function(x + h / 2, y_new) /*f(x_i+1, y_i + h * f(x_i, y_i))*/);
            }
            Console.WriteLine($"{Line}\n\n");
            Results.Add(CauchyEulerResults);


            i = 0;
            Names.Add("Вдосконалений Ейлера"); //("Improved Euler's");
            List<double []> ImprovedEulerResults = new List<double []>();
            Console.WriteLine($"\tВдосконалений метод Ейлера");//($"\tImproved Euler's Method");
            Console.WriteLine($"{Line}\n{Header}\n{Line}");
            for (double x = x0, y = y0; Math.Round(x, 1) <= x0 + 1; x = Math.Round(x + h, 1), i++)
            {
                ImprovedEulerResults.Add(new double[] { x, y });
                Console.WriteLine(string.Format(FormatString, x, y, Math.Abs(AccurateResults[i][1] - y)));
                y += h * Function(x + h / 2, y + h / 2 * Function(x, y));
            }
            Console.WriteLine($"{Line}\n\n");
            Results.Add(ImprovedEulerResults);


            i = 0;
            Names.Add("Рунге-Кутта");//("Runge-Kutta's");
            List<double []> RungeKuttaResults = new List<double []>();
            Console.WriteLine($"\tМетод Рунге-Кутта");//($"\tRunge-Kutta's Method");
            Console.WriteLine($"{Line}\n{Header}\n{Line}");
            for (double x = x0, y = y0, k0, k1, k2, k3; Math.Round(x, 2) <= x0 + 1; x = Math.Round(x + h, 1), i++)
            {
                RungeKuttaResults.Add(new double[] { x, y });
                Console.WriteLine(string.Format(FormatString, x, y, Math.Abs(AccurateResults[i][1] - y)));
                k0 = Function(x, y);
                k1 = Function(x + h / 2, y + k0 * h / 2);
                k2 = Function(x + h / 2, y + k1 * h / 2);
                k3 = Function(x + h, y + k2 * h);
                y += h / 6 * (k0 + 2 * k1 + 2 * k2 + k3);
            }
            Console.WriteLine($"{Line}\n\n");
            Results.Add(RungeKuttaResults);
            //JavaScriptSerializer ser
        }
    }
}
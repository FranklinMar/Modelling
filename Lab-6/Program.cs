using System;
using System.Data;
using System.Threading;

namespace Lab_6
{
    class Program
    {
        public class Lab6
        {
            static public void Main(string[] args)
            {
                string Line = new string('-', 50);
                string ReadLine;
                int Mode;
                string[] Modes = new string[] { "MSS with refusals", "Single-channel MSS with a limited queue" };
                while (true)
                {
                    Console.OutputEncoding = System.Text.Encoding.UTF8;
                    string ParamError = "Intensity must be a positive number";
                    bool WithLimitedQueue;
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine(Line);
                        Console.WriteLine("\tMass Service Systems:");
                        Console.WriteLine(Line);

                        Console.WriteLine("Select the operating mode:\n");
                        for (int i = 0; i < Modes.Length; i++)
                        {
                            Console.WriteLine($"\t{i + 1}. {Modes[i]}");
                        }
                        Console.Write("@ ");
                        ReadLine = Console.ReadLine();
                        try
                        {
                            Mode = Convert.ToInt32(ReadLine);
                            if (Mode > 0 && Mode <= Modes.Length)
                            {
                                WithLimitedQueue = (Mode == 2);
                                break;
                            }
                        }
                        catch (Exception) { }
                    }
                    double Lambda, Mu;
                    Lambda = GetVariable("Enter the intensity of the flow of customers (λ): ", ParamError);
                    Mu = GetVariable("Enter the application processing intensity (μ): ", ParamError);


                    int Number;
                    Number = (int)GetVariable((WithLimitedQueue ? "Enter the number of seats in the queue" : "Enter the number of channels") + " (n): ",
                        "The number must be natural");
                    double Rho = Lambda / Mu;
                    double[] P = new double[Number + 1];
                    double Denominator = 0, A, Q, P_vidm;
                    if (WithLimitedQueue)
                    {
                        P[0] = (1 - Rho) / (1 - Math.Pow(Rho, Number + 2));
                        for (int i = 1; i <= Number; i++)
                        {
                            P[i] = Math.Pow(Rho, i) * P[0];
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= Number; i++)
                        {
                            Denominator += Math.Pow(Rho, i) / Factorial(i);
                        }
                        P[0] = 1 / Denominator;
                        for (int i = 1; i <= Number; i++)
                        {
                            P[i] = P[0] * Math.Pow(Rho, i) / Factorial(i);
                        }
                    }
                    Q = 1 - P[P.Length - 1];
                    A = Lambda * Q;
                    Console.WriteLine(Line);
                    Console.WriteLine($"\tλ = {Lambda}");
                    Console.WriteLine($"\tμ = {Mu}");
                    Console.WriteLine($"\tn = {Number}");
                    Console.WriteLine($"\tρ = {Rho}");
                    Console.WriteLine(Line);
                    Console.WriteLine("\tMarginal probabilities:");
                    Console.WriteLine(Line);
                    for (int i = 0; i < P.Length; i++)
                    {
                        Console.WriteLine($"P{i} = {P[i],5:0.000}");
                    }
                    Console.WriteLine(Line);
                    if (WithLimitedQueue)
                    {
                        P_vidm = Math.Pow(Rho, Number + 1) * (1 - Rho) / (1 - Math.Pow(Rho, Number + 2));
                        Q = 1 - P_vidm;
                        A = Q * Lambda;
                        double r = (1 - Math.Pow(Rho, Number) * (Number + 1 - Number * Rho)) * Math.Pow(Rho, 2) / ((1 - Math.Pow(Rho, Number + 2)) * (1 - Rho));
                        double w = (Rho - Math.Pow(Rho, Number + 2)) / (1 - Math.Pow(Rho, Number + 2));
                        Console.WriteLine($"\tProbability of denial of service       | P den = P{Number}+1 = ρ^(n+1)*(1-ρ)/(1-ρ^(n+2)) = {P_vidm,5:0.000}");
                        Console.WriteLine($"\tRelative bandwidth                     | Q = 1 - P відм = {Q,5:0.000}");
                        Console.WriteLine($"\tAbsolute bandwidth                     | А = λ * Q = {A,5:0.000}");
                        Console.WriteLine($"\tAverage number of requests in the queue| r = {r,5:0.000}");
                        Console.WriteLine($"\tAverage number of applications inside  | w = {w,5:0.000}");
                        Console.WriteLine($"\tAverage number of pending applications | k = w + r = {w + r,5:0.000}");
                        Console.WriteLine($"\tAverage waiting time                   | T wait = {r / Lambda,5:0.000}");
                        Console.WriteLine($"\tAverage time spent in the system       | T spent = T оч + Q / μ = {r / Lambda + Q / Mu,5:0.000}");
                    }
                    else
                    {
                        Console.WriteLine($"\tProbability of denial of service   | P den = P{Number} = {P[Number],5:0.000}");
                        Console.WriteLine($"\tRelative bandwidth                 | Q = 1 - P{Number} = {Q,5:0.000}");
                        Console.WriteLine($"\tAbsolute bandwidth                 | А = λ * Q = {A,5:0.000}");
                        Console.WriteLine($"\tAverage number of busy channels    | k = ρ * (1 - P{Number}) = {Rho * (1 - P[Number]),5:0.000}");
                        Console.WriteLine($"\tAverage service time of one channel| T serv = 1 / μ = {1 / Mu,5:0.000}");
                        Console.WriteLine($"\tAverage channel idle time          | T λ = 1 / λ = {1 / Lambda,5:0.000}");
                    }
                    Console.WriteLine(Line);
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            static public double GetVariable(string Prompt, string ErrorMessage)
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

                    if (Variable <= 0)
                    {
                        Console.WriteLine(new string('-', 50));
                        Console.WriteLine('\t' + ErrorMessage);
                        Console.WriteLine(new string('-', 50));
                        Thread.Sleep(5000);
                        Console.Clear();
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
}

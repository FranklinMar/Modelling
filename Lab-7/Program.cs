using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Common;
using System.Runtime.InteropServices;

namespace Lab_7
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AllocConsole();
            string ReadLine;
            string Line = new string('-', 50);
            string ParamError = "Intensity must be a positive number";
            string[] Modes = new string[] { "MSS with a limited queue", "MSS with an unlimited queue", "Calculate MSS and display charts" };
            int Mode;
            double Lambda, Mu = 1, Rho;
            double[] P;
            int NumberChannels, NumberQueue;
            while (true)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
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
                            Mode--;
                            break;
                        }
                    }
                    catch (Exception) { }
                }
                Lambda = GetVariable("Enter the intensity of the flow of customers (λ): ", ParamError, 0);
                if (Mode != 2)
                {
                    Mu = GetVariable("Enter the application processing intensity (μ): ", ParamError, 0);
                }
                NumberChannels = (int)GetVariable("Enter the number of channels (n): ", "The number must be natural");
                if (Mode == 1)
                {
                    NumberQueue = 1;
                }
                else
                {
                    NumberQueue = (int)GetVariable("Enter the number of seats in the queue (m): ", "The number must be natural or 0", 0);
                }
                if (Mode == 2)
                {
                    Dictionary<int, double> DictionaryA = new Dictionary<int, double>(),
                        DictionaryP_vidm = new Dictionary<int, double>(),
                        DictionaryTime_obr = new Dictionary<int, double>();
                    int TimeBegin = (int)GetVariable("Enter the start of the time interval [t start]: ", "The number must be natural", 0);
                    int TimeEnd = (int)GetVariable("Enter the end of the time interval [t end]: ", "The number must be bigger than the start of interval", TimeBegin);
                    for (int time = TimeBegin; time <= TimeEnd; time++)
                    {
                        Console.WriteLine($"\t t = {time}");
                        Mu = 1.0 / time;
                        Rho = Lambda / Mu;
                        Console.WriteLine(Line);
                        Console.WriteLine($"\tIntensity of the incoming flow\nλ = {Lambda}");
                        Console.WriteLine($"\tService flow intensity\nμ = {Mu}");
                        Console.WriteLine($"\tCalculated intensity\nρ = {Rho}");
                        P = new double[NumberChannels + NumberQueue + 1];
                        double Gamma = Rho / NumberChannels;
                        double Denominator = 0, A, Q, P_vidm, r;
                        for (int i = 0; i <= NumberChannels; i++)
                        {
                            Denominator += Math.Pow(Rho, i) / Factorial(i);
                        }
                        for (int i = NumberChannels + 1; i <= NumberChannels + NumberQueue; i++)
                        {
                            Denominator += Math.Pow(Rho, i) / (Factorial(NumberChannels) * Math.Pow(NumberChannels, (i - NumberChannels)));
                        }
                        P[0] = 1 / Denominator;
                        for (int i = 1; i <= NumberChannels; i++)
                        {
                            P[i] = P[0] * Math.Pow(Rho, i) / Factorial(i);
                        }
                        for (int i = NumberChannels + 1; i < P.Length; i++)
                        {
                            P[i] = P[0] * Math.Pow(Rho, i) / (Factorial(NumberChannels) * Math.Pow(NumberChannels, (i - NumberChannels)));
                        }
                        P_vidm = P[P.Length - 1];
                        Q = 1 - P_vidm;
                        A = Lambda * Q;
                        r = ((Math.Pow(Rho, NumberChannels + 1) * P[0]) / (NumberChannels * Factorial(NumberChannels))) *
                                ((1 - (NumberQueue + 1) * Math.Pow(Gamma, NumberQueue) + NumberQueue * Math.Pow(Gamma, NumberQueue + 1)) / ((1 - Gamma) * (1 - Gamma)));
                        double Time_obr = r / Lambda;
                        Console.WriteLine(Line);
                        Console.WriteLine($"P den: {P_vidm}\nA     : {A}\nt calc : {Time_obr}");
                        Console.WriteLine("\tMarginal probabilities:");
                        Console.WriteLine(Line);
                        for (int i = 0; i < P.Length; i++)
                        {
                            string Number = i > NumberChannels ? $"{NumberChannels}+{i - NumberChannels}" : $"{i}";
                            Console.WriteLine($"P{Number} = {P[i],5:0.000000}");
                        }
                        DictionaryP_vidm.Add(time, P_vidm);
                        DictionaryA.Add(time, A);
                        DictionaryTime_obr.Add(time, Time_obr);
                    }
                    var HTMLFile = File.ReadAllText($"..\\..\\..\\ChartFile.html");
                    string Names = JsonConvert.SerializeObject(new List<string> { "P den", "A", "t wait" });
                    string Maps = JsonConvert.SerializeObject(new List<List<KeyValuePair<int, double>>> { DictionaryP_vidm.ToList(), DictionaryA.ToList(), DictionaryTime_obr.ToList() });
                    HTMLFile = HTMLFile.Replace("let names", $"let names = {Names}");
                    HTMLFile = HTMLFile.Replace("let maps", $"let maps = {Maps}");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new LabForm(HTMLFile));

                }
                else
                {
                    Rho = Lambda / Mu;
                    P = new double[NumberChannels + NumberQueue + 1];
                    double Gamma = Rho / NumberChannels;
                    double Denominator = 0, A, Q, P_vidm, z, r;
                    for (int i = 0; i <= NumberChannels; i++)
                    {
                        Denominator += Math.Pow(Rho, i) / Factorial(i);
                    }
                    if (Mode == 1)
                    {
                        Denominator += Math.Pow(Rho, NumberChannels + 1) / (Factorial(NumberChannels) * (NumberChannels - Rho));
                    }
                    else
                    {
                        for (int i = NumberChannels + 1; i <= NumberChannels + NumberQueue; i++)
                        {
                            Denominator += Math.Pow(Rho, i) / (Factorial(NumberChannels) * Math.Pow(NumberChannels, (i - NumberChannels)));
                        }
                    }
                    P[0] = 1 / Denominator;
                    for (int i = 1; i <= NumberChannels; i++)
                    {
                        P[i] = P[0] * Math.Pow(Rho, i) / Factorial(i);
                    }
                    for (int i = NumberChannels + 1; i < P.Length; i++)
                    {
                        P[i] = P[0] * Math.Pow(Rho, i) / (Factorial(NumberChannels) * Math.Pow(NumberChannels, (i - NumberChannels)));
                    }
                    Console.WriteLine(Line);
                    Console.WriteLine($"\tIntensity of the incoming flow\nλ = {Lambda}");
                    Console.WriteLine($"\tService flow intensity\nμ = {Mu}");
                    Console.WriteLine($"\tNumber of channels\nn = {NumberChannels}");
                    if (Mode == 1)
                    {
                        Console.WriteLine($"\tNumber of seats in the queue\nm = {NumberChannels}");
                    }
                    Console.WriteLine($"\tCalculated intensity\nρ = {Rho}");
                    Console.WriteLine(Line);
                    Console.WriteLine("\tMarginal probabilities:");
                    Console.WriteLine(Line);
                    for (int i = 0; i < P.Length; i++)
                    {
                        string Number = i > NumberChannels ? $"{NumberChannels}+{i - NumberChannels}" : $"{i}";
                        Console.WriteLine($"P{Number} = {P[i],5:0.000000}");
                    }
                    if (Mode == 1)
                    {
                        Console.WriteLine("... ... ...");
                    }
                    Console.WriteLine(Line);
                    if (Mode == 1)
                    {
                        P_vidm = 0;
                        Q = 1;
                        A = Lambda;
                        z = A / Mu;
                        r = Math.Pow(Rho, NumberChannels + 1) * P[0] / (NumberChannels * Factorial(NumberChannels) * (1 - Gamma) * (1 - Gamma));
                    }
                    else
                    {
                        P_vidm = P[P.Length - 1];
                        Q = 1 - P_vidm;
                        A = Lambda * Q;
                        z = A / Mu;
                        r = ((Math.Pow(Rho, NumberChannels + 1) * P[0]) / (NumberChannels * Factorial(NumberChannels))) *
                                ((1 - (NumberQueue + 1) * Math.Pow(Gamma, NumberQueue) + NumberQueue * Math.Pow(Gamma, NumberQueue + 1)) / ((1 - Gamma) * (1 - Gamma)));
                    }
                    Console.WriteLine($"\tProbability of denial of service                          | P den = P{NumberChannels}+{NumberQueue} = ρ^(n+m)/((n^m)*(n!)) = {P_vidm,5:0.000000}");
                    Console.WriteLine($"\tRelative bandwidth                                        | Q = 1 - P den = {Q,5:0.000000}");
                    Console.WriteLine($"\tAbsolute bandwidth                                        | А = λ * Q = {A,5:0.000000}");
                    Console.WriteLine($"\tAverage number of busy channels                           | z = A / μ = {z,5:0.000000}\n\t(Average number of applications for processing in MSS)");
                    Console.WriteLine($"\tAverage number of applications for processing inside queue| r = {r,5:0.000000}");
                    Console.WriteLine($"\tAverage number of applications related to MSS             | k = z + r = {z + r,5:0.000000}");
                    Console.WriteLine($"\tAverage waiting time in line                              | T wait = {r / Lambda,5:0.000000}");
                    Console.WriteLine($"\tAverage time spent in the system                          | T av = T оч + Q / μ = {r / Lambda + Q / Mu,5:0.000000}");
                    Console.WriteLine(Line);
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        static public double GetVariable(string Prompt, string ErrorMessage, double MinValue = 1)
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

                if (Variable < MinValue)
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

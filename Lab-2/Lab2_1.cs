using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Lab_2
{

    class Lab2_1
    {
        static public void Main_(string[] args)
        {
            List<double[]> Values;
            Lagrange polynome;
            /*Values = new List<double[]>() { new double[] { 0, 1 },
                new double[] { 2, 3 },
                new double[] { 3, 2 },
                new double[] { 5, 5 } };
            double x = 4;
            Console.Write($"\nL{Values.Count} (x) = ");
            
            //Console.WriteLine($"L{Values.Count} (x) = {polynome.Polynome}");
            Console.WriteLine($"\nL{Values.Count} (x) = {polynome.Simplify()}\n");

            Console.WriteLine($"L{Values.Count} ({x}) = {polynome.Evaluate(4)}\n\n");*/


            Values = new List<double[]>() { new double[] { -2, -7 },
                new double[] { -1, 4 },
                new double[] { 0, 1 },
                new double[] { 1, 2 } };
            Console.Write($"\nL{Values.Count} (x) = ");
            polynome = new Lagrange(Values);
            //Console.WriteLine($"L{Values.Count} (x) = {polynome.Polynome}");
            Console.WriteLine($"\nL{Values.Count} (x) = {polynome.Simplify()}\n\n");
            List<double> SearchedValues = new List<double>() { -3, -1.5, 0.5, 1.5 };

            foreach(double value in SearchedValues)
            {
                Console.WriteLine($"L{polynome.Count} ({value}) = {polynome.Evaluate(value)}");
            }
            /*Console.WriteLine($"L{Values.Count} (-3) = {polynome.Evaluate(-3)}\n");
            Console.WriteLine($"L{Values.Count} (-1.5) = {polynome.Evaluate(-1.5)}\n");
            Console.WriteLine($"L{Values.Count} (0.5) = {polynome.Evaluate(0.5)}\n");
            Console.WriteLine($"L{Values.Count} (1.5) = {polynome.Evaluate(1.5)}\n");*/
            //Console.ReadKey();
            string HTMLFile = File.ReadAllText("..\\..\\..\\..\\ChartFile.html");
            var Function = JsonConvert.SerializeObject(polynome.Simplify());
            var FunctionValues = JsonConvert.SerializeObject(SearchedValues);
            HTMLFile = HTMLFile.Replace("/*@MARK1*/", $"{Function}");
            HTMLFile = HTMLFile.Replace("/*@MARK2*/", $"{FunctionValues}");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LabForm(HTMLFile));
        }
    }
}

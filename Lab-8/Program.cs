using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Symbolics;
using Expression = MathNet.Symbolics.SymbolicExpression;
using Newtonsoft.Json;
using Common;
using System.Runtime.InteropServices;
using System.IO;

namespace Lab_8
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
        static void Main(string[] args)
        {
            AllocConsole();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var X = Expression.Variable("x");
            var Function = X * X * X - 3 * X * X - 30 * X + 140;
            double Xmin = Lab8.Main_(Function, args);

            var HTMLFile = File.ReadAllText($"..\\..\\..\\ChartFile.html");
            string FunctionString = JsonConvert.SerializeObject(Lab8.ToString(Function));// MaxSineFunction = x * sin(5 * x)
            string ValueString = JsonConvert.SerializeObject(Xmin);
            HTMLFile = HTMLFile.Replace("let Function", $"let Function = {FunctionString}");
            HTMLFile = HTMLFile.Replace("let Value", $"let Value = {ValueString}");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LabForm(HTMLFile));
        }
    }
}

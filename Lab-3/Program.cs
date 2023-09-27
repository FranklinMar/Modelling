using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Common;

namespace Lab_3
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
            List<string> Names = new List<string>();
            List<List<double[]>> Results = new List<List<double[]>>();
            Lab3.Main_(Names, Results);

            string HTMLFile = File.ReadAllText("..\\..\\..\\ChartFile.html");
            var names_array = JsonConvert.SerializeObject(Names);
            var map_array = JsonConvert.SerializeObject(Results);
            HTMLFile = HTMLFile.Replace("/*@MARK1*/", $"{names_array}");
            HTMLFile = HTMLFile.Replace("/*@MARK2*/", $"{map_array}");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LabForm(HTMLFile));
        }
    }
}

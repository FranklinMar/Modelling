using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_2
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
            int Lab;
            int Labs = 3;
            while (true)
            {
                Console.Write("Enter Lab (0, 1, 2): ");
                try
                {
                    Lab = Convert.ToInt32(Console.ReadLine());
                    if (Lab < 0 || Lab >= Labs)
                    {
                        throw new Exception("Out of labs range");
                    }
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            switch (Lab)
            {
                case 0:
                    Lab2.Main_(args);
                    break;
                case 1:
                    Lab2_1.Main_(args);
                    break;
                case 2:
                    Lab2_2.Main_(args);
                    break;
            }
            Console.Read();
        }
    }
}

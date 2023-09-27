using System;
using System.Collections.Generic;

namespace Lab_2
{
    class Lab2
    {
        static public void Main_(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Model model;
            double lambda, time;
            int iterations;
            while (true)
            {
                Console.Write("Enter λ (Average amount per time): ");
                try
                {
                    lambda = Convert.ToDouble(Console.ReadLine());
                    model = new Model(lambda, 1, 1);
                    break;
                } 
                catch (Exception e) 
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (true)
            {
                Console.Write("Enter the time T (Time of observing): ");
                try
                {
                    time = Convert.ToDouble(Console.ReadLine());
                    model = new Model(lambda, time, 1);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            while (true)
            {
                Console.Write("Enter the number of iterations N: ");
                try
                {
                    iterations = Convert.ToInt32(Console.ReadLine());
                    model = new Model(lambda, time, iterations);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Generating streams of random events");
            model.GenerateStream();
            int counter = 1;
            foreach (List<Tuple<double, double>> list in model.RandomAndTime)
            {
                Console.WriteLine($"\nIteration #{counter}");
                Console.WriteLine($"{new string('=', 20)}");
                foreach (Tuple<double, double> RandomTime in list)
                {
                    Console.WriteLine($"\tTime: {Math.Round(RandomTime.Item2, 2)}\tRandom Number: {RandomTime.Item1}");
                }
                counter++;
            }
            int number;
            while (true)
            {
                try
                {
                    Console.Write("\nEnter the number of UA's: ");
                    number = Convert.ToInt32(Console.ReadLine());
                    if (number > 0) {
                        break;
                    }
                    Console.WriteLine("Positive number is required");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.WriteLine("Calculating probability");
            double equals = 0, bigger = 0, lessOrEquals = 0;
            foreach (List<Tuple<double, double>> list in model.RandomAndTime)
            {
                if (list.Count == number)
                {
                    equals++;
                }
                if (list.Count > number)
                {
                    bigger++;
                }
                if (list.Count <= number)
                {
                    lessOrEquals++;
                }
            }
            Console.WriteLine($"Exactly {number} UA's: {(double) equals / iterations}");
            Console.WriteLine($"More than {number} UA's: {(double) bigger / iterations}");
            Console.WriteLine($"No more than {number} UA's: {(double) lessOrEquals / iterations}");
        }
    }
}

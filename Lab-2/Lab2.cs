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
                Console.Write("Введіть λ: ");
                lambda = Convert.ToDouble(Console.ReadLine());
                try
                {
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
                Console.Write("Введіть час T: ");
                time = Convert.ToDouble(Console.ReadLine());
                try
                {
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
                Console.Write("Введіть число ітерацій: ");
                iterations = Convert.ToInt32(Console.ReadLine());
                try
                {
                    model = new Model(lambda, time, iterations);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("Генеруємо потоки випадкових подій");
            model.GenerateStream();
            int counter = 1;
            foreach (List<Tuple<double, double>> list in model.RandomAndTime)
            {
                Console.WriteLine($"\nІтерація №{counter}");
                Console.WriteLine($"{new string('=', 20)}");
                foreach (Tuple<double, double> RandomTime in list)
                {
                    Console.WriteLine($"\tЧас: {Math.Round(RandomTime.Item2, 2)}\tВипадкове число: {RandomTime.Item1}");
                }
                counter++;
            }
            int number;
            while (true)
            {
                Console.Write("\nВведіть число НСД: ");
                number = Convert.ToInt32(Console.ReadLine());
                if (number > 0) {
                    break;
                }
                Console.WriteLine("Потрібно додатнє число");
            }

            Console.WriteLine("Обраховуємо ймовірності");
            int equals = 0, bigger = 0, lessOrEquals = 0;
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
            Console.WriteLine($"Рівно {number} НСД: {(double) equals / iterations}");
            Console.WriteLine($"Більше як {number} НСД: {(double) bigger / iterations}");
            Console.WriteLine($"Не більше як {number} НСД: {(double) lessOrEquals / iterations}");
        }
    }
}

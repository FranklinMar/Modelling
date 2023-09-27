using System;
using System.Collections.Generic;

namespace Lab_2
{ 
	public class Model
	{
		readonly Random Generator = new Random();
		public List<Tuple<double, double>> [] RandomAndTime { get; private set; }
		private double lambda;
		public double Lambda { 
			get 
			{ 
				return lambda; 
			} 
			set
			{
				lambda = value > 0 ? value : throw new Exception("Positive number is required");
			}
		}
		private double time;
		public double Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value > 0 ? value : throw new Exception("Positive number is required");
			}
		}
		private int iterations;
		public int Iterations
		{
			get
			{
				return iterations;
			}
			set
			{
				iterations = value > 0 ? value : throw new Exception("Positive number is required");
			}
		}

		public Model(double lambda, double time, int iterations)
		{
			Lambda = lambda;
			Time = time;
			Iterations = iterations;
		}

		public void GenerateStream()
        {
			RandomAndTime = new List<Tuple<double, double>>[Iterations];
			double timer, random, randomTime;
			for (int number = 0; number < Iterations; number++){
				RandomAndTime[number] = new List<Tuple<double, double>>();
				timer = 0;
				do
				{
					random = Generator.NextDouble();
					//random = Generator.Next(0, 101) / 100.0;
					randomTime = -1 / Lambda * Math.Log(random);
					if (timer + randomTime > Time) {
						break;
					}
					timer += randomTime;
					RandomAndTime[number].Add(new Tuple<double, double>(random, timer));
					/*Console.WriteLine($"{new string('-', 20)}");
					Console.WriteLine($"Iteration #{number}\n");
					random = Generator.Next(0, 101) / 100.0;
					Console.WriteLine($"Random number: {random}");
					random = -1 / Lambda * Math.Log(random);
					Console.WriteLine($"Time interval: {random}");
					timer += random;
					Console.WriteLine($"Current time: {timer}");
					number += 1;*/
				} while (timer <= Time);
			}

		}
	}
}

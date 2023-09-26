using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Symbolics;
using Expression = MathNet.Symbolics.SymbolicExpression;

namespace Lab_2
{
    class Lagrange
    {
        static Expression x = Expression.Variable("x");

        public int Count { get; private set; }
        public Expression Polynome { get; private set; }

        public Lagrange(List<double[]> ValueMap)
        {
            if (ValueMap.Count < 1)
            {
                throw new Exception("No values added");
            }

            Expression Function = 0;
            Expression Nominator;
            Expression Denominator;

            for (int i = 0; i < ValueMap.Count; i++)
            {
                
                double xi = ValueMap[i][0], yi = ValueMap[i][1];
                Nominator = 1;
                Denominator = 1;
                for (int j = 0; j < ValueMap.Count; j++)
                {
                    double xj = ValueMap[j][0];

                    if (i != j)
                    {
                        Nominator *= (x - xj);
                        Denominator *= (xi - xj);
                    }
                }
                Console.Write(((Nominator) / (Denominator) * yi).ToString());
                if (ValueMap.Count - 1 != i)
                {
                    Console.Write(" + ");
                }
                Function += (Nominator) / (Denominator) * yi;
            }
            Console.Write("\n");
            Polynome = Function;
            Count = ValueMap.Count;
        }

        public string Simplify()
        {
            return Infix.Format(Algebraic.Expand(Polynome.Expression));
        }

        public double Evaluate(double X)
        {
            return Polynome.Evaluate(new Dictionary<string, FloatingPoint>() { { "x", X } }).RealValue;
        }
    }
}

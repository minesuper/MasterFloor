using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialCountityCalculation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MaterialCalculation.CountityMaterialCalc(1, 1, 3, 12.8m, 13.9m));
            Console.Read();
        }
    }
}

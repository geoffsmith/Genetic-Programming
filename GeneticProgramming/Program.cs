using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneticProgramming.FitnessFunctions;
using GeneticProgramming.NodeMutators;
using GeneticProgramming.Selection;

namespace GeneticProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            Population population = new Population(500, new StandardDeviationFitnessFunction(), new SimpleReproduction(),
                                                   new SimpleNodeMutator(), new RouletteSelection());
            for (int i = 0; i < 50000; ++i)
            {
                Console.WriteLine("Iteration {0}", i);
                population.ProcessGeneration();
            }

            Console.WriteLine("Finished");

            Console.ReadLine();
        }
    }
}

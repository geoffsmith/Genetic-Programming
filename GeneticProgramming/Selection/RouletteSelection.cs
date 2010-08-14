using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Selection
{
    public class RouletteSelection : ISelection
    {
        #region Implementation of ISelection

        public INode SelectNode()
        {
            // Get a random number upto the total fitness
            double nodeTarget = RandomUtil.Random.NextDouble()*this.fitnessTotal;
            double sum = 0.0;
            foreach (var fitness in this.sortedFitness)
            {
                sum += fitness.Fitness;
                if (sum >= nodeTarget)
                {
                    return this.nodes[fitness.Key];
                }
            }

            return this.nodes[0];
        }

        public void Initialise(IList<INode> nodes, IDictionary<int, double> fitnesses)
        {
            this.fitnessTotal = fitnesses.Values.Sum(o => 1.0 / o);
            this.nodes = nodes;

            this.sortedFitness = new List<KeyAndFitness>();
            foreach (KeyValuePair<int, double> pair in fitnesses)
            {
                sortedFitness.Add(new KeyAndFitness{Fitness = 1.0 / pair.Value, Key = pair.Key});
            }

            // Sort by fitness, descending order
            sortedFitness.Sort(new KeyAndFitnessComparer());
        }

        #endregion

        private double fitnessTotal;
        private IList<INode> nodes;
        private List<KeyAndFitness> sortedFitness;
    }

    class KeyAndFitness
    {
        public int Key { get; set; }
        public double Fitness { get; set; }
    }

    class KeyAndFitnessComparer : IComparer<KeyAndFitness>
    {
        #region Implementation of IComparer<in KeyAndFitness>

        public int Compare(KeyAndFitness x, KeyAndFitness y)
        {
            return y.Fitness.CompareTo(x.Fitness);
        }

        #endregion
    }
}
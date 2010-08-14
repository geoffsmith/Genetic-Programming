using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticProgramming.Interfaces;
using System.Linq;
using GeneticProgramming.Utility;

namespace GeneticProgramming
{
    public class Population : IPopulation
    {
        #region Public methods

        public Population(int size, IFitnessFunction fitnessFunction, IReproduction reproductionFunction, INodeMutator mutator, ISelection selection)
        {
            this.populationSize = size;
            this.fitnessFunction = fitnessFunction;
            this.reproductionFunction = reproductionFunction;
            this.mutator = mutator;
            this.selector = selection;

            this.fitnessFunction.Initialise();

            // Create the initial population
            for (int i = 0; i < size; i++)
            {
                try
                {
                    NodeContext zeroContext = new NodeContext();
                    zeroContext.AvailableCollections = fitnessFunction.GetCollections();
                    zeroContext.AvailableInputs = fitnessFunction.GetInputs();

                    INode candidateNode = NodeFactory.GenerateNode(zeroContext);

                    // Make sure we have a decent candidate (i.e. not too large)
                    double fitness = this.fitnessFunction.CalculateFitness(candidateNode);
                    if (fitness == Double.MaxValue) continue;

                    this.population.Add(NodeFactory.GenerateNode(zeroContext));
                }
                catch (StackOverflowException)
                {
                }
            }
        }

        /// <summary>
        /// Generate a new population based on the fitness of the previous population's
        /// individuals
        /// </summary>
        public void ProcessGeneration()
        {
            NodeContext zeroContext = new NodeContext();
            zeroContext.AvailableCollections = fitnessFunction.GetCollections();
            zeroContext.AvailableInputs = fitnessFunction.GetInputs();

            // Create a new set of tests
            fitnessFunction.Initialise();

            Console.WriteLine("Calculate");

            // Get the fitness measure for each individual
            var fitnesses = new ThreadSafeDictionary<int, double>();
            for (int i = 0; i < this.population.Count; ++i)
            {
                fitnesses[i] = this.fitnessFunction.CalculateFitness(this.population[i]);
                fitnesses[i] = Math.Abs(fitnesses[i]);
            }

            //var loopResult = Parallel.For(0, this.population.Count, delegate(int i)
            //{
            //    fitnesses[i] = Math.Abs(this.fitnessFunction.CalculateFitness(this.population[i]));
            //    //fitnesses[i] = Math.Abs(fitnesses[i]);
            //});

            //if (!loopResult.IsCompleted)
            //{
            //    Console.WriteLine("Loop failed");
            //}

            this.selector.Initialise(this.population, fitnesses);


            var bestIndividuals = from k in fitnesses.Keys orderby fitnesses[k] ascending select this.population[k];

            INode best = bestIndividuals.FirstOrDefault();
            this.BestIndividual = best;

            // Update the best fitness
            this.BestFitness = fitnesses.Values.Min();
            this.AverageFitness = fitnesses.Values.Sum(f => f < Double.MaxValue ? f : 0)/fitnesses.Values.Count(f => f < Double.MaxValue);

            Console.WriteLine("Best fitness: {0} {1}", fitnesses.Values.Min(), best);

            this.PrintAverageTreeSize();

            List<INode> newPopulation = new List<INode>();

            // Always keep the best 10%
            newPopulation.AddRange(bestIndividuals.Take(this.populationSize / 10));

            while (newPopulation.Count < this.populationSize)
            {
                INode parentA = this.selector.SelectNode();
                INode parentB = this.selector.SelectNode();
                INode child = this.reproductionFunction.Reproduce(parentA, parentB);
                //INode child = (INode) this.selector.SelectNode().Clone();
                this.mutator.Mutate(ref child, zeroContext);
                newPopulation.Add(child);

            }

            // Replace the old population);
            this.population = newPopulation;
        }

        private void PrintAverageTreeSize()
        {
            double total = this.population.Sum(i => i.GetNodes().Count);
            Console.WriteLine("Average tree size: {0}", total / (double)this.populationSize);
        }

        #endregion

        #region Public properties

        public double BestFitness { get; set; }

        public List<INode> Individuals
        {
            get { return this.population; }
        }

        public INode BestIndividual { get; private set; }

        public double AverageFitness { get; private set; }

        #endregion

        #region Private fields

        private List<INode> population = new List<INode>();

        private readonly int populationSize;

        private readonly IFitnessFunction fitnessFunction;

        private readonly IReproduction reproductionFunction;

        private readonly INodeMutator mutator;

        private readonly ISelection selector;

        #endregion
    }
}
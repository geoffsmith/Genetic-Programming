using System;
using System.Linq;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;
using GeneticProgramming.Utility;

namespace GeneticProgramming.Populations
{
    public class TwoIslandsPopulation : IPopulation
    {
        #region Public methods

        public TwoIslandsPopulation(int size, IFitnessFunction fitnessFunction, IReproduction reproductionFunction, INodeMutator mutator, ISelection selection)
        {
            this.populationSize = size;
            this.fitnessFunction = fitnessFunction;
            this.reproductionFunction = reproductionFunction;
            this.mutator = mutator;
            this.selector = selection;

            this.fitnessFunction.Initialise();

            // The main population needs initializing
            this.IntialisePopulation(this.mainPopulation);

            this.IntialisePopulation(this.secondaryPopulation);

        }
        #endregion

        #region Implementation of IPopulation

        public void ProcessGeneration()
        {
            // Run a cycle for the secondary population
            if (this.generation % TwoIslandsPopulation.secondaryMergeGenerations == 0)
            {
                this.CycleGeneration(this.secondaryPopulation, this.mainPopulation);
            }
            else
            {
                this.CycleGeneration(this.secondaryPopulation);
            }

            // Run a cycle for the main generation
            this.CycleGeneration(this.mainPopulation);

            this.generation++;
        }

        public double BestFitness { get; private set; }

        public INode BestIndividual { get; private set; }

        public double AverageFitness { get; private set; }

        #endregion

        #region Private methods

        private void IntialisePopulation(ICollection<INode> population)
        {
            population.Clear();

            // Create the initial main population
            for (int i = 0; i < this.populationSize; i++)
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

                    population.Add(NodeFactory.GenerateNode(zeroContext));
                }
                catch (StackOverflowException)
                {
                }
            }
            
        }

        private void CycleGeneration(List<INode> population, List<INode> mergePopulation = null)
        {
            NodeContext zeroContext = new NodeContext();
            zeroContext.AvailableCollections = fitnessFunction.GetCollections();
            zeroContext.AvailableInputs = fitnessFunction.GetInputs();

            // Create a new set of tests
            fitnessFunction.Initialise();

            // Get the fitness measure for each individual
            var fitnesses = new ThreadSafeDictionary<int, double>();
            for (int i = 0; i < population.Count; ++i)
            {
                fitnesses[i] = this.fitnessFunction.CalculateFitness(population[i]);
                fitnesses[i] = Math.Abs(fitnesses[i]);
            }

            this.selector.Initialise(population, fitnesses);

            var bestIndividuals = from k in fitnesses.Keys orderby fitnesses[k] ascending select population[k];

            INode best = bestIndividuals.FirstOrDefault();
            this.BestIndividual = best;

            // Update the best fitness
            this.BestFitness = fitnesses.Values.Min();

            List<INode> newPopulation = new List<INode>();

            if (mergePopulation == null)
            {
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
    
                // Replace the old population;
                population.Clear();
                population.AddRange(newPopulation);
            }
            else
            {
                mergePopulation.AddRange(bestIndividuals.Take(this.populationSize / 10));

                // Reinitialise the population
                this.IntialisePopulation(population);
            }
        }

        #endregion

        #region Private fields

        private readonly List<INode> mainPopulation = new List<INode>();
        
        private List<INode> secondaryPopulation = new List<INode>();

        private readonly int populationSize;

        private readonly IFitnessFunction fitnessFunction;

        private readonly IReproduction reproductionFunction;

        private readonly INodeMutator mutator;

        private readonly ISelection selector;

        private int generation;

        private const int secondaryMergeGenerations = 50;

        #endregion
    }
}
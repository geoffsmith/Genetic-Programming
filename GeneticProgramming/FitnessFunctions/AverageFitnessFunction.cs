using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;
using GeneticProgramming.Nodes;

namespace GeneticProgramming.FitnessFunctions
{
    public class AverageFitnessFunction : IFitnessFunction
    {
        #region Implementation of IFitnessFunction

        public double CalculateFitness(INode individual)
        {
            // Don't allow nodes to get too big
            if (individual.GetNodes().Count > 30) return 1000;

            if (this.perfectNode == null) this.CreatePerfectNode();

            // Keep track the average error
            double totalError = 0;
            const int numberTests = 50;

            for (int i = 0; i < numberTests; ++i)
            {
                int top = RandomUtil.Random.Next(1000);
                int size = RandomUtil.Random.Next(10);
                if (size == 0) size = 1;
                double expectedTotal = 0.0;
                List<double> input = new List<double>();
                for (int j = 0; j < size; ++j)
                {
                    double expectedItem = RandomUtil.Random.Next(top);
                    input.Add(expectedItem);
                    expectedTotal += expectedItem;
                }
                double expectedResult = expectedTotal/(double) size;

                List<List<double>> inputs = new List<List<double>>();
                inputs.Add(input);

                double error = expectedResult - individual.Calculate(new List<double>(), inputs);
                error = Math.Abs(error);
                error = Math.Abs(Math.Sqrt(0.1*error));
                totalError += error;
                
                // Add some error if the error is too large taking account of the size
                //if (error > 0.3) totalError += individual.GetNodes().Count/100.0;

            }


            return totalError/(double)(numberTests);
        }

        public List<int> GetInputs()
        {
            return new List<int>();
        }

        public List<int> GetCollections()
        {
            return new List<int>() {0};
        }

        public void Initialise()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private methods

        private void CreatePerfectNode()
        {
            INode node = new SumNode(new NodeContext(), 0, new InputVariableNode(0, new NodeContext()));
            INode main = new DivideNode(new NodeContext(), node, new CollectionSizeNode(0, new NodeContext()));
            this.perfectNode = main;
        }

        #endregion

        #region Private fields

        private INode perfectNode;

        #endregion
    }
}
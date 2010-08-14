using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Interfaces;
using GeneticProgramming.Nodes;

namespace GeneticProgramming.FitnessFunctions
{
    public class StandardDeviationFitnessFunction : IFitnessFunction
    {
        #region Implementation of IFitnessFunction

        public double CalculateFitness(INode individual)
        {
            // Don't allow nodes to get too big
            if (individual.GetNodes().Count > 30) return 1000;

            // We don't allow an individual to take more than 1 second to test
            DateTime startTime = DateTime.Now;

            // Keep track the average error
            double totalError = 0;

            // Keep track of results to make sure that we have some non-zero
            bool haveNonZero = false;

            //INode ideal = MakeVarianceNode();

            foreach (TestCase testCase in this.testCases)
            {
                double calculate = individual.Calculate(new List<double>(), testCase.Inputs);

                if (calculate != 0) haveNonZero = true;

                double error = testCase.ExpectedResult - calculate;

                //error = Math.Abs(Math.Sqrt(0.1*error));
                if (testCase.ExpectedResult != 0) error = error/testCase.ExpectedResult;
                totalError += Math.Abs(error);
                
                // Add some error if the error is too large taking account of the size
                //if (error > 0.3) totalError += individual.GetNodes().Count/100.0;

                // If we've gone over a second, stop with a high error
                if ((DateTime.Now - startTime).TotalMilliseconds > StandardDeviationFitnessFunction.maximumRunLength)
                    return Double.MaxValue;
            }

            if (!haveNonZero) return Double.MaxValue;

            return totalError/(double)(this.testCases.Count);
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
            if (this.initialise) return;
            this.initialise = true;

            this.testCases.Clear();
            const int numberTests = 50;
            for (int i = 0; i < numberTests; ++i)
            {
                int top = RandomUtil.Random.Next(100, 1000);
                int size = RandomUtil.Random.Next(10);
                if (size == 0) size = 1;
                List<double> input = new List<double>();
                for (int j = 0; j < size; ++j)
                {
                    double expectedItem = RandomUtil.Random.Next(top);
                    input.Add(expectedItem);
                }
                double expectedResult = this.StandardDeviation(input);

                List<List<double>> inputs = new List<List<double>> {input};

                this.testCases.Add(new TestCase{ExpectedResult = expectedResult, Inputs = inputs});
            }
        }

        #endregion

        #region Private methods

        private double StandardDeviation(List<double> items)
        {
            if (items.Count == 0) return 0;

            double mean = this.Mean(items);
            double sum = items.Sum(i => Math.Pow(i - mean, 2));
            //return Math.Sqrt(sum/items.Count);
            return sum/items.Count;
        }

        private double Mean(List<double> items)
        {
            if (items.Count == 0) return 0;

            double sum = items.Sum();
            return sum/(double)items.Count;
        }

        private INode MakeMeanNode()
        {
            INode sumNode = new SumNode(new NodeContext(), 0, new InputVariableNode(1, new NodeContext()));
            INode divide = new DivideNode(new NodeContext(), sumNode, new CollectionSizeNode(0, new NodeContext()));
            return divide;
        }

        private INode MakeVarianceNode()
        {
            INode mean1 = this.MakeMeanNode();
            INode mean2 = this.MakeMeanNode();
            INode left1 = new InputVariableNode(0, new NodeContext());
            INode left2 = new InputVariableNode(0, new NodeContext());
            INode sub1 = new SubtractNode(new NodeContext(), left1, mean1);
            INode sub2 = new SubtractNode(new NodeContext(), left2, mean2);
            INode mult = new MultiplyNode(new NodeContext(), sub1, sub2);
            INode sum = new SumNode(new NodeContext(), 0, mult);
            INode divide = new DivideNode(new NodeContext(), sum, new CollectionSizeNode(0, new NodeContext()));
            return divide;
        }

        #endregion

        #region Private fields

        private List<TestCase> testCases = new List<TestCase>();

        private bool initialise = false;

        private const int maximumRunLength = 25;

        #endregion
    }

    class TestCase
    {
        public List<List<double>> Inputs { get; set; }
        public double ExpectedResult { get; set; }
    }
}
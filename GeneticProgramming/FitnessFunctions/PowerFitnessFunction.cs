using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.FitnessFunctions
{
    public class PowerFitnessFunction : IFitnessFunction
    {
        #region Implementation of IFitnessFunction

        public double CalculateFitness(INode individual)
        {
            // Keep track the average error
            double totalError = 0;
            const int numberTests = 50;

            for (int i = 0; i < numberTests; ++i)
            {
                int input = RandomUtil.Random.Next(100);
                double expectedResult = input*input;

                double error = expectedResult - individual.Calculate(new List<double>{input}, new List<List<double>>());
                error = Math.Abs(error);
                totalError += Math.Abs(Math.Sqrt(0.1*error));
            }

            return totalError/numberTests;
        }

        public List<int> GetInputs()
        {
            throw new NotImplementedException();
        }

        public List<int> GetCollections()
        {
            throw new NotImplementedException();
        }

        public void Initialise()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
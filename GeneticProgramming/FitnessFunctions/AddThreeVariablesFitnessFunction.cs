using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.FitnessFunctions
{
    public class AddThreeVariablesFitnessFunction : IFitnessFunction
    {
        #region Implementation of IFitnessFunction

        public double CalculateFitness(INode individual)
        {
            // Keep track the average error
            double totalError = 0;
            const int numberTests = 50;

            for (int i = 0; i < numberTests; ++i)
            {
                int input1 = RandomUtil.Random.Next(100);
                int input2 = RandomUtil.Random.Next(100);
                int input3 = RandomUtil.Random.Next(100);
                int input4 = RandomUtil.Random.Next(100);
                if (input4 == 0) input4 = 1;
                double expectedResult = (double)input1 + (double)input2*(double)input3/(double)input4;

                double error = expectedResult - individual.Calculate(new List<double>{input1, input2, input3, input4}, new List<List<double>>());
                error = Math.Abs(error);
                error = Math.Abs(Math.Sqrt(0.1*error));
                totalError += error;
                
                // Add some error if the error is too large taking account of the size
                //if (error > 0.3) totalError += individual.GetNodes().Count/100.0;
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
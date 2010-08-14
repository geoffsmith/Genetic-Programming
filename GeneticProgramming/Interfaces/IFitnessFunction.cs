using System.Collections.Generic;

namespace GeneticProgramming.Interfaces
{
    public interface IFitnessFunction
    {
        double CalculateFitness(INode individual);

        List<int> GetInputs();

        List<int> GetCollections();

        void Initialise();
    }
}
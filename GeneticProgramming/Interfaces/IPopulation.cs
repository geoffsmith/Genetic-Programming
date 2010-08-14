namespace GeneticProgramming.Interfaces
{
    public interface IPopulation
    {
        void ProcessGeneration();

        double BestFitness { get; }

        INode BestIndividual { get; }

        double AverageFitness { get; }
    }
}
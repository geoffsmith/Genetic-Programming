namespace GeneticProgramming.Interfaces
{
    public interface IReproduction
    {
        INode Reproduce(INode a, INode b);
    }
}
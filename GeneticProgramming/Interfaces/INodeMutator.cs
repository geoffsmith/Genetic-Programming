namespace GeneticProgramming.Interfaces
{
    public interface INodeMutator
    {
        void Mutate(ref INode node, NodeContext zeroContext);
    }
}
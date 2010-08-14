using System.Collections.Generic;

namespace GeneticProgramming.Interfaces
{
    public interface ISelection
    {
        INode SelectNode();

        void Initialise(IList<INode> nodes, IDictionary<int, double> fitnesses);
    }
}
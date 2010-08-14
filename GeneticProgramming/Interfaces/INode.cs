using System;
using System.Collections.Generic;

namespace GeneticProgramming.Interfaces
{
    public interface INode : ICloneable
    {
        double Calculate(List<double> inputs, List<List<double>> collections);

        List<INode> GetNodes();

        void ReplaceChild(INode newChild);

        NodeContext Context { get; set; }
    }
}

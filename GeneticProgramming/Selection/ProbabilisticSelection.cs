using System;
using System.Collections.Generic;
using System.Linq;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Selection
{
    public class ProbabilisticSelection : ISelection
    {
        #region Implementation of ISelection

        public INode SelectNode()
        {
            // Get a random number between 0 - 1, this is the area under the curve. We
            // are trying to find an x value for this area
            double x = RandomUtil.Random.NextDouble();

            // convert the area into a value on the x axis
            double percent =  1.0 - Math.Pow(1 - x, 0.25);

            // Get the index for the given percent
            int index = Convert.ToInt32(percent*(this.sortedNodes.Count - 1));

            return this.sortedNodes[index];
        }

        public void Initialise(IList<INode> nodes, IDictionary<int, double> fitnesses)
        {
            // Sort nodes by fitness
            this.sortedNodes = new List<INode>(from k in fitnesses.Keys orderby fitnesses[k] ascending select nodes[k]);
        }

        #endregion

        private List<INode> sortedNodes;
    }
}
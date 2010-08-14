using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming
{
    public class SimpleReproduction : IReproduction
    {
        #region Implementation of IReproduction

        /// <summary>
        /// A very simple reproduction algorithm. It starts with parent A and takes a single node
        /// from each parent and performs a crossover with those nodes.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public INode Reproduce(INode a, INode b)
        {
            // Clone parent A to start with
            INode child = (INode) a.Clone();

            // Get all the nodes in the child and parent b
            List<INode> childNodes = child.GetNodes();
            List<INode> bNodes = b.GetNodes();

            // Get a random node from the child and b
            INode childCrossoverNode = childNodes[RandomUtil.Random.Next(childNodes.Count - 1)];
            List<INode> bCandidates = FindNodeWithMatchingContext(childCrossoverNode, bNodes);
            if (bCandidates.Count == 0) return child;
            INode bCrossoverNode = bCandidates[RandomUtil.Random.Next(bCandidates.Count)];

            childCrossoverNode.ReplaceChild((INode) bCrossoverNode.Clone());
            return child;
        }

        private static List<INode> FindNodeWithMatchingContext(INode node, List<INode> candidates)
        {
            List<INode> results = new List<INode>();
            foreach (INode candidate in candidates)
            {
                if (candidate.Context.AvailableInputs.Count == node.Context.AvailableInputs.Count 
                    && candidate.Context.AvailableCollections.Count == node.Context.AvailableCollections.Count)
                {
                    results.Add(candidate);
                }
            }
            return results;

        }

        #endregion
    }
}
using System;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.NodeMutators
{
    public class SimpleNodeMutator : INodeMutator
    {
        private int mutationRate;

        public SimpleNodeMutator(int mutationRate = 25)
        {
            this.mutationRate = mutationRate;
        }

        #region Implementation of INodeMutator

        public void Mutate(ref INode node, NodeContext zeroContext)
        {
            // Only do a mutation 1/2 the time
            int doMutation = RandomUtil.Random.Next(100);
            if (doMutation < this.mutationRate) return;

            // Mutate a single node, by replacing one of its children
            var nodes = node.GetNodes();
            if (nodes.Count == 1)
            {
                node = NodeFactory.GenerateNode(zeroContext);
            }
            else
            {
                INode mutationNode = nodes[RandomUtil.Random.Next(nodes.Count)];
                INode newNode = NodeFactory.GenerateNode(mutationNode.Context);
                mutationNode.ReplaceChild(newNode);

                //Console.WriteLine("Before {0} after {1}", nodes.Count, node.GetNodes().Count);
            }


        }

        #endregion
    }
}
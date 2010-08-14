using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class CollectionSizeNode : INode
    {
        #region Public methods

        public CollectionSizeNode(NodeContext context)
        {
            this.Context = context;
            this.collectionIndex =
                context.AvailableCollections[RandomUtil.Random.Next(context.AvailableCollections.Count)];
        }

        public CollectionSizeNode(int collectionIndex, NodeContext context)
        {
            this.Context = (NodeContext) context.Clone();
            this.collectionIndex = collectionIndex;
        }

        public override string ToString()
        {
            return String.Format("CCount({0})", this.collectionIndex);
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new CollectionSizeNode(this.collectionIndex, this.Context);
        }

        #endregion

        #region Implementation of INode

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            return collections[this.collectionIndex].Count;
        }

        public List<INode> GetNodes()
        {
            return new List<INode>(){this};
        }

        public void ReplaceChild(INode newChild)
        {
        }

        public NodeContext Context { get;
            set;
        }

        #endregion

        #region Private fields

        private readonly int collectionIndex;

        #endregion
    }
}
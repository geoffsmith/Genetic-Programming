using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class SumNode : INode
    {

        #region Public methods

        public SumNode(NodeContext context)
        {
            this.Context = context;

            this.collectionIndex =
                context.AvailableCollections[RandomUtil.Random.Next(context.AvailableCollections.Count)];

            // Add a variable to this context
            this.Context.AvailableInputs.Add(this.Context.AvailableInputs.Count);

            this.sumFunction = NodeFactory.GenerateNode(context);

        }

        public SumNode(NodeContext context, int collectionIndex, INode sumFunction)
        {
            this.Context = (NodeContext) context.Clone();
            this.collectionIndex = collectionIndex;
            this.sumFunction = (INode) sumFunction.Clone();
        }

        public override string ToString()
        {
            return String.Format("Sum {0} ({1})", this.collectionIndex, this.sumFunction);
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new SumNode(this.Context, this.collectionIndex, this.sumFunction);
        }

        #endregion

        #region Implementation of INode

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            List<double> collection = collections[this.collectionIndex];
            double result = 0;
            foreach (double item in collection)
            {
                inputs.Add(item);
                result += this.sumFunction.Calculate(inputs, collections);
                inputs.RemoveAt(inputs.Count - 1);
            }
            return result;
        }

        public List<INode> GetNodes()
        {
            List<INode> result = new List<INode>();
            result.Add(this);
            result.AddRange(this.sumFunction.GetNodes());
            return result;
        }

        public void ReplaceChild(INode newChild)
        {
            this.sumFunction = newChild;
        }

        public NodeContext Context { get; set; }

        #endregion

        #region Private fields

        private readonly int collectionIndex;

        private INode sumFunction;


        #endregion
    }
}
using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class AddNode : INode
    {
        #region Public methods

        public AddNode(NodeContext context)
        {
            this.context = context;
            this.right = NodeFactory.GenerateNode(this.context);
            this.left = NodeFactory.GenerateNode(this.context);
        }

        public AddNode(NodeContext context, INode right, INode left)
        {
            this.context = (NodeContext) context.Clone();
            this.right = (INode) right.Clone();
            this.left = (INode) left.Clone();
        }

        public override string ToString()
        {
            return "(" + this.left.ToString() + " + " + this.right.ToString() + ")";
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new AddNode(this.context, this.right, this.left);
        }

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            return this.right.Calculate(inputs, collections) + this.left.Calculate(inputs, collections);
        }

        public List<INode> GetNodes()
        {
            List<INode> results = new List<INode>();
            results.Add(this);
            results.AddRange(this.right.GetNodes());
            results.AddRange(this.left.GetNodes());
            return results;
        }

        public void ReplaceChild(INode newChild)
        {
            if (RandomUtil.Random.Next(1) == 1)
            {
                this.right = newChild;
            }
            else
            {
                this.left = newChild;
            }
        }

        public NodeContext Context
        {
            get { return this.context; }
            set { this.context = value; }
        }

        #endregion

        #region Private fields

        private INode right;
        private INode left;
        private NodeContext context;

        #endregion
    }
}
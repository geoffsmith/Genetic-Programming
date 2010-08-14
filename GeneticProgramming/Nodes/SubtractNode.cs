using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class SubtractNode : INode
    {
        #region Public methods

        public SubtractNode(NodeContext context)
        {
            this.context = context;
            this.right = NodeFactory.GenerateNode(this.context);
            this.left = NodeFactory.GenerateNode(this.context);
        }

        public SubtractNode(NodeContext context, INode left, INode right)
        {
            this.context = (NodeContext) context.Clone();
            this.left = (INode) left.Clone();
            this.right = (INode) right.Clone();
        }

        public override string ToString()
        {
            return "(" + this.left.ToString() + " - " + this.right.ToString() + ")";
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new SubtractNode(this.context, this.left, this.right);
        }

        #endregion

        #region Implementation of INode

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            return this.left.Calculate(inputs, collections) - this.right.Calculate(inputs, collections);
        }

        public List<INode> GetNodes()
        {
            List<INode> results = new List<INode>();
            results.Add(this);
            results.AddRange(this.left.GetNodes());
            results.AddRange(this.right.GetNodes());
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

        private NodeContext context;

        private INode right;

        private INode left;

        #endregion
    }
}
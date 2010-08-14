using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class ConstantNode : INode
    {
        #region Public methods

        public ConstantNode(NodeContext context)
        {
            this.context = context;
            this.constant = RandomUtil.Random.Next(100);
        }

        public ConstantNode(NodeContext context, double constant)
        {
            this.context = (NodeContext) context.Clone();
            this.constant = constant;
        }

        public override string ToString()
        {
            return string.Format("{0}", this.constant);
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new ConstantNode(this.context, this.constant);
        }

        #endregion

        #region Implementation of INode

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            return this.constant;
        }

        public List<INode> GetNodes()
        {
            return new List<INode> {this};
        }

        public void ReplaceChild(INode newChild)
        {
        }

        public NodeContext Context
        {
            get { return this.context; }
            set { this.context = value; }
        }

        #endregion

        #region Private fields

        private double constant;
        private NodeContext context;

        #endregion
    }
}
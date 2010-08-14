using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class InputSizeNode : INode
    {
        #region Public methods

        public InputSizeNode(NodeContext context)
        {
            this.context = context;
        }

        public override string ToString()
        {
            return "Count()";
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new InputSizeNode((NodeContext) context.Clone());
        }

        #endregion

        #region Implementation of INode

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            return inputs.Count;
        }

        public List<INode> GetNodes()
        {
            return new List<INode>() {this};
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

        private NodeContext context;

        #endregion
    }
}
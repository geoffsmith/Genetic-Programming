using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class InputVariableNode : INode
    {
        #region Public methods

        public InputVariableNode(NodeContext context)
        {
            this.context = context;
            this.variableKey = context.AvailableInputs[RandomUtil.Random.Next(context.AvailableInputs.Count)];

        }

        public InputVariableNode(int variableKey, NodeContext context)
        {
            this.context = context;
            this.variableKey = variableKey;
        }

        public override string ToString()
        {
            return "{" + this.variableKey + "}";
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new InputVariableNode(this.variableKey, (NodeContext) this.context.Clone());
        }

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            return inputs[this.variableKey];
        }

        public List<INode> GetNodes()
        {
            var results = new List<INode> {this};
            return results;
        }

        public void ReplaceChild(INode newChild)
        {
            // Do nothing, doesn't have any children
        }

        public NodeContext Context
        {
            get { return this.context; }
            set { this.context = value; }
        }

        #endregion

        #region Private fields

        private readonly int variableKey;
        private NodeContext context;

        #endregion
    }
}
using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;

namespace GeneticProgramming.Nodes
{
    public class DivideNode : INode
    {
        #region Public methods

        public DivideNode(NodeContext context)
        {
            this.context = context;
            this.numerator = NodeFactory.GenerateNode(this.context);
            this.denomiator = NodeFactory.GenerateNode(this.context);
        }

        public DivideNode(NodeContext context, INode numerator, INode denomiator)
        {
            this.context = (NodeContext) context.Clone();
            this.numerator = (INode) numerator.Clone();
            this.denomiator = (INode) denomiator.Clone();
        }

        public override string ToString()
        {
            return "(" + this.numerator.ToString() + " / " + this.denomiator.ToString() + ")";
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            INode result = new DivideNode(this.context, this.numerator, this.denomiator);
            return result;
        }

        #endregion

        #region Implementation of INode

        public double Calculate(List<double> inputs, List<List<double>> collections)
        {
            // Avoid div-by-0, if the denominator is 0, just return 0
            double denominatorResult = this.denomiator.Calculate(inputs, collections);
            if (denominatorResult == 0)
            {
                return 0;
            }
            double result = this.numerator.Calculate(inputs, collections)/denominatorResult;
            return result;
        }

        public List<INode> GetNodes()
        {
            var results = new List<INode>();
            results.Add(this);
            results.AddRange(this.numerator.GetNodes());
            results.AddRange(this.denomiator.GetNodes());
            return results;
        }

        public void ReplaceChild(INode newChild)
        {
            if (RandomUtil.Random.Next(1) == 1)
            {
                this.numerator = newChild;
            }
            else
            {
                this.denomiator = newChild;
            }
        }

        public NodeContext Context
        {
            get { return this.context; }
            set { this.context = value; }
        }

        #endregion

        #region Private fields

        private INode numerator;
        private INode denomiator;
        private NodeContext context;

        #endregion
    }
}
using System;
using System.Collections.Generic;

namespace GeneticProgramming
{
    public class NodeContext : ICloneable
    {
        #region Public properties

        public int DepthLevel { get; set; }

        public List<int> AvailableInputs { get; set; }

        public List<int> AvailableCollections { get; set; }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            NodeContext result = new NodeContext();
            result.DepthLevel = this.DepthLevel;
            result.AvailableCollections = new List<int>(this.AvailableCollections);
            result.AvailableInputs = new List<int>(this.AvailableInputs);
            return result;
        }

        #endregion

        #region Public methods

        public NodeContext GenerateNewLevel()
        {
            NodeContext result = (NodeContext) this.Clone();
            result.DepthLevel++;
            return result;
        }

        public NodeContext()
        {
            this.AvailableCollections = new List<int>();
            this.AvailableInputs = new List<int>();
        }

        #endregion
    }
}
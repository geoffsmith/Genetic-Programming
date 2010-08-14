using System;
using System.Collections.Generic;
using GeneticProgramming.Interfaces;
using GeneticProgramming.Nodes;
using System.Linq;

namespace GeneticProgramming
{
    public static class NodeFactory
    {
        public static INode GenerateNode(NodeContext context)
        {
            INode result = null;
            if (context.DepthLevel > 20)
            {
                result = new ConstantNode(context.GenerateNewLevel());
                return result;
            }

            int probability = 10;

            while (result == null)
            {
                if (RandomUtil.Random.Next(100) < probability)
                {
                    result = new AddNode(context.GenerateNewLevel());
                }
                else if (RandomUtil.Random.Next(100) < probability)
                {
                    result = new SubtractNode(context.GenerateNewLevel());
                }
                else if (RandomUtil.Random.Next(100) < probability)
                {
                    result = new MultiplyNode(context.GenerateNewLevel());
                }
                else if (RandomUtil.Random.Next(100) < probability)
                {
                    result = new DivideNode(context.GenerateNewLevel());
                }
                else if (RandomUtil.Random.Next(100) < probability)
                {
                    result = new InputSizeNode(context.GenerateNewLevel());
                }
                else if (RandomUtil.Random.Next(100) < probability)
                {
                    result = new CollectionSizeNode(context.GenerateNewLevel());
                }
                else if (RandomUtil.Random.Next(100) < probability && context.AvailableInputs.Count > 0)
                {
                    result = new InputVariableNode(context.GenerateNewLevel());
                }
                else if (RandomUtil.Random.Next(100) < probability && context.AvailableCollections.Count > 0)
                {
                    result = new SumNode(context.GenerateNewLevel());
                }
            }


            //NodeFactory.SimplifyNode(ref result);

            return result;
        }

        private static void SimplifyNode(ref INode result)
        {
            // Check for an InputVariableNode in result
            if (result.GetNodes().Any(n => n.GetType() == typeof(InputVariableNode) || n.GetType() == typeof(InputSizeNode)
                || n.GetType() == typeof(CollectionSizeNode) || n.GetType() == typeof(SumNode)))
                return;

            // We don't have an InputVariableNode, so we can simplify into a single constant
            double constant = result.Calculate(new List<double>(), new List<List<double>>());
            result = new ConstantNode(result.Context, constant);
        }
    }
}
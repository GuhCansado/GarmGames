using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// using Unity.Entities;

namespace NinjutsuGames.StateMachine.Runtime
{
    /// <summary>
    /// Graph processor
    /// </summary>
    public class StateMachineGraphProcessor : BaseGraphProcessor
    {
        List<BaseNode> processList;

        /// <summary>
        /// Manage graph scheduling and processing
        /// </summary>
        /// <param name="graph">Graph to be processed</param>
        public StateMachineGraphProcessor(StateMachineAsset graph, GameObject context) : base(graph, context)
        {
        }

        public override void UpdateComputeOrder()
        {
            // processList = graph.nodes.OrderBy(n => n.computeOrder).ToList();
        }

        /// <summary>
        /// Process all the nodes following the compute order.
        /// </summary>
        public override void Run()
        {
            var initialNodes = graph.nodes.Where(n => n is TriggerNode or StartNode);
            foreach (var node in initialNodes)
            {
                node.OnProcess(context);
            }
            /*var count = processList.Count;

            for (var i = 0; i < count; i++)
                processList[i].OnProcess(context);*/
        }
    }
}
using UnityEngine;

// using Unity.Entities;

namespace NinjutsuGames.StateMachine.Runtime
{
    /// <summary>
    /// Graph processor
    /// </summary>
    public abstract class BaseGraphProcessor
    {
        protected StateMachineAsset graph;
        public GameObject context;

        /// <summary>
        /// Manage graph scheduling and processing
        /// </summary>
        /// <param name="graph">Graph to be processed</param>
        public BaseGraphProcessor(StateMachineAsset graph, GameObject context)
        {
            this.graph = graph;
            this.context = context;

            UpdateComputeOrder();
        }

        public abstract void UpdateComputeOrder();

        /// <summary>
        /// Schedule the graph into the job system
        /// </summary>
        public abstract void Run();

        public void RunNode(string nodeId, GameObject context)
        {
            graph.RunNode(nodeId, context);
        }

        public void StopNode(string nodeId, GameObject context)
        {
            graph.StopNode(nodeId, context);
        }
        
        public void DisableNode(string nodeId, GameObject context)
        {
            graph.DisableNode(nodeId, context);
        }
        
        public void EnableNode(string nodeId, GameObject context)
        {
            graph.EnableNode(nodeId, context);
        }

        public bool IsNodeEnabled(string nodeId, GameObject context)
        {
            return graph.IsNodeEnabled(nodeId, context);
        }
        
        public bool IsNodeRunning(string nodeId, GameObject context)
        {
            return graph.IsNodeRunning(nodeId, context);
        }
    }
}
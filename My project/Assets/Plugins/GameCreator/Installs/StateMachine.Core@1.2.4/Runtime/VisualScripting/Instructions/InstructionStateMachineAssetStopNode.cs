using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Stop State Machine Node")]
    [Description("Stops a node from State Machine asset")]

    [Category("State Machine/Asset/Stop State Machine Node")]

    [Parameter(
        "Node",
        "The node to execute from the specified State Machine"
    )]
    
    [Keywords("Stop", "Cancel", "Instruction", "Action", "State Machine", "Runner")]
    [Image(typeof(IconStateMachineOverlayRed), ColorTheme.Type.Blue, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionStateMachineAssetStopNode : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Node = GetNodeStateMachine.Create;
        [SerializeField, HideInInspector] private StateMachineAsset m_StateMachine = null;

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Stop {m_Node}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            var nodeId = m_Node.Get(args);
            m_StateMachine.StopNode(nodeId);
            return DefaultResult;
        }
    }
}
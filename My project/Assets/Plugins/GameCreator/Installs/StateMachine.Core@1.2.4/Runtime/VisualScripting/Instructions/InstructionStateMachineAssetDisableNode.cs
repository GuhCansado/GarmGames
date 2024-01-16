using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Disable State Machine Node")]
    [Description("Disables a node from State Machine asset")]

    [Category("State Machine/Asset/Disable State Machine Node")]

    [Parameter(
        "Node",
        "The node to execute from the specified State Machine"
    )]
    
    [Keywords("Disable", "Cancel", "Instruction", "Action", "State Machine", "Runner")]
    [Image(typeof(IconStateMachineOverlayRed), ColorTheme.Type.Blue, typeof(OverlayCross))]
    
    [Serializable]
    public class InstructionStateMachineAssetDisableNode : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Node = GetNodeStateMachine.Create;
        [SerializeField, HideInInspector] private StateMachineAsset m_StateMachine = null;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Disable {m_Node}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            var nodeId = m_Node.Get(args);
            m_StateMachine.DisableNode(nodeId);
            return DefaultResult;
        }
    }
}
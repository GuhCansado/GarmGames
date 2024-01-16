using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Run State Machine Node")]
    [Description("Executes a node from State Machine asset")]

    [Category("State Machine/Asset/Run State Machine Node")]

    [Parameter(
        "Node",
        "The node to execute from the specified State Machine"
    )]
    
    [Keywords("Execute", "Call", "Instruction", "Action", "State Machine", "Run")]
    [Image(typeof(IconStateMachineOverlayGreen), ColorTheme.Type.Blue, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class InstructionStateMachineAssetRunNode : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Node = GetNodeStateMachine.Create;
        [SerializeField, HideInInspector] private StateMachineAsset m_StateMachine = null;
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Run {m_Node}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            var nodeId = m_Node.Get(args);
            if (m_StateMachine == null)
            {
                Debug.LogWarning($"Run State Machine Node: State Machine is null");
                return DefaultResult;
            }
            m_StateMachine.RunNode(nodeId);
            return DefaultResult;
        }
    }
}
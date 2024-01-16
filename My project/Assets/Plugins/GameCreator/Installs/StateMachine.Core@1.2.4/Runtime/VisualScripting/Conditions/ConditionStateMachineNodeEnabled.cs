using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Node Is Enabled")]
    [Description("Returns true if the specified node is enabled")]

    [Category("State Machine/Asset/Node Enabled")]

    [Keywords("State Machine", "Is Running", "Run")]
    
    [Image(typeof(IconStateMachineOverlayGreen), ColorTheme.Type.Blue, typeof(OverlayTick))]
    
    [Serializable]
    public class ConditionStateMachineNodeEnabled : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetString m_Node = GetNodeStateMachine.Create;
        [SerializeField, HideInInspector] private StateMachineAsset m_StateMachine = null;
        protected override string Summary => $"Node {m_Node} enabled";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            return m_StateMachine && m_StateMachine.IsNodeEnabled(m_Node.Get(args));
        }
    }
}
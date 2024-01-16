using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Runner Node Is Enabled")]
    [Description("Returns true if the node is enabled on the specified State Machine Runner")]

    [Category("State Machine/Runner/Node Enabled")]

    [Keywords("State Machine", "Is Enabled", "Run")]
    
    [Image(typeof(IconStateMachineOverlayGreen), ColorTheme.Type.Yellow, typeof(OverlayTick))]
    
    [Serializable]
    public class ConditionStateMachineRunnerNodeEnabled : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetString m_Node = GetNodeStateMachine.Create;
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectTarget.Create();

        protected override string Summary => $"Node {m_Node} enabled on {m_Target}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            var runner = m_Target.Get<StateMachineRunner>(args);
            return runner != null && runner.IsNodeEnabled(m_Node.Get(args), args.Target ? args.Target : args.Self);
        }
    }
}
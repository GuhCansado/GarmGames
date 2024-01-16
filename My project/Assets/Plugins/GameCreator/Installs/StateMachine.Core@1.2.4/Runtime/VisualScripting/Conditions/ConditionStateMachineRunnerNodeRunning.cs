using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Node Is Enabled")]
    [Description("Returns true if the node is running on the specified State Machine Runner")]

    [Category("State Machine/Runner/Node Running")]

    [Keywords("State Machine", "Is Running", "Run")]
    
    [Image(typeof(IconStateMachineOverlayGreen), ColorTheme.Type.Yellow, typeof(OverlayArrowRight))]
    
    [Serializable]
    public class ConditionStateMachineRunnerNodeRunning : Condition
    {
        // PROPERTIES: ----------------------------------------------------------------------------
        
        [SerializeField] private PropertyGetString m_Node = GetNodeStateMachine.Create;
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectTarget.Create();

        protected override string Summary => $"Node {m_Node} running on {m_Target}";
        
        // RUN METHOD: ----------------------------------------------------------------------------

        protected override bool Run(Args args)
        {
            var runner = m_Target.Get<StateMachineRunner>(args);
            return runner != null && runner.IsNodeRunning(m_Node.Get(args), args.Target ? args.Target : args.Self);
        }
    }
}
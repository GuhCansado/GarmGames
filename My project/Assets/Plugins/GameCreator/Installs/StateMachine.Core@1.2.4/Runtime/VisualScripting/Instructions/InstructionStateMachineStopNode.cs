using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("Stop State Machine Runner Node")]
    [Description("Stops a State Machine node from an specific target runner")]

    [Category("State Machine/Runner/Stop State Machine Runner Node")]

    [Parameter(
        "Target",
        "The target GameObject that contains the State Machine Runner"
    )]

    [Parameter(
        "Node",
        "The node to execute from the specified State Machine"
    )]
    
    [Keywords("Stop", "Cancel", "Instruction", "Action", "State Machine", "Runner")]
    [Image(typeof(IconStateMachineOverlayRed), ColorTheme.Type.Yellow, typeof(OverlayMinus))]
    
    [Serializable]
    public class InstructionStateMachineStopNode : Instruction
    {
        // MEMBERS: -------------------------------------------------------------------------------

        [SerializeField] private PropertyGetString m_Node = GetNodeStateMachine.Create;
        [SerializeField] private PropertyGetGameObject m_Target = GetGameObjectTarget.Create();
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"Stop {m_Node} on {m_Target}";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            var runner = m_Target.Get<StateMachineRunner>(args);
            if (runner == null) return DefaultResult;
            runner.StopNode(m_Node.Get(args), args.Target ? args.Target : args.Self);
            return DefaultResult;
        }
    }
}
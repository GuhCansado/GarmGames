using System.Linq;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [System.Serializable, NodeMenuItem("Conditions Node")]
    public class ConditionsNode : BaseGameCreatorNode //, ICreateNodeFrom<Actions>
    {
        [Input("In", true)] public ConditionsPort input;
        [Input("In", true), Vertical] public TriggerPortIn input2;
        [Output("Out Fail")] public ConditionsPortOutFail failure;
        [Output("Out Success")] public ConditionsPortOutSuccess success;

        public ConditionList conditions = new();

        public override string name => "Conditions";

        public override string layoutStyle => "GraphProcessorStyles/ConditionsNode";

        protected override void Process(GameObject context, Args customArgs = null)
        {
            if(!Application.isPlaying) return;

            this.context = context;

            if(!CanExecute(context)) return;
            if(IsContextRunning.Contains(NodeId(context))) return;

            OnStartRunning(context);
            var args = customArgs ?? GetArgs(context);
            var result = conditions.GetCachedData(NodeId(context)).Check(args, CheckMode.And);
            var fieldName = result ? nameof(success) : nameof(failure);

            var nodes = outputPorts.FirstOrDefault(n => n.fieldName == fieldName)?.GetEdges().Where(e => e.inputNode.enabledForExecution && e.inputNode is ConditionsNode or ActionsNode or BranchNode or StateMachineNode)
                .Select(e => e.inputNode);
            foreach (var node in nodes)
            {
                node.OnProcess(context);
            }
            OnStopRunning(context, result);
        }
    }
}
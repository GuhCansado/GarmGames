using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [System.Serializable, NodeMenuItem("StateMachine Node")]
    public class StateMachineNode : BaseGameCreatorNode, ICreateNodeFrom<StateMachineAsset>
    {
        [Input("In", true), Vertical] public StateMachinePort input;

        public StateMachineAsset stateMachine;
        public override string name => "State Machine";
        public override string layoutStyle => "GraphProcessorStyles/StateMachineNode";

        protected override void Process(GameObject context, Args customArgs = null)
        {
            if(!Application.isPlaying) return;

            this.context = context;

            if (!stateMachine) return;
            if(!CanExecute(context)) return;
            if(IsContextRunning.Contains(NodeId(context))) return;

            OnStartRunning(context);

            var processor = new StateMachineGraphProcessor(stateMachine, context);
            processor.Run();

            OnStopRunning(context);
        }

        public bool InitializeNodeFromObject(StateMachineAsset value)
        {
            var result = value && value != StateMachineAsset.Active;
            if (!result) return result;
            nodeCustomName = value.name;
            stateMachine = value;
            return result;
        }
    }
}
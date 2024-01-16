using NinjutsuGames.StateMachine.Runtime;
using UnityEngine.UIElements;

namespace NinjutsuGames.StateMachine.Editor
{
    public class ProcessorView : PinnedElementView
    {
        private BaseGraphProcessor processor;

        public ProcessorView()
        {
            title = "Process panel";
        }

        protected override void Initialize(BaseGraphView graphView)
        {
            processor = new StateMachineGraphProcessor(graphView.graph, null);

            graphView.computeOrderUpdated += processor.UpdateComputeOrder;

            Button b = new Button(OnPlay) {name = "ActionButton", text = "Play !"};

            content.Add(b);
        }

        private void OnPlay()
        {
            processor.Run();
        }
    }
}
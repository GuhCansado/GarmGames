using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    public class StartNode : BaseGameCreatorNode
    {
        [Output("Out")] public EntryPort output;
        public override string layoutStyle => "GraphProcessorStyles/EntryNode";

        public override string name => "Entry";

        public override Color color => new(0.24f, 0.15f, 0.48f);

        public override bool deletable => false;
        public override bool isRenamable => false;
        
        public InstructionList instructions = new();
        
        [NonSerialized] private GameObject runner;

        protected override void Process(GameObject context, Args customArgs = null)
        {
            this.context = context;
            if(!Application.isPlaying) return;
            if(!CanExecute(context)) return;

            var args = customArgs ?? GetArgs(context);

            var runner = context.Get<ActionsRunner>();
            if(!runner) runner = context.Add<ActionsRunner>();
            if(runner.IsRunning) return;
            OnStartRunning(context);

            runner.Run(instructions.Clone(), args.Clone, (args) =>
            {
                if(!Application.isPlaying) return;
                OnStopRunning(context);
                RunChildNodes(args);
            });
        }
    }
}
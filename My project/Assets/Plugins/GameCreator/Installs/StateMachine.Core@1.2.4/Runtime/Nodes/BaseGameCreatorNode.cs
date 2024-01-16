using System;
using System.Collections.Generic;
using System.Linq;
using GameCreator.Runtime.Common;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    public class BaseGameCreatorNode : BaseNode
    {
        public override bool isRenamable => true;
        public override bool needsInspector => true;
        public override bool hideControls => true;
        
        public GameObject Context => context;

        public delegate void StartRunningDelegate(GameObject context);
        public delegate void StopRunningDelegate(GameObject context, bool result);

        public event StartRunningDelegate eventStartRunning;
        public event StopRunningDelegate eventStopRunning;
        [NonSerialized] private Args args;
        
        public event ProcessDelegate onExecutionDisabled;
        public event ProcessDelegate onExecutionEnabled;
        
        public void SetArgs(Args args)
        {
            this.args = args;
        }

        protected Args GetArgs(GameObject fallbackTarget)
        {
            return new Args(fallbackTarget);
        }

        protected IEnumerable<BaseNode> GetChildNodes(GameObject context)
        {
            return GetOutputNodes().Where(n => n.CanExecute(context) && n is ActionsNode or ConditionsNode or StateMachineNode or BranchNode);
        }

        protected void RunChildNodes(Args args)
        {
            var nodes = GetChildNodes(args.Self);

            foreach (var baseNode in nodes)
            {
                var node = (BaseGameCreatorNode) baseNode;
                node.SetArgs(args);
                node.OnProcess(args.Self);
            }
        }

        protected void OnStartRunning(GameObject context)
        {
            if(!Application.isPlaying) return;

            IsContextRunning.Add(NodeId(context));
            eventStartRunning?.Invoke(context);
        }

        protected void OnStopRunning(GameObject context, bool runResult = true)
        {
            if(!Application.isPlaying) return;

            IsContextRunning.Remove(NodeId(context));
            eventStopRunning?.Invoke(context, runResult);
        }

        public void Stop(GameObject context)
        {
            StopRunning(context);
        }
        
        public void Disable(GameObject context)
        {
            if(context)
            {
                if(!IsContextDisabled.Contains(NodeId(context)))
                {
                    IsContextDisabled.Add(NodeId(context));
                }
            }
            else enabledForExecution = false;
            
            onExecutionDisabled?.Invoke();
        }
        
        public void Enable(GameObject context)
        {
            if(context)
            {
                if(IsContextDisabled.Contains(NodeId(context))) 
                {
                    IsContextDisabled.Remove(NodeId(context));
                }
            }
            else enabledForExecution = true;

            onExecutionEnabled?.Invoke();
        }

        protected virtual void StopRunning(GameObject context) {}

        public bool IsRunning(GameObject context)
        {
            var canExecute = !context ? enabledForExecution : IsContextDisabled.Contains(NodeId(context));
            return canExecute && IsContextRunning.Contains(NodeId(!context ? this.context : context));
        }
    }
}
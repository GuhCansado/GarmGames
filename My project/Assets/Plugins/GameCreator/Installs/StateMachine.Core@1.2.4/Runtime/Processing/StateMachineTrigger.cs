using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [AddComponentMenu("")]
    public class StateMachineTrigger : Trigger
    {
        [NonSerialized] private Args m_Args;
        public new bool IsExecuting { get; private set; }

        public new event Action<Args> EventBeforeExecute;
        public new event Action EventAfterExecute;

        public void Init(Trigger trigger)
        {
            Awake();
        }

        public new async Task Execute(Args args)
        {
            if (IsExecuting) return;
            IsExecuting = true;
            
            EventBeforeExecute?.Invoke(args);

            try
            {
                await ExecInstructions(args);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), this);
            }

            IsExecuting = false;
            EventAfterExecute?.Invoke();
        }
        
        public new async Task Execute(GameObject target)
        {
            if (IsExecuting) return;
            
            m_Args.ChangeTarget(target);
            await Execute(m_Args);
        }
        
        public new async Task Execute()
        {
            if (IsExecuting) return;
            
            m_Args.ChangeTarget(null);
            await Execute(m_Args);
        }
    }
}
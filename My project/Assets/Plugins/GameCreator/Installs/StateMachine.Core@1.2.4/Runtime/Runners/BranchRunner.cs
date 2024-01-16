using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [AddComponentMenu("")]

    public class BranchRunner : MonoBehaviour, ICancellable
    {
        // MEMBERS: -------------------------------------------------------------------------------
        
        private Branch branch;

        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector | HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy;
        }
        
        // PROPERTIES: ----------------------------------------------------------------------------

        public bool IsRunning { get; private set; }
        public bool IsStopped { get; private set; }
        public ICancellable Cancellable { get; private set; }
        public bool IsCancelled => IsStopped || (Cancellable?.IsCancelled ?? false);
        
        // PUBLIC METHODS: ------------------------------------------------------------------------
        
        public void Run(Branch branch, Args args, Action<bool> onFinish)
        {
            this.branch = branch;
            enabled = true;
            _ = Evaluate(args, this, onFinish);
        }

        private async Task Evaluate(Args args, ICancellable cancellable, Action<bool> onFinish)
        {
            try
            {
                if (IsRunning) return;

                Cancellable = cancellable;
                
                IsRunning = true;
                IsStopped = false;
                
                var result = await branch.Evaluate(args, null);
                IsRunning = false;
                onFinish?.Invoke(result.Value);
                enabled = false;
            }
            catch (Exception)
            {
                //Debug.LogError(exception.ToString(), this);
            }
        }

        public void Cancel()
        {
            IsStopped = true;
        }

        private void OnDisable()
        {
            Cancel();
        }
    }
}
using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [AddComponentMenu("")]
    public class ActionsRunner : BaseActions
    {
        private void Awake()
        {
            hideFlags = HideFlags.HideInInspector | HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy;
        }

        public override void Invoke(GameObject self = null)
        {
            Args args = new Args(self != null ? self : gameObject, gameObject);
            _ = Run(args, null);
        }
        
        // ASYNC METHODS: -------------------------------------------------------------------------

        public async Task Run()
        {
            try
            {
                await ExecInstructions();
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), this);
            }
        }
        
        public void Run(InstructionList instructionList, Args args, Action<Args> onFinish)
        {
            m_Instructions = instructionList;
            enabled = true;
            _ = Run(args, onFinish);
        }

        private async Task Run(Args args, Action<Args> onFinish)
        {
            try
            {
                await ExecInstructions(args);
                onFinish?.Invoke(args);
                if(this) enabled = false;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.ToString(), this);
            }
        }
        
        // CANCEL METHOD: -------------------------------------------------------------------------

        public void Cancel()
        {
            StopExecInstructions();
            enabled = false;
        }
    }
}
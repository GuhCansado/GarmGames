using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Components
{
    [AddComponentMenu("Game Creator/Photon/Actions RPC")]
    [Icon(RuntimePaths.GIZMOS + "GizmoActions.png")]
    [RequireComponent(typeof(Actions))]
    public class ActionsRPC : BaseRPC
    {
        private Actions Actions => gameObject.Get<Actions>();

        protected override void Awake()
        {
            base.Awake();
            
            if(Actions == null)
            {
                Debug.LogError($"ActionsRPC: component at '{gameObject.name}' not found", gameObject);
            }
        }

        private void OnEnable()
        {
            Actions.EventInstructionStartRunning += OnStartRunning;
            Actions.EventInstructionEndRunning += OnEndRunning;
        }

        private void OnDisable()
        {
            Actions.EventInstructionStartRunning -= OnStartRunning;
            Actions.EventInstructionEndRunning -= OnEndRunning;
        }

        public override void Run(GameObject senderTag)
        {
            _ = Actions.Run(new Args(gameObject, senderTag));
        }
    }
}
using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Components
{
    [AddComponentMenu("Game Creator/Photon/Trigger RPC")]
    [Icon(RuntimePaths.GIZMOS + "GizmoTrigger.png")]
    [RequireComponent(typeof(Trigger))]
    public class TriggerRPC : BaseRPC
    {
        private Trigger Trigger => gameObject.Get<Trigger>();

        protected override void Awake()
        {
            base.Awake();
            
            if(Trigger == null)
            {
                Debug.LogError($"TriggerRPC: component at '{gameObject.name}' not found", gameObject);
            }
        }

        private void OnEnable()
        {
            Trigger.EventBeforeExecute += OnStartRunning;
            Trigger.EventAfterExecute += OnEndRunning;
        }

        private void OnDisable()
        {
            Trigger.EventBeforeExecute -= OnStartRunning;
            Trigger.EventAfterExecute -= OnEndRunning;
        }

        public override void Run(GameObject senderTag)
        {
            _ = Trigger.Execute(senderTag);
        }
    }
}
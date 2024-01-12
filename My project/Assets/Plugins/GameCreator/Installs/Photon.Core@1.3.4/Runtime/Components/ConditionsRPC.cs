using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Args = GameCreator.Runtime.Common.Args;

namespace NinjutsuGames.Photon.Runtime.Components
{
    [AddComponentMenu("Game Creator/Photon/Conditions RPC")]
    // [AddComponentMenu("")]
    [Icon(RuntimePaths.GIZMOS + "GizmoConditions.png")]
    public class ConditionsRPC : BaseRPC
    {
        private Conditions _conditions;
        private bool _run;
        private bool _lastRun;

        protected override void Awake()
        {
            base.Awake();
            
            _conditions = GetComponent<Conditions>();
            if(_conditions == null)
            {
                Debug.LogError($"SyncConditions: component at '{gameObject.name}' not found", gameObject);
            }
        }

        private void Update()
        {
            Debug.Log($"1 Run conditions: '{_conditions.IsRunning}' run: '{_run}' lastRun: '{_lastRun}'");

            if (_conditions && _conditions.IsRunning != _lastRun)
            {
                _lastRun = _conditions.IsRunning;

                if (_lastRun && !_run)
                {
                    // photonView.RPC(nameof(TriggerRPC), rpcTarget);
                    OnStartRunning();
                    _run = true;
                    Debug.Log($"Run conditions at '{gameObject.name}'");
                }
                if (!_lastRun) _run = false;
            }
        }
        
        public override void Run(GameObject senderTag)
        {
            _ = _conditions.Run(new Args(gameObject, senderTag));
        }

        /*[PunRPC]
        private void TriggerRPC(PhotonMessageInfo info)
        {
            _sender = info.Sender;
            _senderTag = info.Sender.TagObject as GameObject;
            Debug.Log($"TriggerRPC by: {_sender} isSenderLocal: {Equals(_sender, PhotonNetwork.LocalPlayer)}");
            if(!Equals(_sender, PhotonNetwork.LocalPlayer))
            {
                _ = _conditions.Run(new Args(gameObject, _senderTag));
            }
        }*/
    }
}
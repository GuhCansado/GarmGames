using System;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Components
{
    [RequireComponent(typeof(LocalNameVariables), typeof(PhotonView))]
    // [HelpURL("https://docs.gamecreator.io/gamecreator/variables/local-name-variables")]
    [AddComponentMenu("Game Creator/Photon/Local Name Variables Network")]
    [Icon(RuntimePaths.GIZMOS + "GizmoLocalNameVariables.png")]
    public class LocalNameVariablesNetwork : MonoBehaviourPunCallbacks
    {
        [SerializeField] private bool debug = false;

        private LocalNameVariables _localNameVars;
        private Hashtable _syncedVars = new();
        private bool _ignoreNextUpdate;

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            if(!_localNameVars) _localNameVars = GetComponent<LocalNameVariables>();
            _localNameVars.Register(OnVariableChange);
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            _localNameVars.Unregister(OnVariableChange);
        }

        private void OnVariableChange(string varId)
        {
            if(!PhotonNetwork.InRoom) return;
            if(_ignoreNextUpdate && !photonView.IsMine)
            {
                _ignoreNextUpdate = false;
                return;
            }
            var data = _localNameVars.Get(varId);
            if(_syncedVars.ContainsKey(varId) && _syncedVars[varId].Equals(data))
            {
                _ignoreNextUpdate = false;
                return;
            }
            
            if (!data.IsAllowedType()) return;
            
            if(!_syncedVars.ContainsKey(varId)) _syncedVars.Add(varId, data);
            else _syncedVars[varId] = data;
            if(debug) Debug.Log($"OnVariableChange: {varId}={data} type: {data.GetType()} _syncedVars: {_syncedVars.Count} allowed: {data.IsAllowedType()}");

            if (photonView.IsMine)
            {
                _ignoreNextUpdate = true;
                photonView.RPC(nameof(SyncVariable), RpcTarget.Others, varId, data, _ignoreNextUpdate);
                _ignoreNextUpdate = false;
            }
            else photonView.RPC(nameof(SyncVariableMaster), RpcTarget.MasterClient, varId, data);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!photonView.IsMine) return;

            if(debug) Debug.Log($"OnPlayerEnteredRoom: {newPlayer} _syncedVars: {_syncedVars.Count}");
            photonView.RPC(nameof(SyncUpdatedVariables), newPlayer, _syncedVars);
        }
        
        [PunRPC]
        private void SyncUpdatedVariables(Hashtable data)
        {
            if(debug) Debug.Log($"SyncUpdatedVariables: {data.Count}");

            foreach (var varData in data)
            {
                _localNameVars.Set(varData.Key.ToString(), varData.Value);
            }
            _syncedVars = data;
        }

        [PunRPC]
        private void SyncVariable(string varId, object value, bool ignoreNextUpdate)
        {
            _ignoreNextUpdate = ignoreNextUpdate;
            if(debug) Debug.Log($"SyncVariable: {varId} = {value} ignoreNextUpdate {_ignoreNextUpdate}");
            _localNameVars.Set(varId, value);
        }
        
        [PunRPC]
        private void SyncVariableMaster(string varId, object value, PhotonMessageInfo info)
        {
            if(debug) Debug.Log($"SyncVariableMaster: {varId} = {value} from {info.Sender}");
            _ignoreNextUpdate = true;
            _localNameVars.Set(varId, value);
        }
    }
}
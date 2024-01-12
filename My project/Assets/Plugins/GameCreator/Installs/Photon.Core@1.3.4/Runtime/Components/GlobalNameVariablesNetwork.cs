using System;
using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.Components
{
    [RequireComponent(typeof(PhotonView))]
    // [HelpURL("https://docs.gamecreator.io/gamecreator/variables/local-name-variables")]
    [AddComponentMenu("Game Creator/Photon/Global Name Variables Network")]
    [Icon(RuntimePaths.GIZMOS + "GizmoGlobalNameVariables.png")]
    public class GlobalNameVariablesNetwork : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GlobalNameVariables globalNameVariables;
        [SerializeField] private bool debug;

        private Hashtable _syncedVars = new();
        private bool _ignoreNextUpdate;

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
            if(!globalNameVariables) globalNameVariables = GetComponent<GlobalNameVariables>();
            globalNameVariables.Register(OnVariableChange);
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
            globalNameVariables.Unregister(OnVariableChange);
        }

        private void OnVariableChange(string varId)
        {
            if(_ignoreNextUpdate && !photonView.IsMine)
            {
                _ignoreNextUpdate = false;
                return;
            }
            var data = globalNameVariables.Get(varId);
            if(_syncedVars.ContainsKey(varId) && _syncedVars[varId].Equals(data))
            {
                _ignoreNextUpdate = false;
                return;
            }
            
            if (!data.IsAllowedType()) return;
            
            if(!_syncedVars.ContainsKey(varId)) _syncedVars.Add(varId, data);
            else _syncedVars[varId] = data;
            if(debug) Debug.Log($"Global OnVariableChange: {varId}={data} type: {data.GetType()} _syncedVars: {_syncedVars.Count} allowed: {data.IsAllowedType()}");

            if (photonView.IsMine)
            {
                _ignoreNextUpdate = true;
                photonView.RPC(nameof(SyncGlobalVariable), RpcTarget.Others, varId, data, _ignoreNextUpdate);
                _ignoreNextUpdate = false;
            }
            else photonView.RPC(nameof(SyncGlobalVariableMaster), RpcTarget.MasterClient, varId, data);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (!photonView.IsMine) return;

            if(debug) Debug.Log($"Global OnPlayerEnteredRoom: {newPlayer} _syncedVars: {_syncedVars.Count}");
            photonView.RPC(nameof(SyncUpdatedGlobalVariables), newPlayer, _syncedVars);
        }
        
        [PunRPC]
        private void SyncUpdatedGlobalVariables(Hashtable data)
        {
            if(debug) Debug.Log($"Global SyncUpdatedVariables: {data.Count}");

            foreach (var varData in data)
            {
                globalNameVariables.Set(varData.Key.ToString(), varData.Value);
            }
            _syncedVars = data;
        }

        [PunRPC]
        private void SyncGlobalVariable(string varId, object value, bool ignoreNextUpdate)
        {
            _ignoreNextUpdate = ignoreNextUpdate;
            if(debug) Debug.Log($"Global SyncVariable: {varId} = {value} ignoreNextUpdate {_ignoreNextUpdate}");
            globalNameVariables.Set(varId, value);
        }
        
        [PunRPC]
        private void SyncGlobalVariableMaster(string varId, object value, PhotonMessageInfo info)
        {
            if(debug) Debug.Log($"Global SyncVariableMaster: {varId} = {value} from {info.Sender}");
            _ignoreNextUpdate = true;
            globalNameVariables.Set(varId, value);
        }
    }
}
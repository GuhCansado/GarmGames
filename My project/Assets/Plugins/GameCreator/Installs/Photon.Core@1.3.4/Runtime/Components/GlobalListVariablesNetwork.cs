using System;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime
{
    [RequireComponent(typeof(PhotonView))]
    [AddComponentMenu("Game Creator/Photon/Global List Variables Network")]
    [Icon(RuntimePaths.GIZMOS + "GizmoGlobalListVariables.png")]
    public class GlobalListVariablesNetwork : MonoBehaviourPunCallbacks
    {
        private enum ListMode
        {
            PlayersList,
            PhotonInstantiation,
            SyncData
        }
        
        [SerializeField] private GlobalListVariables m_GlobalListVariables;
        [SerializeField] private ListMode m_SyncMode = ListMode.PlayersList;
        
        private readonly Hashtable _syncedVars = new();
        private bool _ignoreNextUpdate;

        private void Awake()
        {
            if(!m_GlobalListVariables) return;
            
            switch (m_SyncMode)
            {
                case ListMode.SyncData:
                {
                    if (m_GlobalListVariables.TypeID.String == ValueGameObject.TYPE_ID.String) Debug.LogWarning("LocalListVariablesNetwork does not support GameObjects.");
                    if (m_GlobalListVariables.TypeID.String == ValueTexture.TYPE_ID.String) Debug.LogWarning("LocalListVariablesNetwork does not support Textures.");
                    if (m_GlobalListVariables.TypeID.String == ValueSprite.TYPE_ID.String) Debug.LogWarning("LocalListVariablesNetwork does not support Sprites.");
                    m_GlobalListVariables.Register(OnChange);
                    break;
                }
                case ListMode.PlayersList:
                {
                    if (m_GlobalListVariables.TypeID.String != ValueGameObject.TYPE_ID.String) Debug.LogWarning("LocalListVariablesNetwork in PlayersList mode only supports GameObjects.");
                    CharacterNetwork.OnCharacterNetworkSpawned += OnCharacterNetworkSpawned;
                    CharacterNetwork.OnCharacterNetworkDespawned += OnCharacterNetworkDespawned;
                    break;
                }
                case ListMode.PhotonInstantiation:
                {
                    if (m_GlobalListVariables.TypeID.String != ValueGameObject.TYPE_ID.String) Debug.LogWarning("LocalListVariablesNetwork in PhotonInstantiation mode only supports GameObjects.");
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Start()
        {
            if(!m_GlobalListVariables) return;

            if (m_SyncMode != ListMode.PhotonInstantiation) return;
            for (var i = 0; i < m_GlobalListVariables.Count; i++)
            {
                var prefab = m_GlobalListVariables.Get(i) as GameObject;
                var pool = PhotonNetwork.PrefabPool as DefaultPool;
                if (prefab == null) continue;
                if(!pool.ResourceCache.ContainsKey(prefab.name)) pool.ResourceCache.Add(prefab.name, prefab);
            }
        }

        private void OnDestroy()
        {
            if(!m_GlobalListVariables) return;

            switch (m_SyncMode)
            {
                case ListMode.SyncData:
                    m_GlobalListVariables.Unregister(OnChange);
                    break;
                case ListMode.PlayersList:
                    CharacterNetwork.OnCharacterNetworkSpawned -= OnCharacterNetworkSpawned;
                    CharacterNetwork.OnCharacterNetworkDespawned -= OnCharacterNetworkDespawned;
                    break;
                case ListMode.PhotonInstantiation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnCharacterNetworkSpawned()
        {
            UpdatePlayerList();
        }
        
        private void OnCharacterNetworkDespawned()
        {
            UpdatePlayerList();
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.AddCallbackTarget(this);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public override void OnJoinedRoom()
        {
            // UpdatePlayerList();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(!m_GlobalListVariables) return;
            if (!photonView.IsMine) return;

            if (m_SyncMode != ListMode.SyncData) return;
            if(!PhotonNetwork.IsMasterClient) return;
            if(_syncedVars.Count > 0) photonView.RPC(nameof(RPC_SyncAllData), newPlayer, _syncedVars);
        }
        
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            // UpdatePlayerList();
        }

        private void UpdatePlayerList()
        {
            if (m_SyncMode != ListMode.PlayersList) return;

            for (var i = 0; i < m_GlobalListVariables.Count; i++)
            {
                m_GlobalListVariables.Remove(i);
            }

            for (var i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var player = PhotonNetwork.PlayerList[i];
                if(player.TagObject == null) continue;
                if(m_GlobalListVariables.Get(i) != null) m_GlobalListVariables.Set(i, player.TagObject as GameObject);
                else m_GlobalListVariables.Insert(i, player.TagObject as GameObject);
            }
        }

        [PunRPC]
        private void RPC_SyncAllData(Hashtable data)
        {
            foreach (var syncedVar in _syncedVars)
            {
                m_GlobalListVariables.Insert((int)syncedVar.Key, syncedVar.Value);
            }
        }
        
        [PunRPC]
        private void RPC_Insert(int index, object data, bool ignoreNextUpdate)
        {
            _ignoreNextUpdate = ignoreNextUpdate;
            m_GlobalListVariables.Insert(index, data);
            _syncedVars.Add(index, data);
        }
        
        [PunRPC]
        private void RPC_Set(int index, object data, bool ignoreNextUpdate)
        {
            _ignoreNextUpdate = ignoreNextUpdate;
            m_GlobalListVariables.Set(index, data);
            _syncedVars[index] = data;
        }
        
        [PunRPC]
        private void RPC_Move(int index, object data, bool ignoreNextUpdate)
        {
            _ignoreNextUpdate = ignoreNextUpdate;
            m_GlobalListVariables.Set(index, data);
            _syncedVars[index] = data;
        }
        
        [PunRPC]
        private void RPC_Remove(int index, bool ignoreNextUpdate)
        {
            _ignoreNextUpdate = ignoreNextUpdate;
            m_GlobalListVariables.Remove(index);
            _syncedVars.Remove(index);
        }

        private void OnChange(ListVariableRuntime.Change change, int index)
        {
            if(_ignoreNextUpdate && !photonView.IsMine)
            {
                _ignoreNextUpdate = false;
                return;
            }
            var data = m_GlobalListVariables.Get(index);
            if(_syncedVars.ContainsKey(index) && _syncedVars[index].Equals(data))
            {
                _ignoreNextUpdate = false;
                return;
            }
            
            if (!data.IsAllowedType()) return;
            
            _ignoreNextUpdate = true;
            switch (change)
            {
                case ListVariableRuntime.Change.Set:
                    _syncedVars[index] = data;
                    photonView.RPC(nameof(RPC_Set), RpcTarget.Others, index, data, _ignoreNextUpdate);
                    break;
                case ListVariableRuntime.Change.Move:

                    break;
                case ListVariableRuntime.Change.Insert:
                    _syncedVars.Add(index, data);
                    photonView.RPC(nameof(RPC_Insert), RpcTarget.Others, index, data, _ignoreNextUpdate);
                    break;
                case ListVariableRuntime.Change.Remove:
                    _syncedVars.Remove(index);
                    photonView.RPC(nameof(RPC_Remove), RpcTarget.Others, index, _ignoreNextUpdate);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(change), change, null);
            }
            _ignoreNextUpdate = false;

            // _syncedVars[data] = index;
            Debug.Log($"OnListChange: {change} index: {index}={data} type: {data.GetType()} _syncedVars: {_syncedVars.Count} allowed: {data.IsAllowedType()}");

            // if (photonView.IsMine)
            // {
            //     _ignoreNextUpdate = true;
            //     photonView.RPC(nameof(RPC_Insert), RpcTarget.Others, index, data, _ignoreNextUpdate);
            //     _ignoreNextUpdate = false;
            // }
            // else photonView.RPC(nameof(SyncVariableMaster), RpcTarget.MasterClient, varId, data);
        }
    }
}
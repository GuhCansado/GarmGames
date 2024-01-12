using System;
using Photon.Pun;
using Photon.Realtime;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.Components
{
    public enum VisualScriptingRpcTarget
    {
        /// <summary>Sends the RPC to everyone else and executes it immediately on this client. Player who join later will not execute this RPC.</summary>
        All,

        /// <summary>Sends the RPC to everyone else. This client does not execute the RPC. Player who join later will not execute this RPC.</summary>
        Others,

        /// <summary>Sends the RPC to MasterClient only. Careful: The MasterClient might disconnect before it executes the RPC and that might cause dropped RPCs.</summary>
        MasterClient,

        /// <summary>Sends the RPC to everyone (including this client) through the server.</summary>
        /// <remarks>
        /// This client executes the RPC like any other when it received it from the server.
        /// Benefit: The server's order of sending the RPCs is the same on all clients.
        /// </remarks>
        AllViaServer
    }
    public abstract class BaseRPC : MonoBehaviour
    {
        public Player Sender;
        public Player LocalSender;

        [SerializeField] protected RpcTarget rpcTarget = RpcTarget.Others;
        [SerializeField] protected bool sendStateToNewPlayers;

        public bool IsRunning { get; private set; }
        public bool SendStateToNewPlayers
        {
            get => sendStateToNewPlayers;
            set => sendStateToNewPlayers = value;
        }

        private GameObject _senderTag;
        private VisualScriptingNetwork _visualNetwork;
        private bool _isQuitting;

#if UNITY_EDITOR
        public void CheckNetwork(bool isValidate = false)
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode) return;
            if(!this) return;
            if(!gameObject) return;
            // if(!gameObject.activeInHierarchy) return;
            if(!isValidate)
            {
                if (!_visualNetwork) _visualNetwork = GetComponent<VisualScriptingNetwork>();
                if (!_visualNetwork) _visualNetwork = GetComponentInParent<VisualScriptingNetwork>();
                if (!_visualNetwork)
                {
                    var view = PhotonView.Get(gameObject);
                    if (!view) view = Undo.AddComponent<PhotonView>(gameObject.transform.root.gameObject);
                    _visualNetwork = Undo.AddComponent<VisualScriptingNetwork>(view.gameObject);
                    _visualNetwork.SettingUp = true;
                }
            }

            if (!_visualNetwork) return;

            if (_visualNetwork.actions.Contains(this)) return;
            _visualNetwork.actions.Add(this);
            
            EditorApplication.delayCall += FinishSetup;
        }

        private void FinishSetup()
        {   
            EditorApplication.delayCall -= FinishSetup;
            _visualNetwork.SettingUp = false;
            _visualNetwork.CleanUp();
        }
#endif
        protected virtual void Awake()
        {
            // var view = PhotonView.Get(gameObject.transform.root.gameObject);
            // if (!view) view = GetComponentInParent<PhotonView>();
            if (!_visualNetwork) _visualNetwork = GetComponent<VisualScriptingNetwork>();
            if (!_visualNetwork) _visualNetwork = GetComponentInParent<VisualScriptingNetwork>();
        }

        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        private void OnDestroy()
        {
            if(!_isQuitting && !Application.isPlaying) CleanUp(true);
        }

        public void CleanUp(bool forceDestroy = false)
        {
            if(_isQuitting) return;
            if(_visualNetwork) _visualNetwork.CleanUp(false, forceDestroy);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            EditorApplication.delayCall -= Validate;
            EditorApplication.delayCall += Validate;
        }

        private void Validate()
        {
            EditorApplication.delayCall -= Validate;
            CheckNetwork();
        }
        #endif

        protected void OnStartRunning()
        {
            if(!PhotonNetwork.InRoom) return;
            IsRunning = true;
            if (Sender != null && !Equals(Sender, PhotonNetwork.LocalPlayer)) return;
            
            LocalSender = PhotonNetwork.LocalPlayer;
            var index = _visualNetwork.actions.IndexOf(this);
            _visualNetwork.photonView.RPC(nameof(_visualNetwork.ExecuteRPC), rpcTarget, index);
        }

        private static RpcTarget GetRpcTarget(VisualScriptingRpcTarget target)
        {
            return target switch
            {
                VisualScriptingRpcTarget.All => RpcTarget.All,
                VisualScriptingRpcTarget.Others => RpcTarget.Others,
                VisualScriptingRpcTarget.MasterClient => RpcTarget.MasterClient,
                VisualScriptingRpcTarget.AllViaServer => RpcTarget.AllViaServer,
                _ => throw new ArgumentOutOfRangeException(nameof(target), target, null)
            };
        }

        protected void OnEndRunning()
        {
            IsRunning = false;
            Sender = null;
        }

        public virtual void Run(GameObject senderTag) {}
    }
}
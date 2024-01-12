#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Components
{
    [AddComponentMenu("")]
    // [Icon(RuntimePaths.GIZMOS + "GizmoInstaller.png")]
    public class VisualScriptingNetwork : MonoBehaviourPunCallbacks
    {

        public List<BaseRPC> actions = new();
        private bool destroying;
        public bool SettingUp { get; set; }

        // CONSTRUCTORS: --------------------------------------------------------------------------

        private void Awake()
        {
            // hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
            hideFlags = HideFlags.None;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            CleanUp();
            if (PhotonNetwork.UseRpcMonoBehaviourCache)
            {
                photonView.RefreshRpcMonoBehaviourCache();
            }
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
            if(EditorApplication.isPlayingOrWillChangePlaymode) return;
            CleanUp(false, true);
        }
        #endif

        public void CleanUp(bool isValidate = false, bool forceDestroy = false)
        {
            if(destroying || SettingUp)
            {
                destroying = false;
                return;
            }

            actions.RemoveAll(action => action == null);

            if (actions.Count == 0 || forceDestroy)
            {
                if((!isValidate || forceDestroy) && !destroying)
                {
                    if(actions.Count == 0)
                    {
                        if(Application.isPlaying)
                        {
                            Destroy(this);
                        }
                        else
                        {
                            DestroyImmediate(this, true);
                        }
                        destroying = true;
                    }
                }
            }

            if (destroying || this == null) return;
            var duplicates = GetComponents<VisualScriptingNetwork>();
            for (var i = 0; i < duplicates.Length; i++)
            {
                if(i == 0) continue;
                if(Application.isPlaying)
                {
                    Destroy(duplicates[i]);
                }
                else
                {
                    DestroyImmediate(duplicates[i], true);
                }
            }
        }

        [PunRPC]
        public void ExecuteRPC(int index, PhotonMessageInfo info)
        {
            var sender = info.Sender;
            var senderTag = info.Sender.TagObject as GameObject;
            if (Equals(sender, PhotonNetwork.LocalPlayer)) return;
            actions[index].Sender = sender;
            actions[index].Run(senderTag);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                var rpc = actions[i];
                if(rpc.LocalSender is not {IsLocal: true}) continue;
                if(!rpc.IsRunning && !rpc.SendStateToNewPlayers) continue;
                photonView.RPC(nameof(ExecuteRPC), newPlayer, i);
            }
        }
    }
}
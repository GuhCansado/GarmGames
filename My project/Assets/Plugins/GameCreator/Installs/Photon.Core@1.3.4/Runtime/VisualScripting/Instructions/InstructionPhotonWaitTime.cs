using System;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Wait Seconds")]
    [Description("Waits a certain amount of seconds from the Photon Network")]

    [Category("Photon/Core/Photon Wait Seconds")]

    [Parameter(
        "Seconds",
        "The amount of seconds to wait"
    )]
    
    [Keywords("Wait", "Time", "Seconds", "Minutes", "Cooldown", "Timeout", "Yield")]
    [Image(typeof(IconTimer), ColorTheme.Type.Green)]
    
    [Serializable]
    public class InstructionPhotonWaitTime : Instruction, IInRoomCallbacks
    {
        [FormerlySerializedAs("m_Seconds")] [SerializeField]
        private PropertyGetDecimal seconds = new(1f);
        
        public override string Title =>
            $"Photon Wait {seconds} {(seconds.ToString() == "1" ? "second" : "seconds")}";
        
        private string _timerId;
        private float _waitTime;
        private bool _isWaiting;
        private bool _isLocal;

        protected override async Task Run(Args args)
        {
            var targetRoot = args.Self;
            var view = PhotonView.Get(targetRoot);
            if(!view) targetRoot = args.Self.transform.root.gameObject;
            view = PhotonView.Get(targetRoot);
            if (!view)
            {
                Debug.LogWarning($"Photon Wait Time: No PhotonView found on {targetRoot}");
                return;
            }
            PhotonNetwork.AddCallbackTarget(this);
            
            _isWaiting = true;
            _timerId = $"timer-{view.ViewID}";
            if(args.Target == (GameObject) PhotonNetwork.LocalPlayer.TagObject)
            {
                _isLocal = true;
                PhotonNetwork.LocalPlayer.SetFloat(_timerId, (float) PhotonNetwork.Time + (float) seconds.Get(args), false);
            }
            
            await While(() => _isWaiting);
        }
        
        private async void WaitTimer()
        {
            while (PhotonNetwork.Time < _waitTime)
            {
                await Task.Yield();
            }
            FinishTimer();
        }
        
        private void FinishTimer()
        {
            if(!_isWaiting) return;
            _isWaiting = false;
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (_isLocal)
            {
                PhotonNetwork.LocalPlayer.SetFloat(_timerId, _waitTime, false);
            }
        }

        public void OnPlayerLeftRoom(Player otherPlayer) {}

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!changedProps.ContainsKey(_timerId)) return;

            _waitTime = (float)changedProps[_timerId];
            if (_waitTime > PhotonNetwork.Time) WaitTimer();
            else FinishTimer();
        }

        public void OnMasterClientSwitched(Player newMasterClient) {}
    }
}
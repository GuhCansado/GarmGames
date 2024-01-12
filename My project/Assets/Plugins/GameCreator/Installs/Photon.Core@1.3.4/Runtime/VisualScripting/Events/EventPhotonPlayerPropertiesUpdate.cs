using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Player Properties Update")]
    [Category("Photon/Room/On Player Properties Update")]
    [Description("Called when custom player-properties are changed.")]

    [Image(typeof(IconCharacter), ColorTheme.Type.Purple, typeof(OverlayListVariable))]

    [Keywords("Player", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonPlayerPropertiesUpdate : Event, IInRoomCallbacks
    {
        [SerializeField] private PropertyGetGameObject targetPlayer = GetGameObjectNone.Create();
        [SerializeField] private CollectorNameVariable setVariables;

        protected override void OnEnable(Trigger trigger)
        {
            base.OnEnable(trigger);
            if(PhotonNetwork.NetworkingClient != null) PhotonNetwork.AddCallbackTarget(this);
        }

        protected override void OnDisable(Trigger trigger)
        {
            base.OnDisable(trigger);
            if (PhotonNetwork.NetworkingClient != null) PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnPlayerEnteredRoom(Player newPlayer) {}
        public void OnPlayerLeftRoom(Player otherPlayer) {}
        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            var p = (GameObject)targetPlayer.TagObject;
            var go = this.targetPlayer.Get(Self);
            if (go == null)
            {
                setVariables.SetHashtableValues(changedProps);
                _ = m_Trigger.Execute(p);
            }
            else
            {
                var tp = go.GetPlayerFromView();
                if (!Equals(tp, targetPlayer)) return;

                setVariables.SetHashtableValues(changedProps);
                _ = m_Trigger.Execute(p);
            }
        }
        public void OnMasterClientSwitched(Player newMasterClient) {}
    }
}

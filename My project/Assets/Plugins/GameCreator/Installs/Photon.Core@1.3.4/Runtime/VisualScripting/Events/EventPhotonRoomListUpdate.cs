using System;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Room List Update")]
    [Category("Photon/Lobby/On Room List Update")]
    [Description("Called for any update of the room-listing while in a lobby (InLobby) on the Master Server.")]

    [Image(typeof(IconHome), ColorTheme.Type.Green, typeof(OverlayListVariable))]

    [Keywords("Lobby", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonRoomListUpdate : Event, ILobbyCallbacks
    {
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
        public void OnJoinedLobby() {}
        public void OnLeftLobby() {}
        public void OnRoomListUpdate(List<RoomInfo> roomList) { _ = m_Trigger.Execute(Self); }
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {}
    }
}

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

    [Title("On Photon Left Lobby")]
    [Category("Photon/Lobby/On Left Lobby")]
    [Description("Called after leaving a lobby.")]

    [Image(typeof(IconComputer), ColorTheme.Type.Red)]

    [Keywords("Lobby", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonLeftLobby : Event, ILobbyCallbacks
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
        public void OnLeftLobby() { _ = m_Trigger.Execute(Self); }
        public void OnRoomListUpdate(List<RoomInfo> roomList) {}
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {}
    }
}

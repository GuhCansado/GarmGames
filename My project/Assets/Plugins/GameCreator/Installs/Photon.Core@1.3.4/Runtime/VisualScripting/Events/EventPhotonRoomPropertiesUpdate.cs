using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Room Properties Update")]
    [Category("Photon/Room/On Room Properties Update")]
    [Description("Called when room properties changed. The propertiesThatChanged contain only the keys that changed.")]

    [Image(typeof(IconHome), ColorTheme.Type.Green, typeof(OverlayListVariable))]

    [Keywords("Player", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonRoomPropertiesUpdate : Event, IInRoomCallbacks
    {
        [SerializeField] private RoomPropertyName propertyName;
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

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if(propertyName.Value != null && propertiesThatChanged.ContainsKey(propertyName.ToString()) || propertyName.Value == null)
            {
                setVariables.SetHashtableValues(propertiesThatChanged);
                _ = m_Trigger.Execute(Self);
            }
            else 
            {
                setVariables.SetHashtableValues(PhotonNetwork.CurrentRoom.CustomProperties);
                _ = m_Trigger.Execute(Self);
            }
        }
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}
        public void OnMasterClientSwitched(Player newMasterClient) {}
    }
}

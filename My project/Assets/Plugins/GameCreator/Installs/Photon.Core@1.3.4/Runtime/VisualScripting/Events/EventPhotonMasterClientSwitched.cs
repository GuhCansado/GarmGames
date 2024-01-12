using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Master Client Switched")]
    [Category("Photon/Room/On Master Client Switched")]
    [Description("Called after switching to a new MasterClient when the current one leaves.")]

    [Image(typeof(IconCharacter), ColorTheme.Type.Blue)]

    [Keywords("Player", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonMasterClientSwitched : Event, IInRoomCallbacks
    {
        [SerializeField] private PropertySetGameObject setPlayer = SetGameObjectNone.Create;

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
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            var t = Self;
            if (newMasterClient.TagObject != null)
            {
                t = (GameObject) newMasterClient.TagObject;
            }
            setPlayer.Set(t, Self);
            _ = m_Trigger.Execute(t);
        }
    }
}

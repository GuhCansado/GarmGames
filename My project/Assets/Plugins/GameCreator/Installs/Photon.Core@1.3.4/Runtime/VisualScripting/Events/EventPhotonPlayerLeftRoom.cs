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

    [Title("On Photon Player Left Room")]
    [Category("Photon/Room/On Player Left Room")]
    [Description("Called when a remote player left the room or became inactive. Check otherPlayer.IsInactive.")]

    [Image(typeof(IconCharacter), ColorTheme.Type.Red, typeof(OverlayMinus))]

    [Keywords("Player", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonPlayerLeftRoom : Event, IInRoomCallbacks
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
        public void OnPlayerLeftRoom(Player otherPlayer) 
        {
            var t = Self;
            if (otherPlayer.TagObject != null)
            {
                t = (GameObject) otherPlayer.TagObject;
            }
            setPlayer.Set(t, Self);
            _ = m_Trigger.Execute(t);
        }
        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}
        public void OnMasterClientSwitched(Player newMasterClient) {}
    }
}

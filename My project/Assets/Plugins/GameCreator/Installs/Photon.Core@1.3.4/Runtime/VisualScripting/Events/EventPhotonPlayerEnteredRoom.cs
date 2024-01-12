using System;
using System.Collections;
using System.Collections.Generic;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Player Entered Room")]
    [Category("Photon/Room/On Player Entered Room")]
    [Description("Called when a remote player entered the room. This Player is already added to the playerlist.")]

    [Image(typeof(IconCharacter), ColorTheme.Type.Green, typeof(OverlayPlus))]

    [Keywords("Player", "Network", "Photon", "Connection")]

    [Serializable]
    public class EventPhotonPlayerEnteredRoom : Event, IInRoomCallbacks
    {
        [SerializeField] private PropertySetGameObject setPlayer = SetGameObjectNone.Create;
        [SerializeField] private bool waitForPlayerObject = true;

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

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            if(waitForPlayerObject) Self.Get<Trigger>().StartCoroutine(WaitForTagObject(newPlayer));
            else Trigger(newPlayer);
        }

        private IEnumerator WaitForTagObject(Player newPlayer)
        {
            while(newPlayer.TagObject == null)
            {
                yield return null;
            }

            Trigger(newPlayer);
        }

        private void Trigger(Player newPlayer)
        {
            var p = (GameObject)newPlayer.TagObject;
            setPlayer.Set(p, Self);
            _ = m_Trigger.Execute(p);
        }
        public void OnPlayerLeftRoom(Player otherPlayer) {}
        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {}
        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {}
        public void OnMasterClientSwitched(Player newMasterClient) {}
    }
}

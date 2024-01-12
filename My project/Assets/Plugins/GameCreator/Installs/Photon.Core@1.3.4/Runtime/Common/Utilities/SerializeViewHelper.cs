using System;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    public class SerializeViewHelper : MonoBehaviour, IPunObservable
    {
        private PhotonView _photonView;
        public Action<PhotonStream, PhotonMessageInfo> OnSerialize;

        private void Awake()
        {
            if (!_photonView) _photonView = gameObject.GetPhotonView();

            if (!_photonView)
            {
                Debug.LogWarningFormat("[OnPhotonSerializeView] Couldn't find any PhotonView on {0}", gameObject);
                return;
            }

            if (!_photonView.ObservedComponents.Contains(this))
            {
                _photonView.ObservedComponents.Add(this);
            }
        }
        
        private void OnEnable()
        {
            if(PhotonNetwork.NetworkingClient != null) PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            if (PhotonNetwork.NetworkingClient != null) PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            OnSerialize?.Invoke(stream, info);
        }
    }
}
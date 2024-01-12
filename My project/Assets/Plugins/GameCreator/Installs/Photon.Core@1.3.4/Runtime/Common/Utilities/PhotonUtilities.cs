using GameCreator.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    public static class PhotonUtilities
    {
        /// <summary>
        /// Returns a cached Photon View from a GameObject
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static PhotonView GetCachedPhotonView(this GameObject gameObject)
        {
            var photonView = gameObject.Get<PhotonView>();
            if(!photonView)
            {
#if UNITY_EDITOR
                Debug.LogError($"The object {gameObject} doesn't have PhotonView component.", gameObject);
#endif
                return null;
            }
            return photonView;
        }
        
        /// <summary>
        /// Returns a Photon Player from a Photon View
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static Player GetPlayerFromView(this GameObject gameObject)
        {
            var photonView = gameObject.GetCachedPhotonView();
            if (!photonView) return null;
            return photonView.Owner;
        }
    }
}
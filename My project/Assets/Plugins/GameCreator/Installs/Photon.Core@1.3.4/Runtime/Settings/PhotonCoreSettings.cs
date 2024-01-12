using System;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime
{
    [Serializable]
    public class PhotonCoreSettings
    {
        public bool monobehaviourCache;
        public int sendRate = 30;
        public int sendRateOnSerialize = 10;
        public int unreliableCommandsLimit = 10;

        public bool updatePing = true;
        public float updatePingEvery = 2f;

        public bool switchMasterClient = true;
        public float lagCheck = 3f;
        public int lagThreshold = 1000;
        public int switchMasterErrors = 5;

        public string defaultName = "Player";
    }
}
using System;
using UnityEngine;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Serializable]
    public abstract class NetworkingSettings
    {
        [HideInInspector]
        public string nodeId;
        public bool networkSync;

        protected NetworkingSettings(string nodeId)
        {
            this.nodeId = nodeId;
        }
    }
}
using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Event = GameCreator.Runtime.VisualScripting.Event;

namespace NinjutsuGames.Photon.Runtime
{

    [Title("On Photon Instantiate")]
    [Category("Photon/Core/On Instantiate")]
    [Description("Called to the PhotonView that was instantiated.")]

    [Image(typeof(IconDownload), ColorTheme.Type.Green)]

    [Keywords("Player", "Network", "Photon", "Connection", "Instantiate", "Data")]

    [Serializable]
    public class EventPhotonInstantiate : Event, IPunInstantiateMagicCallback
    {
        [SerializeField] private CollectorNameVariable instantiationData;
        private bool executed;

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            if(executed) return;
            
            executed = true;
            var view = info.photonView;
            var data = view.InstantiationData;
            if (data == null) return;
            for (int i = 0, imax = data.Length; i < imax; i++)
            {
                var key = data[i].ToString();
                var value = data[++i];
                instantiationData.Set(key, value);
            }
            
            _ = m_Trigger.Execute(Self);
        }
    }
}

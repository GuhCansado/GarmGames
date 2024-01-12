using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Start Timer")]
    [Description("Starts the room timer. Only the master client can start the timer.")]

    [Category("Photon/Core/Start Timer")]
    
    [Image(typeof(IconClock), ColorTheme.Type.Green, typeof(OverlayPlus))]

    [Keywords("Create", "Network", "Photon", "Timer")]
    [Serializable]
    public class InstructionPhotonStartTimer : Instruction
    {
        public override string Title => $"Start Timer";

        protected override Task Run(Args args)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetworkManager.SetStartTime();
            }
            return DefaultResult;
        }
    }
}
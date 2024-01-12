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
    [Description("Starts the room countdown timer. Only the master client can start the timer.")]

    [Category("Photon/Core/Start Countdown Timer")]
    [Parameter("Duration", "Defines the duration time of this Room (in seconds).")]
    
    [Image(typeof(IconClock), ColorTheme.Type.Green, typeof(OverlayMinus))]

    [Keywords("Create", "Network", "Photon", "Timer", "Countdown")]
    [Serializable]
    public class InstructionPhotonStartCountdownTimer : Instruction
    {
        [SerializeField] private PropertyGetInteger duration = new(60);

        public override string Title => $"Start Countdown Timer to {duration} seconds";

        protected override Task Run(Args args)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.SetDurationTime((int)duration.Get(args));
                PhotonNetworkManager.SetStartTime();
            }
            return DefaultResult;
        }
    }
}
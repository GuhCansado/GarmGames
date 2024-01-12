using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Disconnect")]
    [Description("Makes this client disconnect from the photon server, a process that leaves any room and calls OnDisconnected on completion.")]

    [Category("Photon/Connection/Disconnect")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Red, typeof(OverlayArrowDown))]

    [Keywords("Connect", "Network", "Photon")]
    [Serializable]
    public class InstructionPhotonDisconnect : Instruction
    {
        public override string Title => $"Photon Disconnect";

        protected override Task Run(Args args)
        {
            PhotonNetwork.Disconnect();
            return DefaultResult;
        }
    }
}
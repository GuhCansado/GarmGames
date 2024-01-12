using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Reconnect")]
    [Description("Can be used to reconnect to the master server after a disconnect.")]

    [Category("Photon/Connection/Reconnect")]
    
    [Image(typeof(IconComputer), ColorTheme.Type.Yellow)]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonReconnect : Instruction
    {
        public override string Title => $"Reconnect";

        protected override Task Run(Args args)
        {
            return Task.FromResult(PhotonNetwork.Reconnect());
        }
    }
}
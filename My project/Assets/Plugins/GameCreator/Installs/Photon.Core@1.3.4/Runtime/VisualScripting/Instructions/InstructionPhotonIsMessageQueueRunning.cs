using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Is Message Queue Running")]
    [Description("Can be used to pause dispatching of incoming events (RPCs, Instantiates and anything else incoming). Useful for pausing the game while loading a new level.")]

    [Category("Photon/Core/Is Message Queue Running")]
    
    [Parameter("Running", "If false no incoming RPCs and Instantiates will be executed.")]
    
    [Image(typeof(IconGear), ColorTheme.Type.Blue)]

    [Keywords("Queue", "Running", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonIsMessageQueueRunning : Instruction
    {
        [SerializeField] private PropertyGetBool running = new(true);

        public override string Title => $"Is Message Queue Running: {running}";

        protected override Task Run(Args args)
        {
            PhotonNetwork.IsMessageQueueRunning = running.Get(args);
            return DefaultResult;
        }
    }
}
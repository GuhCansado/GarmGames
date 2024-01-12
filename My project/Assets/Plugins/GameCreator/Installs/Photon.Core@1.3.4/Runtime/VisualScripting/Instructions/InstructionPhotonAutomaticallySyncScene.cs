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
    [Title("Photon Automatically Sync Scene")]
    [Description("Defines if all clients in a room should automatically load the same level as the Master Client.")]

    [Category("Photon/Core/Automatically Sync Scene")]
    
    [Parameter("Sync Scene", "When enabled, clients load the same scene that is active on the Master Client.")]
    
    [Image(typeof(IconGear), ColorTheme.Type.Green)]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonAutomaticallySyncScene : Instruction
    {
        [SerializeField] private PropertyGetBool syncScene = new(true);

        public override string Title => $"Automatically Sync Scene: {syncScene}";

        protected override Task Run(Args args)
        {
            PhotonNetwork.AutomaticallySyncScene = syncScene.Get(args);
            return DefaultResult;
        }
    }
}
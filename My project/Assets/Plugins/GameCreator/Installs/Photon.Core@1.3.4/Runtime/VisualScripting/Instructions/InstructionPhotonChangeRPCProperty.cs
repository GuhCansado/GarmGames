using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Components;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Change RPC Property")]
    [Description("Control RPC properties.")]

    [Category("Photon/Core/Photon Change RPC Property")]
    
    [Parameter("Target", "Game Object reference that is instantiated")]
    [Parameter("Send New State to Players", "If true when new player joins the room, it will run this RPC on the new player")]
    
    [Image(typeof(IconCog), ColorTheme.Type.Green, typeof(OverlayTick))]

    [Keywords("RPC", "Network", "Photon", "Property", "Game Object")]
    [Serializable]
    public class InstructionPhotonChangeRPCProperty : Instruction
    {
        [SerializeField] private PropertyGetGameObject target = new();
        [SerializeField] private PropertyGetBool sendNewStateToPlayers = new();

        public override string Title => $"Photon Change RPC Property: {target}";

        protected override async Task Run(Args args)
        {
            var go = target.Get(args);
            if (!go)
            {
                Debug.LogWarning($"Photon Change RPC Property: target is null");
                return;
            }
            var rpc = go.Get<BaseRPC>();
            if (!go)
            {
                Debug.LogWarning($"Photon Change RPC Property: target doesn't have an RPC component");
                return;
            }
            rpc.SendStateToNewPlayers = sendNewStateToPlayers.Get(args);
            await NextFrame();
        }
    }
}
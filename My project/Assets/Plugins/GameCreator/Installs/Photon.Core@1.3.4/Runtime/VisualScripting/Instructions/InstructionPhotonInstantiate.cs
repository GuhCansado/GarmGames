using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Common;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Photon Instantiate")]
    [Description("Instantiates a player owned object.")]

    [Category("Photon/Core/Photon Instantiate")]
    
    [Parameter("Game Object", "Game Object reference that is instantiated")]
    [Parameter("Position", "The position where the new game object is instantiated")]
    [Parameter("Rotation", "The rotation that the new game object has")]
    [Parameter("Group", "The network group where this game object is going to be instantiated")]
    [Parameter("Save", "Optional value where the newly instantiated game object is stored")]
    [Parameter("Data", "Optional data that is sent to the newly instantiated game object")]
    
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Green, typeof(OverlayPlus))]

    [Keywords("Create", "Network", "Photon", "New", "Game Object")]
    [Serializable]
    public class InstructionPhotonInstantiate : Instruction
    {
        [SerializeField] private PropertyGetGameObject gameObject = new();
        [SerializeField] private PropertyGetPosition position = GetPositionVector3.Create();
        [SerializeField] private PropertyGetRotation rotation = GetRotationIdentity.Create;
        [SerializeField] private PropertyGetInteger group = GetDecimalInteger.Create(0);
        [SerializeField] private PropertySetGameObject save = SetGameObjectNone.Create;
        [SerializeField] private CollectorNameVariable data;

        public override string Title => $"Photon Instantiate: {gameObject}";

        protected override async Task Run(Args args)
        {
            var pos = position.Get(args);
            var rot = rotation.Get(args);
            var grp = (byte)group.Get(args);
            var go = PhotonNetwork.Instantiate(gameObject.Get(args).name, pos, rot, grp, data?.ToObjectArray());
            save.Set(go, args);
            await NextFrame();
        }
    }
}
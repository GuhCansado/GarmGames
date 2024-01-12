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
    [Title("Photon Load Level Index")]
    [Description("This method wraps loading a level asynchronously and pausing network messages during the process.")]

    [Category("Photon/Core/Load Level Index")]
    
    [Parameter("Level Index", "Build-index number of the level to load. When using level numbers, make sure they are identical on all clients.")]
    
    [Image(typeof(IconUnity), ColorTheme.Type.Green)]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonLoadLevelIndex : Instruction
    {
        [SerializeField] private PropertyGetScene m_Scene = new();

        public override string Title => $"Photon Load Level: {m_Scene}";

        protected override Task Run(Args args)
        {
            PhotonNetwork.LoadLevel(m_Scene.Get(args));
            return DefaultResult;
        }
    }
}
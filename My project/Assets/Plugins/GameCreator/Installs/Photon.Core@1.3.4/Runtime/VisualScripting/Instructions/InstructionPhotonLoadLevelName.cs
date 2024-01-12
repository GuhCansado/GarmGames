using System;
using System.IO;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using GameCreator.Runtime.VisualScripting;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    /*[Title("Photon Load Level Name")]
    [Description("This method wraps loading a level asynchronously and pausing network messages during the process.")]

    [Category("Photon/Core/Load Level Name")]
    
    [Parameter("Level Name", "Name of the level to load. Make sure it's available to all clients in the same room.")]
    
    [Image(typeof(IconUnity), ColorTheme.Type.Yellow)]

    [Keywords("Create", "Network", "Photon", "Room")]
    [Serializable]
    public class InstructionPhotonLoadLevelName : Instruction
    {
        [SerializeField] private PropertyGetScene m_Scene = new();

        public override string Title => $"Photon Load Level Name: {m_Scene}";

        protected override Task Run(Args args)
        {
            var index = m_Scene.Get(args);
            var scenePath = SceneUtility.GetScenePathByBuildIndex(index);
            var sceneName = Path.GetFileNameWithoutExtension(scenePath);
            PhotonNetwork.LoadLevel(sceneName);
            return DefaultResult;
        }
    }*/
}
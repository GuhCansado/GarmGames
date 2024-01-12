using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Dont Destroy On Load")]
    [Description("Makes the target GameObject not be destroyed automatically when loading a new scene.")]
    [Category("Game Object/Dont Destroy On Load")]
    [Parameter("Target", "The target GameObject to not be destroyed automatically when loading a new scene.")]
    [Image(typeof(IconCubeSolid), ColorTheme.Type.Green, typeof(OverlayCross))]
    [Keywords("Dont", "Destroy", "On Load")]
    [Serializable]
    public class InstructionDontDestroyOnLoad : Instruction
    {
        [SerializeField] private PropertyGetGameObject target = new();

        public override string Title => $"Dont Destroy On Load: {target}";

        protected override Task Run(Args args)
        {
            var go = target.Get(args);
            if (go)
            {
                Object.DontDestroyOnLoad(go);
            }

            return DefaultResult;
        }
    }
}
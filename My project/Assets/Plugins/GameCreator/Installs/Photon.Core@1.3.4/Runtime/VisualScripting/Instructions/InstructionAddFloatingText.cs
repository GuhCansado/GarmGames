using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.VisualScripting
{
    [Title("Floating Text")]
    [Description("Shows a floating text message on a target GameObject")]

    [Category("UI/Floating Text")]
    
    [Parameter("Target", "The target GameObject to show the floating text on")]
    [Parameter("Text", "The text to show")]
    [Parameter("Settings", "The settings for the floating text")]
    
    [Image(typeof(IconUIText), ColorTheme.Type.Green, typeof(OverlayArrowUp))]

    [Keywords("Floating", "Text", "Floating Text")]
    [Serializable]
    public class InstructionAddFloatingText : Instruction
    {
        [SerializeField] private PropertyGetGameObject target = GetGameObjectTransform.Create();
        [SerializeField] private PropertyGetString text = GetStringString.Create;
        [SerializeField] private FloatingText settings = new();

        public override string Title => $"Floating Text: {text}";

        private string id;

        protected override Task Run(Args args)
        {
            var go = target.Get(args);
            var character = go.Get<Character>();
            if (character) id = character.ID.String;
            if (string.IsNullOrEmpty(id)) id = Guid.NewGuid().ToString();
            FloatingTextManager.Show(id, text.Get(args), target.Get(args).transform, settings);
            return DefaultResult;
        }
    }
}
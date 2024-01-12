using System;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Photon Runtime Model Sprite")]
    [Category("Photon/Model Sprite")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("Returns sprite preview from a model.")]

    [Serializable]
    public class GetSpriteRuntimeModelsList : PropertyTypeGetSprite
    {
        [SerializeField]
        protected FieldGetPhotonLocalModelsList m_ModelsList = new(ValueString.TYPE_ID);
        
        public override Sprite Get(Args args)
        {
            var modelConfig = m_ModelsList.Get(args) as ModelConfig;
            return modelConfig?.sprite.Get(args);
        }

        public static PropertyGetGameObject Create() => new(new GetGameObjectRuntimeModelsListByName());

        public override string String => "Photon Runtime Model Sprite";
    }
}
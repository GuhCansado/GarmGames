using System;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Photon Runtime Model")]
    [Category("Photon/Runtime Model")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("Returns a model prefab from a Runtime Model List")]

    [Serializable]
    public class GetGameObjectRuntimeModelsList : PropertyTypeGetGameObject
    {
        [SerializeField]
        protected FieldGetPhotonLocalModelsList m_ModelsList = new(ValueString.TYPE_ID);
        
        public override GameObject Get(Args args)
        {
            var modelConfig = m_ModelsList.Get(args) as ModelConfig;
            return modelConfig?.prefab.Get(args);
        }

        public static PropertyGetGameObject Create() => new(new GetGameObjectRuntimeModelsListByName());

        public override string String => "Photon Runtime Model";
    }
}
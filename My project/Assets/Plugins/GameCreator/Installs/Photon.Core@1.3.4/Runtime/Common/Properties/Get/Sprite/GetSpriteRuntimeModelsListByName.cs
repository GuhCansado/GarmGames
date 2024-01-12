using System;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Photon Runtime Model Sprite by Name")]
    [Category("Photon/Model Sprite by Name")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("Returns sprite preview from a model by its name.")]

    [Serializable]
    public class GetSpriteRuntimeModelsListByName : PropertyTypeGetSprite
    {
        [SerializeField] protected PropertyGetString m_ModelName = GetStringString.Create;
        public override Sprite Get(Args args)
        {
            return PhotonNetworkManager.RuntimeModels.TryGetValue(m_ModelName.Get(args), out var model) ? model.sprite.Get(args) : null;
        }

        public static PropertyGetGameObject Create() => new(new GetGameObjectRuntimeModelsListByName());

        public override string String => "Photon Runtime Model Sprite by Name";
    }
}
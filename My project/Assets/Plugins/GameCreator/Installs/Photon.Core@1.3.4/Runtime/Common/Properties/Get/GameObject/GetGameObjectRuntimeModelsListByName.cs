using System;
using ExitGames.Client.Photon.StructWrapping;
using GameCreator.Runtime.Common;
using NinjutsuGames.Photon.Runtime.Managers;
using Photon.Pun;
using UnityEngine;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Photon Runtime Model by Name")]
    [Category("Photon/Runtime Model by Name")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("Returns a model prefab from a Runtime Model List by its name")]

    [Serializable]
    public class GetGameObjectRuntimeModelsListByName : PropertyTypeGetGameObject
    {
        [SerializeField] protected PropertyGetString m_ModelName = GetStringString.Create;
        public override GameObject Get(Args args)
        {
            return PhotonNetworkManager.RuntimeModels.TryGetValue(m_ModelName.Get(args), out var model) ? model.prefab.Get(args) : null;
        }

        public static PropertyGetGameObject Create() => new(new GetGameObjectRuntimeModelsListByName());

        public override string String => "Photon Runtime Model by Name";
    }
}
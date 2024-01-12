using System;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace NinjutsuGames.Photon.Runtime.Common
{
    [Title("Photon Local Models List")]
    [Category("Photon/Local Models List")]
    
    [Image(typeof(IconCharacter), ColorTheme.Type.Yellow)]
    [Description("Returns the string value of a Photon Local Models List")]

    [Serializable] [HideLabelsInEditor]
    public class GetStringPhotonLocalModelsList : PropertyTypeGetString
    {
        [SerializeField]
        protected FieldGetPhotonLocalModelsList m_ModelsList = new(ValueString.TYPE_ID);

        public override string Get(Args args) => (m_ModelsList.Get(args) as ModelConfig)?.prefab.Get(args).name ?? string.Empty;

        public static PropertyGetString Create => new(new GetStringPhotonLocalModelsList());
        
        public override string String => m_ModelsList.ToString();
    }
}
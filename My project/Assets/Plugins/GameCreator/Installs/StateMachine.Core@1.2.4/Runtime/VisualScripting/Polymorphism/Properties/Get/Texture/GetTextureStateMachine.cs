using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Variable")]
    [Category("Variables/State Machine Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]
    [Description("Returns the Texture value of a State Machine Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetTextureStateMachine : PropertyTypeGetTexture
    {
        [SerializeField]
        protected FieldGetStateMachine m_Variable = new(ValueTexture.TYPE_ID);

        public override Texture Get(Args args) => m_Variable.Get<Texture>(args);
        public override Texture Get(GameObject gameObject) => m_Variable.Get<Texture>(new Args(gameObject));

        public override string String => m_Variable.ToString();
    }
}
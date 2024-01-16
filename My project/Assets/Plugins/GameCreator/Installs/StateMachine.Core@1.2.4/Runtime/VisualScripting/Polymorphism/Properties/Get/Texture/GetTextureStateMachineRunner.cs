using System;
using UnityEngine;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;
using NinjutsuGames.StateMachine.Runtime.Common;
using NinjutsuGames.StateMachine.Runtime.Variables;

namespace NinjutsuGames.StateMachine.Runtime
{
    [Title("State Machine Runner Variable")]
    [Category("Variables/State Machine Runner Variable")]
    
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]
    [Description("Returns the Texture value of a State Machine Runner Variable")]

    [Serializable] [HideLabelsInEditor]
    public class GetTextureStateMachineRunner : PropertyTypeGetTexture
    {
        [SerializeField]
        protected FieldGetStateMachineRunner m_Variable = new(ValueTexture.TYPE_ID);

        public override Texture Get(Args args) => m_Variable.Get<Texture>(args);

        public override string String => m_Variable.ToString();
    }
}
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
    
    [Description("Sets the Texture value of a State Machine Runner Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]

    [Serializable] [HideLabelsInEditor]
    public class SetTextureStateMachineRunner : PropertyTypeSetTexture
    {
        [SerializeField]
        protected FieldSetStateMachineRunner m_Variable = new(ValueTexture.TYPE_ID);

        public override void Set(Texture value, Args args) => m_Variable.Set(value, args);
        public override Texture Get(Args args) => m_Variable.Get(args) as Texture;
        public static PropertySetTexture Create => new(
            new SetTextureStateMachineRunner()
        );
        
        public override string String => m_Variable.ToString();
    }
}
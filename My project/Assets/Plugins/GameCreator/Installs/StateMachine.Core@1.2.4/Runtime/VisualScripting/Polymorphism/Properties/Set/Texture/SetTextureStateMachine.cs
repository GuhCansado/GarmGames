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
    
    [Description("Sets the Texture value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable] [HideLabelsInEditor]
    public class SetTextureStateMachine : PropertyTypeSetTexture
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueTexture.TYPE_ID);

        public override void Set(Texture value, Args args) => m_Variable.Set(value, args);
        public override Texture Get(Args args) => m_Variable.Get(args) as Texture;
        
        public static PropertySetTexture Create => new(
            new SetTextureStateMachine()
        );
        
        public override string String => m_Variable.ToString();
    }
}
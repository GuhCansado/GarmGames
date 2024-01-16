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
    
    [Description("Sets the Color value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable] [HideLabelsInEditor]
    public class SetColorStateMachine : PropertyTypeSetColor
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueColor.TYPE_ID);

        public override void Set(Color value, Args args) => m_Variable.Set(value, args);
        public override Color Get(Args args) => (Color) m_Variable.Get(args);
        
        public static PropertySetColor Create => new(
            new SetColorStateMachine()
        );
        
        public override string String => m_Variable.ToString();
    }
}
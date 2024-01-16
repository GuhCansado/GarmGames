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
    
    [Description("Sets the boolean value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable] [HideLabelsInEditor]
    public class SetBoolStateMachine : PropertyTypeSetBool
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueBool.TYPE_ID);

        public override void Set(bool value, Args args) => m_Variable.Set(value, args);
        public override bool Get(Args args) => (bool) m_Variable.Get(args);
        
        public static PropertySetBool Create => new(
            new SetBoolStateMachine()
        );
        
        public override string String => m_Variable.ToString();
    }
}
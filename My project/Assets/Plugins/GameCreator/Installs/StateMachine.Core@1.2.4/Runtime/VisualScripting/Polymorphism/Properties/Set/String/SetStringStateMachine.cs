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
    
    [Description("Sets the string value of a State Machine Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Blue)]

    [Serializable] [HideLabelsInEditor]
    public class SetStringStateMachine : PropertyTypeSetString
    {
        [SerializeField]
        protected FieldSetStateMachine m_Variable = new(ValueString.TYPE_ID);

        public override void Set(string value, Args args) => m_Variable.Set(value, args);
        public override string Get(Args args) => m_Variable.Get(args).ToString();
        
        public static PropertySetString Create => new(
            new SetStringStateMachine()
        );
        
        public override string String => m_Variable.ToString();
    }
}
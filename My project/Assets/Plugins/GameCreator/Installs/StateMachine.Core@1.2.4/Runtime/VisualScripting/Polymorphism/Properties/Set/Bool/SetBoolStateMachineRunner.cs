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
    
    [Description("Sets the boolean value of a State Machine Runner Variable")]
    [Image(typeof(IconStateMachine), ColorTheme.Type.Yellow, typeof(OverlayBolt))]

    [Serializable] [HideLabelsInEditor]
    public class SetBoolStateMachineRunner : PropertyTypeSetBool
    {
        [SerializeField] protected FieldSetStateMachineRunner m_Variable = new(ValueBool.TYPE_ID);

        public override void Set(bool value, Args args) => m_Variable.Set(value, args);

        public override bool Get(Args args) => (bool) m_Variable.Get(args);
        
        public static PropertySetBool Create => new(
            new SetBoolStateMachineRunner()
        );
        
        public override string String => m_Variable.ToString();
    }
}